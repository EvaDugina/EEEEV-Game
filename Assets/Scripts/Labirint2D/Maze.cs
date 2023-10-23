using UnityEngine;


public enum MazeStructureType
{
    Main,
    Field,
    Room,
    Corridor
}

public enum MazeFillType
{
    Empty,
    Wheat,
    BirchGrove
}

public enum MazeWallType {
    Default,
    Invisible,
    Red,
    White
}

public struct MazeInfo
{
    public MazeStructureType StructureType;
    public MazeFillType FillType;
    public MazeWallType WallType;
};

public class Maze
{
    public MazeCell[][] Cells;

    public int Width;
    public int Height;

    public int X;
    public int Y;
    public int ZIndex;

    public Vector2Int CellSize = Vector2Int.one;

    public MazeInfo Info;

    public Vector2Int FinishPosition;


    public Maze(int width, int height, int x, int y, MazeStructureType type=MazeStructureType.Main) {
        Width = width;
        Height = height;
        X = x;
        Y = y;

        Info.StructureType = type;
        Info.FillType = MazeFillType.Empty;
        Info.WallType = MazeWallType.Default;

        if (Info.StructureType == MazeStructureType.Main) ZIndex = 0;
        else ZIndex = 1;
    }


    public string GetMazeStructureTypeAsText() { 
        switch (Info.StructureType)
        {
            case MazeStructureType.Field:
                return "Field";
            case MazeStructureType.Room:
                return "Room";
            case MazeStructureType.Corridor:
                return "Corridor";
            default:
                return "Main";
        }
    }

};



