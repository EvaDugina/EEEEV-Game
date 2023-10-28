using System;
using UnityEngine;

public class MazeUtilities
{
    public static MazeCell[][] DefineMaze(int width, int height)
    {
        MazeCell[][] cells = new MazeCell[width][];

        for (int x = 0; x < width; x++)
        {
            cells[x] = new MazeCell[height];
            for (int y = 0; y < height; y++)
            {

                cells[x][y] = new MazeCell(x, y);
            }
        }

        return cells;
    }
    public static MazeCell[][] GetMazePartBySide(MazeCell[][] cells, string side)
    {
        int koefCuttingSiblingMazes = 2;

        int cloneWidth = cells.Length;
        int cloneHeight = cells[0].Length;

        MazeCell[][] clone = new MazeCell[cloneWidth][];
        for (int x = 0; x < cloneWidth; x++)
        {
            clone[x] = new MazeCell[cloneHeight];
            for (int y = 0; y < cloneHeight; y++)
            {
                clone[x][y] = (MazeCell)cells[x][y].Clone();
            }
        }


        if (side == "Left")
        {
            int width = clone.Length / koefCuttingSiblingMazes;
            MazeCell[][] slicedMaze = new MazeCell[width][];
            Array.Copy(clone, width, slicedMaze, 0, width);
            return slicedMaze;
        }
        else if (side == "Right")
        {
            int width = clone.Length / koefCuttingSiblingMazes;
            MazeCell[][] slicedMaze = new MazeCell[width][];
            Array.Copy(clone, 0, slicedMaze, 0, width);
            return slicedMaze;
        }
        else if (side == "Top")
        {
            int width = clone.Length;
            MazeCell[][] slicedMaze = new MazeCell[width][];
            for (int x = 0; x < width; x++)
            {
                int height = clone[x].Length / koefCuttingSiblingMazes;
                slicedMaze[x] = new MazeCell[height];
                Array.Copy(clone[x], 0, slicedMaze[x], 0, height);
            }
            return slicedMaze;
        }
        else if (side == "Bottom")
        {
            int width = clone.Length;
            MazeCell[][] slicedMaze = new MazeCell[width][];
            for (int x = 0; x < width; x++)
            {
                int height = clone[x].Length / koefCuttingSiblingMazes;
                slicedMaze[x] = new MazeCell[height];
                Array.Copy(clone[x], height, slicedMaze[x], 0, height);
            }
            return slicedMaze;
        }

        else
        {
            return GetMazeTrianglesBySide(clone, side);
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
