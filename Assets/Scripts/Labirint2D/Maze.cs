using UnityEngine;

public enum MazeType { 
    Main,
    Boundary
}

public enum MazeSide { 
    Center, Left, Top, Right, Bottom,
    TopLeft, TopRight, BottomRight, BottomLeft
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


public class Maze
{
    public string Name;

    public int Width;
    public int Height;

    public MazeSide Side { get; set; }
    public MazeType Type { get; set; }

    public Vector2Int StartPosition { get; set; }
    public Vector2Int FinishPosition { get; set; }

    public MazeCell[][] Cells;


    //public float X;
    //public float Y;
    //public int ZIndex;

    //public Vector2Int CellSize = Vector2Int.one;

    //public MazeInfo Info;


    //public List<Maze> BoundaryMazes;


    public Maze(string name, int width, int height, MazeSide side) {

        Name = name;

        Width = width;
        Height = height;
        //X = x;
        //Y = y;

        SetSide(side);

        StartPosition = -Vector2Int.one;
        FinishPosition = -Vector2Int.one;

        //Info.AreaType = areaType;
        //Info.DecorationType = MazeDecorationType.Empty;

        //if (Info.AreaType == MazeAreaType.Main) ZIndex = 0;
        //else ZIndex = -1;

        //BoundaryMazes = new List<Maze>();
    }

    public void SetName(string name) {
        Name = name;
    }

    public void SetSide(MazeSide orientation) {
        Side = orientation;
        if (Side == MazeSide.Center) Type = MazeType.Main;
        else Type = MazeType.Boundary;
    }

    public void SetStartPosition(Vector2Int start)
    {
        StartPosition = start;
        Cells[start.x][start.y].Type = MazeCellType.Start;
    }

    public void SetFinishPosition(Vector2Int finish)
    {
        FinishPosition = finish;
        Cells[finish.x][finish.y].Type = MazeCellType.Finish;
    }

    private Vector2 GetBoundaryMazePosition(MazeSide side)
    {
        switch (side)
        {

            case MazeSide.Left:
                return new Vector2(-Width, 0);
            case MazeSide.Right:
                return new Vector2(Width, 0);
            case MazeSide.Top:
                return new Vector2(0, Height);
            case MazeSide.Bottom:
                return new Vector2(0, -Height);

            case MazeSide.TopLeft:
                return GetBoundaryMazePosition(MazeSide.Top) + GetBoundaryMazePosition(MazeSide.Left);
            case MazeSide.TopRight:
                return GetBoundaryMazePosition(MazeSide.Top) + GetBoundaryMazePosition(MazeSide.Right);
            case MazeSide.BottomLeft:
                return GetBoundaryMazePosition(MazeSide.Bottom) + GetBoundaryMazePosition(MazeSide.Left);
            case MazeSide.BottomRight:
                return GetBoundaryMazePosition(MazeSide.Bottom) + GetBoundaryMazePosition(MazeSide.Right);

            default:
                return Vector2.zero;
        }
    }


    public static string GetMazeSideAsText(MazeSide side)
    {
        switch (side)
        {
            case MazeSide.Top:
                return "Top";
            case MazeSide.Left:
                return "Left";
            case MazeSide.Right:
                return "Right";
            case MazeSide.Bottom:
                return "Bottom";

            case MazeSide.TopLeft:
                return "TopLeft";
            case MazeSide.TopRight:
                return "TopRight";
            case MazeSide.BottomRight:
                return "BottomRight";
            case MazeSide.BottomLeft:
                return "BottomLeft";

            default:
                return "Center";
        }
    }

    public void AddPortal(Vector2Int position) {
        Cells[position.x][position.y].Type = MazeCellType.Portal;
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



