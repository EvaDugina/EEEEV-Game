using System.Collections.Generic;
using UnityEngine;


public class MazeCell
{

    public int X;
    //public int Y = 0;
    public int Z;

    public bool LeftWall = true;
    public bool BottomWall = true;

    public bool Visited = false;

}

public class MazeGenerator : MonoBehaviour
{

    public int Width = 23;
    public int Height = 15;


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */

    /// <summary>
    // Генерируем лабиринт
    /// </summary>
    public MazeCell[,] GenerateMaze()
    {

        MazeCell[,] maze = new MazeCell[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {

                maze[x, z] = new MazeCell { X = x, Z = z };
            }
        }

        RemoveWalls(maze);

        return maze;
    }

    public void RemoveWalls(MazeCell[,] maze)
    {

        MazeCell current = maze[0, 0];
        current.Visited = true;

        Stack<MazeCell> stack = new Stack<MazeCell>();

        do
        {
            int x = current.X;
            int z = current.Z;

            List<MazeCell> unvisitedNeighbours = new List<MazeCell>();
            
            if (x > 0 && !maze[x - 1, z].Visited)
                unvisitedNeighbours.Add(maze[x - 1, z]);
            if (x < Width - 2 && !maze[x + 1, z].Visited)
                unvisitedNeighbours.Add(maze[x + 1, z]);
            if (z > 0 && !maze[x, z - 1].Visited)
                unvisitedNeighbours.Add(maze[x, z - 1]);
            if (z < Height - 2 && !maze[x, z + 1].Visited)
                unvisitedNeighbours.Add(maze[x, z + 1]);

            if (unvisitedNeighbours.Count > 0) {
                MazeCell choosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, choosen);
                choosen.Visited = true;
                current = choosen;
                stack.Push(choosen);
            } else
                current = stack.Pop();

        } while (stack.Count > 0);


        /// Убираем лишние стены сверху
        int zMax = Height - 1;
        for (int x = 0; x < Width; x++) {
            maze[x, zMax].LeftWall = false;
        }

        /// Убираем лишние стены справа
        int xMax = Width - 1;
        for (int z = 0; z < Height; z++)
        {
            maze[xMax, z].BottomWall = false;
        }

    }

    private void RemoveWall(MazeCell a, MazeCell b) {
        if (a.X == b.X)
        {
            if (a.Z > b.Z) a.BottomWall = false;
            else b.BottomWall = false;
        }
        else if (a.Z == b.Z)
        {
            if (a.X > b.X) a.LeftWall = false;
            else b.LeftWall = false;
        }
    }

}
