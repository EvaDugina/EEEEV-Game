using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    public int Id;

    public MazeCell[][] Cells;

    public int Width;
    public int Height;

    public int X;
    public int Y;
    public int ZIndex;

    public Vector2Int CellSize = Vector2Int.one;

    public MazeInfo Info;

    public Vector2Int StartPosition;
    public Vector2Int FinishPosition;

    public List<PortalIn> PortalsIn;
    public List<PortalOut> PortalsOut;


    public Maze(int id, int width, int height, int x, int y, MazeAreaType type=MazeAreaType.Main) {
        Id = id;
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



