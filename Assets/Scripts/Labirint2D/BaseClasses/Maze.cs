using UnityEngine;

public enum MazeType
{
    Main,
    Boundary
}

public enum MazeSide
{
    Center, Left, Top, Right, Bottom,
    TopLeft, TopRight, BottomRight, BottomLeft
}


public class Maze
{

    public int Width { get; private set; }
    public int Height { get; private set; }

    public MazeSide Side { get; private set; }
    public MazeType Type { get; private set; }

    public Vector2Int StartPosition { get; private set; }
    public Vector2Int FinishPosition { get; private set; }

    public MazeCell[][] Cells { get; private set; }


    public Maze(int width, int height, MazeSide side)
    {

        Width = width;
        Height = height;

        SetSide(side);

        StartPosition = -Vector2Int.one;
        FinishPosition = -Vector2Int.one;
    }


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   SETTERS
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/


    public void SetSide(MazeSide orientation)
    {
        Side = orientation;
        if (Side == MazeSide.Center) Type = MazeType.Main;
        else Type = MazeType.Boundary;
    }

    public void SetStartPosition(Vector2Int start)
    {
        StartPosition = start;
        Cells[start.x][start.y].SetType(MazeCellType.Start);
    }

    public void SetFinishPosition(Vector2Int finish)
    {
        FinishPosition = finish;
        Cells[finish.x][finish.y].SetType(MazeCellType.Finish);
    }

    public void SetCells(MazeCell[][] cells) { 
        Cells = cells;
    }


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   GETTERS
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   STATIC
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/


    public string GetMazeSideAsText()
    {
        switch (Side)
        {
            case MazeSide.Top:
                return "Top";
            case MazeSide.Left:
                return "Left";
            case MazeSide.Right:
                return "Right";
            case MazeSide.Bottom:
                return "Bottom";

            case MazeSide.TopLeft:
                return "TopLeft";
            case MazeSide.TopRight:
                return "TopRight";
            case MazeSide.BottomRight:
                return "BottomRight";
            case MazeSide.BottomLeft:
                return "BottomLeft";

            default:
                return "Center";
        }
    }


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Utilities
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/


    public void AddPortal(Vector2Int position)
    {
        Cells[position.x][position.y].SetType(MazeCellType.Portal);
    }

};



