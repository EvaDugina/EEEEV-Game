using System.Collections.Generic;
using UnityEngine;


public enum MazeType { 
    Default,
    Boundary
}
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
    public MazeAreaType AreaType;
    public MazeDecorationType DecorationType;
    //public MazeFillType FillType;
    //public MazeWallType WallType;
};
public struct PortalIn
{
    public int FromMazeId;
    public Vector2Int Position;
};

public struct PortalOut
{
    public int ToMazeId;
    public Vector2Int Position;
};


public class Maze
{
    public string Id;
    public MazeType Type;

    public MazeCell[][] Cells;

    public int Width;
    public int Height;

    public float X;
    public float Y;
    public int ZIndex;

    public Vector2Int CellSize = Vector2Int.one;

    public MazeInfo Info;

    public Vector2Int StartPosition { get; set; }
    public Vector2Int FinishPosition { get; set; }

    public List<PortalIn> PortalsIn;
    public List<PortalOut> PortalsOut;

    public List<Maze> BoundaryMazes;


    public Maze(string id, int width, int height, float x, float y, MazeType type=MazeType.Default, MazeAreaType areaType=MazeAreaType.Main) {
        Id = id;
        Width = width;
        Height = height;
        X = x;
        Y = y;

        Type = type;

        Info.AreaType = areaType;
        Info.DecorationType = MazeDecorationType.Empty;
        //Info.FillType = CellFillType.Empty;
        //Info.WallType = CellWallType.Default;

        if (Info.AreaType == MazeAreaType.Main) ZIndex = 0;
        else ZIndex = -1;

        BoundaryMazes = new List<Maze>();
    }


    public string GetMazeStructureTypeAsText() { 
        switch (Info.AreaType)
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

    public void SetStartPosition(Vector2Int start) {
        FinishPosition = start;
        Cells[start.x][start.y].Type = CellType.Start;
    }

    public void SetFinishPosition(Vector2Int finish)
    {
        FinishPosition = finish;
        Cells[finish.x][finish.y].Type = CellType.Finish;
    }

    public void SetPortalsIn(List<PortalIn> portals) {
        PortalsIn = portals;
    }

    public void SetPortalsOut(List<PortalOut> portals)
    {
        foreach (PortalOut portal in portals)
        {
            Cells[portal.Position.x][portal.Position.y].Type = CellType.Portal;
            Cells[portal.Position.x][portal.Position.y].DestinationMazeId = portal.ToMazeId;
        }
        PortalsOut = portals;
    }

};



