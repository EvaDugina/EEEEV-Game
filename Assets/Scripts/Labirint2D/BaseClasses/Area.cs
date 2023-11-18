

using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public enum AreaPriority
{
    Main, Secondary
}

public enum AreaType
{
    Main, Field, Room, Corridor, ReflectedMain
}

//public enum AreaStatus { 
//    Default, Reflected
//}

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
    public int Id { get; private set; }
    public AreaType Type { get; private set; }
    public AreaPriority Priority { get; private set; }
    //public AreaStatus Status { get; private set; }

    public float X { get; private set; }
    public float Y { get; private set; }
    public int ZIndex { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public AreaTopology Topology { get; private set; }

    public Maze MainMaze { get; private set; }
    public List<Maze> BoundaryMazes { get; private set; }

    public List<Portal> Portals { get; private set; }

    public Area(int id, AreaType type, Vector2 position, int width, int height)
    {
        Id = id;
        Type = type;

        if (Type == AreaType.Main) Priority = AreaPriority.Main;
        else Priority = AreaPriority.Secondary;

        if (Type == AreaType.Main || Type == AreaType.Field || Type == AreaType.ReflectedMain) Topology = AreaTopology.Toroid;
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

    public MazeCell GetCell(Vector2Int position)
    {
        Maze maze = GetMazeByPosition(position);
        int x = Mathf.Abs(position.x);
        int y = Mathf.Abs(position.y);
        return maze.Cells[x % maze.Width][y % maze.Height];
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
            case AreaType.ReflectedMain:
                return "ReflectedMain";

            default:
                return "Main";
        }
    }

    public MazeCellType GetCellTypeByPosition(Vector2Int position)
    {
        return GetMazeByPosition(position).Cells[position.x][position.y].Type;
    }

    public Maze GetMazeByPosition(Vector2Int position)
    {
        if (position.x >= Width && position.y >= Height)
            return GetMazeBySide(MazeSide.TopRight);
        else if (position.x < 0 && position.y < 0)
            return GetMazeBySide(MazeSide.BottomLeft);
        else if (position.x >= Width && position.y < 0)
            return GetMazeBySide(MazeSide.BottomRight);
        else if (position.x < 0 && position.y >= Height)
            return GetMazeBySide(MazeSide.TopLeft);

        if (position.x >= Width)
            return GetMazeBySide(MazeSide.Right);
        else if (position.y >= Height)
            return GetMazeBySide(MazeSide.Top);
        else if (position.x < 0)
            return GetMazeBySide(MazeSide.Left);
        else if (position.y < 0)
            return GetMazeBySide(MazeSide.Bottom);

        return MainMaze;
    }

    public Maze GetMazeBySide(MazeSide side)
    {
        foreach (Maze maze in BoundaryMazes)
        {
            if (maze.Side == side) return maze;
        }

        return MainMaze;
    }

    public Portal GetPortalByPosition(Vector2Int position)
    {
        foreach (Portal portal in Portals)
        {
            if (portal.Position == position) return portal;
        }

        throw new ArgumentException("Портала по данной позиции клетки не существует");
    }

    public Portal GetPortalByToAreaId(int areaId)
    {
        foreach (Portal portal in Portals)
        {
            if (portal.ToAreaId == areaId) return portal;
        }

        throw new ArgumentException("Портала к данному Area не существует");
    }

    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   SETTERS
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */

    public void SetMainMaze(Maze maze)
    {
        MainMaze = maze;
    }

    public void SetBoundaryMazes(List<Maze> mazes)
    {
        BoundaryMazes = mazes;
    }

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

    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   SETTERS
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/


}
