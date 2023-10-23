using UnityEngine;


public enum MazeAreaType
{
    Main,
    Field,
    Room,
    Corridor
}

public enum MazeDecorationType { 
    Empty,
    WheatField,
    BirchGrove,
    Room
}

public struct MazeInfo
{
    public MazeAreaType StructureType;
    public MazeDecorationType DecorationType;
    //public MazeFillType FillType;
    //public MazeWallType WallType;
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


    public Maze(int width, int height, int x, int y, MazeAreaType type=MazeAreaType.Main) {
        Width = width;
        Height = height;
        X = x;
        Y = y;

        Info.StructureType = type;
        Info.DecorationType = MazeDecorationType.Empty;
        //Info.FillType = CellFillType.Empty;
        //Info.WallType = CellWallType.Default;

        if (Info.StructureType == MazeAreaType.Main) ZIndex = 0;
        else ZIndex = -1;
    }


    public string GetMazeStructureTypeAsText() { 
        switch (Info.StructureType)
        {
            case MazeAreaType.Field:
                return "Field";
            case MazeAreaType.Room:
                return "Room";
            case MazeAreaType.Corridor:
                return "Corridor";
            default:
                return "Main";
        }
    }

};



