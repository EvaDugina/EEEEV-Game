
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

    public int X { get; private set; }
    public int Y { get; private set; }

    public MazeCellType Type { get; private set; }
    public MazeCellStatus Status { get; private set; }

    public MazeCellWallsStatus WallsStatus;
    public MazeCellColumnsStatus ColumnsStatus;

    public int DistanceFromStart { get; private set; }


    public MazeCell(int x, int y, MazeCellType type, MazeCellStatus status = MazeCellStatus.Enable)
    {
        X = x;
        Y = y;

        Type = type;
        Status = status;

        WallsStatus = new MazeCellWallsStatus()
        {
            TopWall = true,
            RightWall = true,
            BottomWall = true,
            LeftWall = true
        };

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


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   SETTERS
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/

    public void SetXY(int x, int y) {
        X = x;
        Y=y;
    }

    public void SetStatus(MazeCellStatus status)
    {
        Status = status;
    }

    public void SetType(MazeCellType type)
    {
        Type = type;
    }

    public void SetDistanceFromStart(int distance)
    {
        DistanceFromStart = distance;
    }

    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   UTILITIES
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/

    public void EnableAllWalls() {
        WallsStatus = new MazeCellWallsStatus()
        {
            TopWall = true,
            RightWall = true,
            BottomWall = true,
            LeftWall = true
        };
    }

    public void DisableAllWalls()
    {

        WallsStatus = new MazeCellWallsStatus()
        {
            TopWall = false,
            RightWall = false,
            BottomWall = false,
            LeftWall = false
        };

    }

    public void DisableTopWall() {
        WallsStatus.TopWall = false;
    }

    public void DisableBottomWall() {
        WallsStatus.BottomWall = false;
    }

}
