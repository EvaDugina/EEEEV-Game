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
    public int DistanceFromStart;

}

public class MazeGenerator
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

        PlaceMaxeExit(maze);

        return maze;
    }


    public void RemoveWalls(MazeCell[,] maze)
    {

        MazeCell current = maze[0, 0];
        current.Visited = true;
        current.DistanceFromStart = 0;

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
                stack.Push(choosen);

                choosen.Visited = true;
                choosen.DistanceFromStart = stack.Count;

                current = choosen;
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


    private void PlaceMaxeExit(MazeCell[,] maze)
    {
        MazeCell finish = maze[0,0];

        for (int x = 0; x < maze.GetLength(0); x++) {
            if (maze[x, Height - 2].DistanceFromStart > finish.DistanceFromStart) finish = maze[x, Height - 2];
            if (maze[x, 0].DistanceFromStart > finish.DistanceFromStart) finish = maze[x, 0];
        }
        
        for (int z = 0; z < maze.GetLength(1); z++) {
            if (maze[Width-2, z].DistanceFromStart > finish.DistanceFromStart) finish = maze[Width - 2, z];
            if (maze[0, z].DistanceFromStart > finish.DistanceFromStart) finish = maze[0, z];
        }

        if (finish.X == 0) finish.LeftWall = false;
        else if (finish.Z == 0) finish.BottomWall = false;
        else if (finish.X == Width - 2) maze[finish.X + 1, finish.Z].LeftWall = false;
        else if (finish.Z == Height - 2) maze[finish.X, finish.Z+1].BottomWall = false;
    }

}
