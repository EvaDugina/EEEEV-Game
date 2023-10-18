using UnityEngine;


public class MazeCell
{

    public int X;
    //public int Y = 0;
    public int Z;

    public bool LeftWall = true;
    public bool BottomWall = true;
    public bool Floor = true;

    public bool Visited = false;
    public int DistanceFromStart;

}

public class Maze {
    public MazeCell[,] Cells;
    public Vector2Int FinishPosition;

}
