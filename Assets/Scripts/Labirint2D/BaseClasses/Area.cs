

using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public enum AreaPriority
{
    Main, Secondary
}

public enum AreaType
{
    Main, Field, Room, Corridor
}

public enum AreaTopology
{
    Plain, Toroid
}

public struct Portal
{
    public int FromAreaId;
    public int ToAreaId;
    public Vector2Int Position;
};

public class Area
{
    public int Id;
    public AreaType Type;
    public AreaPriority Priority;

    public float X, Y;
    public int ZIndex;

    public int Width, Height;

    public AreaTopology Topology;

    public Maze MainMaze;
    public List<Maze> BoundaryMazes;

    public List<Portal> Portals;

    public Area(int id, AreaType type, Vector2 position, int width, int height)
    {
        Id = id;
        Type = type;

        if (Type == AreaType.Main) Priority = AreaPriority.Main;
        else Priority = AreaPriority.Secondary;

        if (Type == AreaType.Main || Type == AreaType.Field) Topology = AreaTopology.Toroid;
        else Topology = AreaTopology.Plain;

        X = position.x;
        Y = position.y;
        ZIndex = (int)Type;

        Width = width;
        Height = height;

        MainMaze = null;
        BoundaryMazes = new List<Maze>();

        Portals = new List<Portal>();

    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   GETTERS
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */


    public Maze GetMainMaze()
    {
        return MainMaze;
    }

    public List<Maze> GetBoundaryMazes()
    {
        return BoundaryMazes;
    }

    public List<Maze> GetAllMazes()
    {
        List<Maze> mazes = new List<Maze>
        {
            MainMaze
        };
        mazes.AddRange(BoundaryMazes);
        return mazes;
    }

    public string GetAreaTypeAsText()
    {
        switch (Type)
        {
            case AreaType.Room:
                return "Room";
            case AreaType.Field:
                return "Field";
            case AreaType.Corridor:
                return "Corridor";

            default:
                return "Main";
        }
    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   SETTERS
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */


    public void AddPortals(List<Portal> portals)
    {
        foreach (Portal portal in portals)
            MainMaze.AddPortal(portal.Position);
        Portals = portals;
    }

    public void AddPortal(Portal portal)
    {
        MainMaze.AddPortal(portal.Position);
        Portals.Add(portal);
    }

}
