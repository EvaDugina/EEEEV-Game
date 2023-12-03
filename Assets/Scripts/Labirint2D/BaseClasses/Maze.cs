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


    public void SetSide(MazeSide side)
    {
        Side = side;
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

    public void SetCells(MazeCell[][] cells)
    {
        Cells = cells;
    }


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   GETTERS
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/

    public bool IsBoundaryCell(Vector2Int cellPosition)
    {
        return cellPosition.x == 0 || cellPosition.y == 0 || cellPosition.x == Width - 1 || cellPosition.y == Height - 1;
    }
    public bool HasWayBetweenCells(Vector2Int cellPosition1, Vector2Int cellPosition2)
    {
        if (cellPosition1.x == cellPosition2.x)
        {
            if (cellPosition1.y > cellPosition2.y)
            {
                return !HasWallBySide(cellPosition1, MazeCellWallSide.Bottom) && !HasWallBySide(cellPosition1, MazeCellWallSide.Top);
            }
            else
            {
                return !HasWallBySide(cellPosition1, MazeCellWallSide.Top) && !HasWallBySide(cellPosition1, MazeCellWallSide.Bottom);
            }
        }
        else if (cellPosition1.y == cellPosition2.y)
        {
            if (cellPosition1.x > cellPosition2.x)
            {
                return !HasWallBySide(cellPosition1, MazeCellWallSide.Left) && !HasWallBySide(cellPosition1, MazeCellWallSide.Right);
            }
            else
            {
                return !HasWallBySide(cellPosition1, MazeCellWallSide.Right) && !HasWallBySide(cellPosition1, MazeCellWallSide.Left);
            }
        }

        return false;
    }

    public bool HasWallBySide(Vector2Int cellPosition, MazeCellWallSide wallSide)
    {
        if (wallSide == MazeCellWallSide.Left)
            return Cells[cellPosition.x][cellPosition.y].WallsStatus.LeftWall;
        if (wallSide == MazeCellWallSide.Top)
            return Cells[cellPosition.x][cellPosition.y].WallsStatus.TopWall;
        if (wallSide == MazeCellWallSide.Right)
            return Cells[cellPosition.x][cellPosition.y].WallsStatus.RightWall;
        if (wallSide == MazeCellWallSide.Bottom)
            return Cells[cellPosition.x][cellPosition.y].WallsStatus.BottomWall;


        return false;
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


    public void AddPortal(Vector2Int position)
    {
        Cells[position.x][position.y].SetType(MazeCellType.Portal);
    }

};



