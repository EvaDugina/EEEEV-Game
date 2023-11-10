
//public enum MazeDimension
//{
//    One, Two
//}
public enum MazeForm
{
    Default, Triangle
}

public enum MazeTessellation
{
    Orthogonal, Fractal, Crack
}
public enum MazeRouting
{
    None, Ideal, Braid, ParticallyBraid, Sparse, HighSparse
}
public enum MazeTexture
{
    Bias, Longitudinall, Elite, Symmetry, River
}

public class MazeStructure
{
    //public MazeDimension Dimension;
    public MazeForm Form;
    public MazeTessellation Tessellation;
    public MazeRouting Routing;
    public MazeTexture Texture;

}

public class MazeStructureHandler {

    public static MazeStructure GetMazeStructureByAreaType(AreaType type) {
        if (type == AreaType.Main)
            return GetMainMazeStructure();
        else if (type == AreaType.Room)
            return GetRoomMazeStructure();
        else if (type == AreaType.Field)
            return GetFieldMazeStructure();
        else
            return GetCorridorMazeStructure();
    }

    public static MazeStructure GetMainMazeStructure()
    {
        return new MazeStructure()
        {
            //Dimension = MazeDimension.Two,
            Form = MazeForm.Default,
            Tessellation = MazeTessellation.Orthogonal,
            Routing = MazeRouting.ParticallyBraid,
            Texture = MazeTexture.Longitudinall
        };
    }

    public static MazeStructure GetRoomMazeStructure()
    {
        return new MazeStructure()
        {
            //Dimension = MazeDimension.One,
            Form = MazeForm.Triangle,
            Tessellation = MazeTessellation.Orthogonal,
            Routing = MazeRouting.HighSparse,
            Texture = MazeTexture.Longitudinall
        };
    }

    public static MazeStructure GetFieldMazeStructure()
    {
        return new MazeStructure()
        {
            //Dimension = MazeDimension.One,
            Form = MazeForm.Default,
            Tessellation = MazeTessellation.Orthogonal,
            Routing = MazeRouting.None,
            Texture = MazeTexture.Longitudinall
        };
    }

    public static MazeStructure GetCorridorMazeStructure()
    {
        return new MazeStructure()
        {
            //Dimension = MazeDimension.One,
            Form = MazeForm.Default,
            Tessellation = MazeTessellation.Orthogonal,
            Routing = MazeRouting.None,
            Texture = MazeTexture.Longitudinall
        };
    }

}
