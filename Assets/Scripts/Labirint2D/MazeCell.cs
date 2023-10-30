
using System;

public enum MazeCellType
{
    Default,
    Portal,
    Start,
    Finish
}

public struct MazeCellWallsStatus {
    public bool TopWall;
    public bool RightWall;
    public bool BottomWall;
    public bool LeftWall;
}

//public enum CellFloorType
//{
//    Empty,
//    Wheat
//}

//public enum CellWallType
//{
//    Default,
//    Invisible,
//    Red,
//    White
//}

//public enum ColumnType
//{
//    Default,
//    Crossroad,
//    Solid,
//}

public class MazeCell : ICloneable
{

    public int X;
    public int Y;

    public MazeCellType Type;
    public MazeCellWallsStatus WallsStatus;

    public int DistanceFromStart;

    //public bool ToptWall = false;
    //public bool RightWall = false;
    //public bool LeftWall = true;
    //public bool BottomWall = true;
    //public bool Floor = true;

    //public bool Visited = false;

    //public int DistanceFromStart;
    //public ColumnType BottomLeftColumnType;
    //public ColumnType TopRightColumnType;

    //public CellType Type;
    //public string DestinationMazeName;

    //public CellFloorType FloorType;
    //public CellWallType WallType;


    public MazeCell(int x, int y, MazeCellType type)
    {
        X = x;
        Y = y;

        Type = type;

        WallsStatus.TopWall = false;
        WallsStatus.RightWall = false;
        WallsStatus.BottomWall = false;
        WallsStatus.LeftWall = false;

        DistanceFromStart = -1;

        //BottomLeftColumnType = ColumnType.Default;
        //TopRightColumnType = ColumnType.Default;

        //Type = CellType.Default;
        //DestinationMazeName = "";

        //FloorType = CellFloorType.Empty;
        //WallType = CellWallType.Default;
    }

    public object Clone() => MemberwiseClone();


    public void RemoveAllWalls()
    {
        WallsStatus.TopWall = false;
        WallsStatus.RightWall = false;
        WallsStatus.BottomWall = false;
        WallsStatus.LeftWall = false;

        //BottomLeftColumnType = ColumnType.Default;
        //TopRightColumnType = ColumnType.Default;
    }

}
