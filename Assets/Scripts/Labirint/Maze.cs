using JetBrains.Annotations;
using System;
using UnityEngine;


public class MazeCell
{

    public int X;
    //public int Y = 0;
    public int Z;

    public float XReal;
    public float ZReal;

    public bool LeftWall = true;
    public bool BottomWall = true;
    public bool Floor = true;

    public bool Visited = false;
    public int DistanceFromStart;

}

public class Maze {
    public MazeCell[,] Cells;

    public int Width = 23;
    public int Height = 15;

    public float CellWidth;
    public float CellHeight;

    public Vector2Int FinishPosition;

}
