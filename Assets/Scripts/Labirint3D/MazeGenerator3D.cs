using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator3D
{

    private int Width = 23;
    private int Height = 15;

    public MazeGenerator3D(int width, int height) {
        Width = width;
        Height = height;
    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */

    /// <summary>
    // Генерируем лабиринт
    /// </summary>
    public Maze3D GenerateMaze(Vector3 cellSize)
    {

        MazeCell3D[,] cells = new MazeCell3D[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {

                cells[x, z] = new MazeCell3D { X = x, Z = z, XReal = x * cellSize.x , ZReal = z * cellSize.z };
            }
        }

        RemoveWalls(cells);

        Maze3D maze = new Maze3D();
        maze.Cells = cells;

        maze.Width = Width;
        maze.Height = Height;
        maze.CellWidth = cellSize.x;
        maze.CellHeight = cellSize.z;

        maze.FinishPosition = PlaceMaxeExit(cells);

        return maze;
    }


    public void RemoveWalls(MazeCell3D[,] maze)
    {

        MazeCell3D current = maze[0, 0];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<MazeCell3D> stack = new Stack<MazeCell3D>();

        do
        {
            int x = current.X;
            int z = current.Z;

            List<MazeCell3D> unvisitedNeighbours = new List<MazeCell3D>();
            
            if (x > 0 && !maze[x - 1, z].Visited)
                unvisitedNeighbours.Add(maze[x - 1, z]);
            if (x < Width - 2 && !maze[x + 1, z].Visited)
                unvisitedNeighbours.Add(maze[x + 1, z]);
            if (z > 0 && !maze[x, z - 1].Visited)
                unvisitedNeighbours.Add(maze[x, z - 1]);
            if (z < Height - 2 && !maze[x, z + 1].Visited)
                unvisitedNeighbours.Add(maze[x, z + 1]);

            if (unvisitedNeighbours.Count > 0) {
                MazeCell3D choosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, choosen);

                choosen.Visited = true;
                stack.Push(choosen);

                choosen.DistanceFromStart = current.DistanceFromStart + 1;

                current = choosen;
            } else
                current = stack.Pop();

        } while (stack.Count > 0);


        /// Убираем лишние стены сверху
        int zMax = Height - 1;
        for (int x = 0; x < Width; x++) {
            maze[x, zMax].LeftWall = false;
            maze[x, zMax].Floor = false;
        }

        /// Убираем лишние стены справа
        int xMax = Width - 1;
        for (int z = 0; z < Height; z++)
        {
            maze[xMax, z].BottomWall = false;
            maze[xMax, z].Floor = false;
        }

    }

    private void RemoveWall(MazeCell3D a, MazeCell3D b) {
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


    private Vector2Int PlaceMaxeExit(MazeCell3D[,] maze)
    {
        MazeCell3D finish = maze[0,0];

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

        return new Vector2Int(finish.X, finish.Z);
    }

}
