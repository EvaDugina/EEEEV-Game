using System;

public class MazeUtilities
{
    public static MazeCell[][] GetMazePartBySide(MazeCell[][] cells, string side)
    {
        int koefCuttingSiblingMazes = 2;

        if (side == "Left")
        {
            int width = cells.Length / koefCuttingSiblingMazes;
            MazeCell[][] slicedMaze = new MazeCell[width][];
            Array.Copy(cells, width, slicedMaze, 0, width);
            return slicedMaze;
        }
        else if (side == "Right")
        {
            int width = cells.Length / koefCuttingSiblingMazes;
            MazeCell[][] slicedMaze = new MazeCell[width][];
            Array.Copy(cells, 0, slicedMaze, 0, width);
            return slicedMaze;
        }
        else if (side == "Top")
        {
            int width = cells.Length;
            MazeCell[][] slicedMaze = new MazeCell[width][];
            for (int x = 0; x < width; x++)
            {
                int height = cells[x].Length / koefCuttingSiblingMazes;
                slicedMaze[x] = new MazeCell[height];
                Array.Copy(cells[x], 0, slicedMaze[x], 0, height);
            }
            return slicedMaze;
        }
        else if (side == "Bottom")
        {
            int width = cells.Length;
            MazeCell[][] slicedMaze = new MazeCell[width][];
            for (int x = 0; x < width; x++)
            {
                int height = cells[x].Length / koefCuttingSiblingMazes;
                slicedMaze[x] = new MazeCell[height];
                Array.Copy(cells[x], height, slicedMaze[x], 0, height);
            }
            return slicedMaze;
        }

        else
        {
            return GetMazeTrianglesBySide(cells, side);
        }
    }

    public static MazeCell[][] GetMazeTrianglesBySide(MazeCell[][] cells, string side)
    {
        if (side == "TopLeft")
        {
            MazeCell[][] slicedMaze = GetMazePartBySide(cells, "Top");
            slicedMaze = GetMazePartBySide(slicedMaze, "Left");
            return slicedMaze;
        }
        else if (side == "TopRight")
        {
            MazeCell[][] slicedMaze = GetMazePartBySide(cells, "Top");
            slicedMaze = GetMazePartBySide(slicedMaze, "Right");
            return slicedMaze;
        }
        else if (side == "BottomLeft")
        {
            MazeCell[][] slicedMaze = GetMazePartBySide(cells, "Bottom");
            slicedMaze = GetMazePartBySide(slicedMaze, "Left");
            return slicedMaze;
        }
        else
        {
            MazeCell[][] slicedMaze = GetMazePartBySide(cells, "Bottom");
            slicedMaze = GetMazePartBySide(slicedMaze, "Right");
            return slicedMaze;
        }
    }
}
