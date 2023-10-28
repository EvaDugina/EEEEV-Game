
using System;

public enum CellType
{
    Default,
    Portal,
    Start,
    Finish
}

public enum CellFloorType
{
    Empty,
    Wheat
}

public enum CellWallType
{
    Default,
    Invisible,
    Red,
    White
}

public enum ColumnType
{
    Default,
    Crossroad,
    Solid,
}

public class MazeCell : ICloneable
{

    public int X;
    //public int Y = 0;
    public int Y;

    //public float XReal;
    //public float YReal;

    public bool ToptWall = false;
    public bool RightWall = false;
    public bool LeftWall = true;
    public bool BottomWall = true;
    public bool Floor = true;

    public bool Visited = false;

    public int DistanceFromStart;
    public ColumnType BottomLeftColumnType;
    public ColumnType TopRightColumnType;

    public CellType Type;
    public string DestinationMazeName;

    public CellFloorType FloorType;
    public CellWallType WallType;


    public MazeCell(int x, int y)
    {
        X = x;
        Y = y;

        BottomLeftColumnType = ColumnType.Default;
        TopRightColumnType = ColumnType.Default;

        Type = CellType.Default;
        DestinationMazeName = "";

        FloorType = CellFloorType.Empty;
        WallType = CellWallType.Default;
    }

    public object Clone() => MemberwiseClone();


    public void RemoveAllWalls()
    {
        ToptWall = false;
        RightWall = false;
        LeftWall = false;
        BottomWall = false;

        BottomLeftColumnType = ColumnType.Default;
        TopRightColumnType = ColumnType.Default;
    }

}
