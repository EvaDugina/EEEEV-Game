using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum MazeType { 
    Main,
    Boundary
}

//public enum MazeAreaType
//{
//    Main,
//    Field,
//    Room,
//    Corridor
//}

//public enum MazeDecorationType { 
//    Empty,
//    WheatField,
//    BirchGrove,
//    Room
//}

//public struct MazeInfo
//{
//    public MazeAreaType AreaType;
//    public MazeDecorationType DecorationType;
//    //public MazeFillType FillType;
//    //public MazeWallType WallType;
//};

public struct Portal
{
    public string ToMazeName;
    public Vector2Int Position;
};


public class Maze
{
    //public string Name;


    public int Width;
    public int Height;
    public MazeType Type;
    public MazeStructure Structure;

    public Vector2Int StartPosition { get; set; }
    public Vector2Int FinishPosition { get; set; }

    public MazeCell[][] Cells;
    public List<Portal> Portals;




    //public float X;
    //public float Y;
    //public int ZIndex;

    //public Vector2Int CellSize = Vector2Int.one;

    //public MazeInfo Info;


    //public List<Maze> BoundaryMazes;


    public Maze(int width, int height, MazeType type, MazeStructure structure) {
        //Name = name;
        Width = width;
        Height = height;
        //X = x;
        //Y = y;

        Type = type;

        Structure = structure;

        StartPosition = -Vector2Int.one;
        FinishPosition = -Vector2Int.one;

        Portals = new List<Portal>();

        //Info.AreaType = areaType;
        //Info.DecorationType = MazeDecorationType.Empty;

        //if (Info.AreaType == MazeAreaType.Main) ZIndex = 0;
        //else ZIndex = -1;

        //BoundaryMazes = new List<Maze>();
    }


    //public string GetMazeStructureTypeAsText() { 
    //    switch (Info.AreaType)
    //    {
    //        case MazeAreaType.Field:
    //            return "Field";
    //        case MazeAreaType.Room:
    //            return "Room";
    //        case MazeAreaType.Corridor:
    //            return "Corridor";
    //        default:
    //            return "Main";
    //    }
    //}

    public void SetStartPosition(Vector2Int start) {
        StartPosition = start;
        Cells[start.x][start.y].Type = MazeCellType.Start;
    }

    public void SetFinishPosition(Vector2Int finish)
    {
        FinishPosition = finish;
        Cells[finish.x][finish.y].Type = MazeCellType.Finish;
    }

    public void SetPortals(List<Portal> portals)
    {
        foreach (Portal portal in portals)
        {
            Cells[portal.Position.x][portal.Position.y].Type = MazeCellType.Portal;
        }
        Portals = portals;
    }

    //public void SetPortalsIn(List<PortalIn> portals)
    //{
    //    PortalsIn = portals;
    //}

    //public void SetPortalsOut(List<PortalOut> portals)
    //{
    //    foreach (PortalOut portal in portals)
    //    {
    //        Cells[portal.Position.x][portal.Position.y].Type = CellType.Portal;
    //        Cells[portal.Position.x][portal.Position.y].DestinationMazeName = portal.ToMazeName;
    //    }
    //    PortalsOut = portals;
    //}

};



