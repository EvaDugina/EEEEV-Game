using JetBrains.Annotations;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;


public enum Type
{
    Crossroad,
    Solid,
    Nothing
}

public class MazeCell2D
{

    public int X;
    //public int Y = 0;
    public int Y;

    public float XReal;
    public float YReal;

    public bool ToptWall = false;
    public bool RightWall = false;
    public bool LeftWall = true;
    public bool BottomWall = true;
    public bool Floor = true;

    public bool Visited = false;

    public int DistanceFromStart;
    public Type BottomLeftColumnType = Type.Nothing;
    public Type TopRightColumnType = Type.Nothing;

}

public class Maze2D
{
    public MazeCell2D[][] Cells;

    public int Width = 100;
    public int Height = 100;

    public float CellWidth;
    public float CellHeight;

    public Vector2Int FinishPosition;

    public float KoefCuttingSiblingMazes = 2;


    public MazeCell2D[][] GetMazePartBySide(MazeCell2D[][] cells, string side)
    {
        if (side == "Left")
        {
            int width = cells.Length / 2;
            MazeCell2D[][] slicedMaze = new MazeCell2D[width][];
            Array.Copy(cells, width, slicedMaze, 0, width);
            return slicedMaze;
        }
        else if (side == "Right")
        {
            int width = cells.Length / 2;
            MazeCell2D[][] slicedMaze = new MazeCell2D[width][];
            Array.Copy(cells, 0, slicedMaze, 0, width);
            return slicedMaze;
        }
        else if (side == "Top")
        {
            int width = cells.Length;
            MazeCell2D[][] slicedMaze = new MazeCell2D[width][];
            for (int x = 0; x < width; x++)
            {
                int height = cells[x].Length / 2;
                slicedMaze[x] = new MazeCell2D[height];
                Array.Copy(cells[x], 0, slicedMaze[x], 0, height);
            }
            return slicedMaze;
        }
        else if (side == "Bottom")
        {
            int width = cells.Length;
            MazeCell2D[][] slicedMaze = new MazeCell2D[width][];
            for (int x = 0; x < Width; x++)
            {
                int height = cells[x].Length / 2;
                slicedMaze[x] = new MazeCell2D[height];
                Array.Copy(cells[x], height, slicedMaze[x], 0, height);
            }
            return slicedMaze;
        }

        else
        {
            return GetMazeTrianglesBySide(cells, side);
        }
    }

    public MazeCell2D[][] GetMazeTrianglesBySide(MazeCell2D[][] cells, string side)
    {
        if (side == "TopLeft")
        {
            MazeCell2D[][] slicedMaze = GetMazePartBySide(cells, "Top");
            slicedMaze = GetMazePartBySide(slicedMaze, "Left");
            return slicedMaze;
        }
        else if (side == "TopRight")
        {
            MazeCell2D[][] slicedMaze = GetMazePartBySide(cells, "Top");
            slicedMaze = GetMazePartBySide(slicedMaze, "Right");
            return slicedMaze;
        }
        else if (side == "BottomLeft")
        {
            MazeCell2D[][] slicedMaze = GetMazePartBySide(cells, "Bottom");
            slicedMaze = GetMazePartBySide(slicedMaze, "Left");
            return slicedMaze;
        }
        else {
            MazeCell2D[][] slicedMaze = GetMazePartBySide(cells, "Bottom");
            slicedMaze = GetMazePartBySide(slicedMaze, "Right");
            return slicedMaze;
        }

    }



}
