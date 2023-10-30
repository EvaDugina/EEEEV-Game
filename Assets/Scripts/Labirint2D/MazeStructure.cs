
public enum MazeDimension
{
    One, Two
}
public enum MazeGeometry
{
    Square, Rectangle, Triangle
}
public enum MazeTopology
{
    Plain, Toroid
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
    Bias, Longitudinall, elite, Symmetry, River
}

public class MazeStructure
{
    public MazeDimension Dimension;
    public MazeGeometry Geometry;
    public MazeTopology Topology;
    public MazeTessellation Tessellation;
    public MazeRouting Routing;
    public MazeTexture Texture;
}

public class MazeStructureHandler {

    public static MazeStructure GetMainMazeStructure()
    {
        return new MazeStructure()
        {
            Dimension = MazeDimension.Two,
            Geometry = MazeGeometry.Square,
            Topology = MazeTopology.Toroid,
            Tessellation = MazeTessellation.Orthogonal,
            Routing = MazeRouting.ParticallyBraid,
            Texture = MazeTexture.Longitudinall
        };
    }

    public static MazeStructure GetRoomMazeStructure()
    {
        return new MazeStructure()
        {
            Dimension = MazeDimension.One,
            Geometry = MazeGeometry.Triangle,
            Topology = MazeTopology.Plain,
            Tessellation = MazeTessellation.Orthogonal,
            Routing = MazeRouting.HighSparse,
            Texture = MazeTexture.Longitudinall
        };
    }

    public static MazeStructure GetFieldMazeStructure()
    {
        return new MazeStructure()
        {
            Dimension = MazeDimension.One,
            Geometry = MazeGeometry.Square,
            Topology = MazeTopology.Toroid,
            Tessellation = MazeTessellation.Orthogonal,
            Routing = MazeRouting.None,
            Texture = MazeTexture.Longitudinall
        };
    }

    public static MazeStructure GetCorridorMazeStructure()
    {
        return new MazeStructure()
        {
            Dimension = MazeDimension.One,
            Geometry = MazeGeometry.Rectangle,
            Topology = MazeTopology.Plain,
            Tessellation = MazeTessellation.Orthogonal,
            Routing = MazeRouting.None,
            Texture = MazeTexture.Longitudinall
        };
    }

}
