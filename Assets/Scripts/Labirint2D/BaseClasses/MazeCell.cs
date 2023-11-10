
using System;
using System.Collections.Generic;

public enum MazeCellType
{
    Default,
    Portal,
    Start,
    Finish
}

public enum MazeCellStatus
{
    Enable, Disable
}

public struct MazeCellWallsStatus
{
    public bool TopWall;
    public bool RightWall;
    public bool BottomWall;
    public bool LeftWall;
}

public struct MazeCellColumnsStatus
{
    public bool TopLeft;
    public bool TopRight;
    public bool BottomLeft;
    public bool BottomRight;
}

public class MazeCell : ICloneable
{

    public int X;
    public int Y;

    public MazeCellType Type;
    public MazeCellStatus Status;

    public MazeCellWallsStatus WallsStatus;
    public MazeCellColumnsStatus ColumnsStatus;

    public int DistanceFromStart;


    public MazeCell(int x, int y, MazeCellType type, MazeCellStatus status = MazeCellStatus.Enable)
    {
        X = x;
        Y = y;

        Type = type;
        Status = status;

        WallsStatus.TopWall = true;
        WallsStatus.RightWall = true;
        WallsStatus.BottomWall = true;
        WallsStatus.LeftWall = true;

        ColumnsStatus = new MazeCellColumnsStatus()
        {
            TopLeft = true,
            TopRight = true,
            BottomLeft = true,
            BottomRight = true
        };

        DistanceFromStart = -1;

    }

    public object Clone() => MemberwiseClone();

    public void SetStatus(MazeCellStatus status)
    {
        Status = status;
    }


    public void RemoveAllWalls()
    {
        WallsStatus.TopWall = false;
        WallsStatus.RightWall = false;
        WallsStatus.BottomWall = false;
        WallsStatus.LeftWall = false;
    }

}
