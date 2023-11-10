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

    public int Width;
    public int Height;

    public MazeSide Side { get; set; }
    public MazeType Type { get; set; }

    public Vector2Int StartPosition { get; set; }
    public Vector2Int FinishPosition { get; set; }

    public MazeCell[][] Cells;


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
        Cells[start.x][start.y].Type = MazeCellType.Start;
    }

    public void SetFinishPosition(Vector2Int finish)
    {
        FinishPosition = finish;
        Cells[finish.x][finish.y].Type = MazeCellType.Finish;
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
        Cells[position.x][position.y].Type = MazeCellType.Portal;
    }

};



