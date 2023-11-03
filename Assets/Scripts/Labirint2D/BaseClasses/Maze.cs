using UnityEngine;

public enum MazeType { 
    Main,
    Boundary
}

public enum MazeSide { 
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


    public Maze(int width, int height, MazeSide side) {

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


    public void SetSide(MazeSide orientation) {
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


    public Vector2 GetMazePositionInsideArea(MazeSide side)
    {
        switch (side)
        {

            case MazeSide.Left:
                return new Vector2(-Width, 0);
            case MazeSide.Right:
                return new Vector2(Width, 0);
            case MazeSide.Top:
                return new Vector2(0, Height);
            case MazeSide.Bottom:
                return new Vector2(0, -Height);

            case MazeSide.TopLeft:
                return GetMazePositionInsideArea(MazeSide.Top) + GetMazePositionInsideArea(MazeSide.Left);
            case MazeSide.TopRight:
                return GetMazePositionInsideArea(MazeSide.Top) + GetMazePositionInsideArea(MazeSide.Right);
            case MazeSide.BottomLeft:
                return GetMazePositionInsideArea(MazeSide.Bottom) + GetMazePositionInsideArea(MazeSide.Left);
            case MazeSide.BottomRight:
                return GetMazePositionInsideArea(MazeSide.Bottom) + GetMazePositionInsideArea(MazeSide.Right);

            default:
                return Vector2.zero;
        }
    }


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


    public void AddPortal(Vector2Int position) {
        Cells[position.x][position.y].Type = MazeCellType.Portal;
    }

};



