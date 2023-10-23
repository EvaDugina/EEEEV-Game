

public enum ColumnType
{
    Default,
    Crossroad,
    Solid,
}

public class MazeCell
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
    public ColumnType BottomLeftColumnType = ColumnType.Default;
    public ColumnType TopRightColumnType = ColumnType.Default;

    public MazeStructureType Type = MazeStructureType.Main;


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
