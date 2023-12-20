using System;
using UnityEngine;

public class MazeGenerateUtilities
{

    public static MazeCell[][] DefineTriangleMaze(int width, int height)
    {
        // Формально объявляем и заполняем массив
        MazeCell[][] cells = new MazeCell[width][];
        for (int x = 0; x < width; x++)
        {
            cells[x] = new MazeCell[height];
            for (int y = 0; y < height; y++)
                cells[x][y] = new MazeCell(x, y, MazeCellType.Default);
        }

        int countX = (width / 2 - 1);
        int countY = (height / 2 - 1);
        int k = countX / countY;

        // Треугольник, указывающий влево
        int currentHeight = height / 2;
        for (int x = 0; x < width / 2; x++)
        {

            for (int y = 0; y < currentHeight; y++)
            {
                cells[x][y].SetStatus(MazeCellStatus.Disable);
                cells[x][height - y - 1].SetStatus(MazeCellStatus.Disable);
            }

            // Ступенька
            if (x % k == 0)
                currentHeight--;
        }

        // Треугольник, указывающий вправо
        currentHeight = 0;
        for (int x = width / 2+1; x < width; x++)
        {

            if ((x - width / 2) % k == 0)
                currentHeight++;

            for (int y = 0; y < currentHeight; y++)
            {
                cells[x][y].SetStatus(MazeCellStatus.Disable);
                cells[x][height - y - 1].SetStatus(MazeCellStatus.Disable);
            }
        }

        return cells;
    }

    public static MazeCell[][] DefineDefaultMaze(int width, int height)
    {
        MazeCell[][] cells = new MazeCell[width][];
        for (int x = 0; x < width; x++)
        {
            cells[x] = new MazeCell[height];
            for (int y = 0; y < height; y++)
            {

                cells[x][y] = new MazeCell(x, y, MazeCellType.Default);
            }
        }

        return cells;
    }



    public static MazeCell[][] GetMazePartBySide(MazeCell[][] cells, MazeSide side)
    {
        int koefCuttingSiblingMazes = 2;

        MazeCell[][] clone = GenerateCloneMazeCells(cells);

        int width;
        MazeCell[][] sideMaze;
        switch (side)
        {
            case MazeSide.Left:
                width = clone.Length / koefCuttingSiblingMazes;
                sideMaze = new MazeCell[width][];
                Array.Copy(clone, width + clone.Length % 2, sideMaze, 0, width);
                break;
            case MazeSide.Right:
                width = clone.Length / koefCuttingSiblingMazes;
                sideMaze = new MazeCell[width][];
                Array.Copy(clone, 0, sideMaze, 0, width);
                break;
            case MazeSide.Top:
                width = clone.Length;
                sideMaze = new MazeCell[width][];
                for (int x = 0; x < width; x++)
                {
                    int height = clone[x].Length / koefCuttingSiblingMazes;
                    sideMaze[x] = new MazeCell[height];
                    Array.Copy(clone[x], 0, sideMaze[x], 0, height);
                }
                break;
            case MazeSide.Bottom:
                width = clone.Length;
                sideMaze = new MazeCell[width][];
                for (int x = 0; x < width; x++)
                {
                    int height = clone[x].Length / koefCuttingSiblingMazes;
                    sideMaze[x] = new MazeCell[height];
                    Array.Copy(clone[x], height + clone[x].Length % 2, sideMaze[x], 0, height);
                }
                break;
            default:
                sideMaze = GetMazeTrianglesBySide(clone, side);
                break;
        }

        return sideMaze;
    }

    public static Maze GenerateCloneMaze(Maze prototypeMaze) {

        Maze maze = new Maze(prototypeMaze.Id * 100, prototypeMaze.Width, prototypeMaze.Height, prototypeMaze.Side);
        maze.SetCells(GenerateCloneMazeCells(prototypeMaze.Cells));
        return maze;
    }

    public static MazeCell[][] GenerateCloneMazeCells(MazeCell[][] cells) {
        int cloneWidth = cells.Length;
        int cloneHeight = cells[0].Length;
        MazeCell [][] cloneCells = new MazeCell[cloneWidth][];
        for (int x = 0; x < cloneWidth; x++)
        {
            cloneCells[x] = new MazeCell[cloneHeight];
            for (int y = 0; y < cloneHeight; y++)
            {
                cloneCells[x][y] = (MazeCell)cells[x][y].Clone();
            }
        }
        return cloneCells;
    }

    public static MazeCell[][] GetMazeTrianglesBySide(MazeCell[][] cells, MazeSide side)
    {
        MazeCell[][] slicedMaze = null;
        switch (side)
        {
            case MazeSide.TopLeft:
                slicedMaze = GetMazePartBySide(cells, MazeSide.Top);
                slicedMaze = GetMazePartBySide(slicedMaze, MazeSide.Left);
                break;
            case MazeSide.TopRight:
                slicedMaze = GetMazePartBySide(cells, MazeSide.Top);
                slicedMaze = GetMazePartBySide(slicedMaze, MazeSide.Right);
                break;
            case MazeSide.BottomLeft:
                slicedMaze = GetMazePartBySide(cells, MazeSide.Bottom);
                slicedMaze = GetMazePartBySide(slicedMaze, MazeSide.Left);
                break;
            case MazeSide.BottomRight:
                slicedMaze = GetMazePartBySide(cells, MazeSide.Bottom);
                slicedMaze = GetMazePartBySide(slicedMaze, MazeSide.Right);
                break;
        }

        return slicedMaze;
    }


    public static Vector2 GetBoundaryMazePositionInsideArea(int mainMazeWidth, int mainMazeHeight, MazeSide side)
    {
        switch (side)
        {

            case MazeSide.Left:
                return new Vector2(-mainMazeWidth/2, 0);
            case MazeSide.Right:
                return new Vector2(mainMazeWidth, 0);
            case MazeSide.Top:
                return new Vector2(0, mainMazeHeight);
            case MazeSide.Bottom:
                return new Vector2(0, -mainMazeHeight/2);

            case MazeSide.TopLeft:
                return GetBoundaryMazePositionInsideArea(mainMazeWidth, mainMazeHeight, MazeSide.Top) + GetBoundaryMazePositionInsideArea(mainMazeWidth, mainMazeHeight, MazeSide.Left);
            case MazeSide.TopRight:
                return GetBoundaryMazePositionInsideArea(mainMazeWidth, mainMazeHeight, MazeSide.Top) + GetBoundaryMazePositionInsideArea(mainMazeWidth, mainMazeHeight, MazeSide.Right);
            case MazeSide.BottomLeft:
                return GetBoundaryMazePositionInsideArea(mainMazeWidth, mainMazeHeight, MazeSide.Bottom) + GetBoundaryMazePositionInsideArea(mainMazeWidth, mainMazeHeight, MazeSide.Left);
            case MazeSide.BottomRight:
                return GetBoundaryMazePositionInsideArea(mainMazeWidth, mainMazeHeight, MazeSide.Bottom) + GetBoundaryMazePositionInsideArea(mainMazeWidth, mainMazeHeight, MazeSide.Right);

            default:
                return Vector2.zero;
        }
    }
}
