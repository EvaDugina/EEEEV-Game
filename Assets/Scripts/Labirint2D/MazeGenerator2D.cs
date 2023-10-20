using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator2D
{

    private int Width = 100;
    private int Height = 100;

    public MazeGenerator2D(int width, int height)
    {
        Width = width;
        Height = height;
    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */

    /// <summary>
    // Генерируем лабиринт
    /// </summary>
    public Maze2D GenerateMaze(Vector2 cellSize)
    {

        MazeCell2D[][] cells = new MazeCell2D[Width][];

        for (int x = 0; x < Width; x++)
        {
            cells[x] = new MazeCell2D[Height];
            for (int y = 0; y < Height; y++)
            {

                cells[x][y] = new MazeCell2D { X = x, Y = y, XReal = x * cellSize.x, YReal = y * cellSize.y };
            }
        }

        RemoveWalls(cells);

        // Показываем / отключаем колонны
        SetColumnTypes(cells);


        Maze2D maze = new Maze2D
        {
            Cells = cells,

            Width = Width,
            Height = Height,
            CellWidth = cellSize.x,
            CellHeight = cellSize.y,

            FinishPosition = PlaceMaxeExit(cells)
        };

        //Назначаем клеткам дистанции до старта
        //SetDistances(maze);


        return maze;
    }


    /// <summary>
    // Генерируем лабиринт, удаляя стены
    /// </summary>
    public void RemoveWalls(MazeCell2D[][] maze)
    {

        MazeCell2D current = maze[0][0];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<MazeCell2D> stackVisited = new Stack<MazeCell2D>();

        do
        {
            int x = current.X;
            int y = current.Y;

            List<MazeCell2D> unvisitedNeighbours = new List<MazeCell2D>();

            if (x > 0 && !maze[x - 1][y].Visited)
                unvisitedNeighbours.Add(maze[x - 1][y]);
            if (x < Width - 2 && !maze[x + 1][y].Visited)
                unvisitedNeighbours.Add(maze[x + 1][y]);
            if (y > 0 && !maze[x][y - 1].Visited)
                unvisitedNeighbours.Add(maze[x][y - 1]);
            if (y < Height - 2 && !maze[x][y + 1].Visited)
                unvisitedNeighbours.Add(maze[x][y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeCell2D choosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, choosen);

                choosen.Visited = true;
                stackVisited.Push(choosen);

                choosen.DistanceFromStart = current.DistanceFromStart + 1;

                current = choosen;
            }
            else
            {
                current = stackVisited.Pop();
                x = current.X;
                y = current.Y;

                // Заплетаем лаибринт, кроме гранчных клеток, у них - фиксированная ширина = 1
                int indexRandomNeighbor = Random.Range(0, 4);
                MazeCell2D randomNeighbor = null;

                if (x > 2 && indexRandomNeighbor == 0)
                    randomNeighbor = maze[x - 1][y];
                else if (x < Width - 3 && indexRandomNeighbor == 1)
                    randomNeighbor = maze[x + 1][y];
                else if (y > 2 && indexRandomNeighbor == 2)
                    randomNeighbor = maze[x][y - 1];
                else if (y < Height - 3 && indexRandomNeighbor == 3)
                    randomNeighbor = maze[x][y + 1];

                if (randomNeighbor != null)
                {
                    RemoveWall(current, randomNeighbor);
                }
            }

        } while (stackVisited.Count > 0);


        /// Убираем лишние стены сверху
        int yMax = Height - 1;
        for (int x = 0; x < Width; x++)
        {
            maze[x][yMax].LeftWall = false;
            //maze[x, zMax].Floor = false;
        }

        /// Убираем лишние стены справа
        int xMax = Width - 1;
        for (int y = 0; y < Height; y++)
        {
            maze[xMax][y].BottomWall = false;
            //maze[xMax, z].Floor = false;
        }

        // Добавляем проходы для расположения лабиринта на торе
        CreateBoundaryPasseges(maze);

    }

    private void RemoveWall(MazeCell2D a, MazeCell2D b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y) a.BottomWall = false;
            else b.BottomWall = false;
        }
        else if (a.Y == b.Y)
        {
            if (a.X > b.X) a.LeftWall = false;
            else b.LeftWall = false;
        }
    }


    public void CreateBoundaryPasseges(MazeCell2D[][] cells)
    {
        int xMax = Width - 2;
        bool flagSymmetric = false;
        int startIndex = 0;
        for (int y = 0; y < Height - 1; y++)
        {
            //Более крутое удаление стен, для которого необходима отдельная и осторожная генерация крайних клеток, чтобы гарантировать 
            //    наличие симметричной последовательности, с двух сторон ограниченной левыми стенами
            //if (cells[0][y].BottomWall != cells[xMax][y].BottomWall)
            //{
            //    if (flagSymmetric)
            //    {
            //        for (int i = startIndex; i < y-1; i++)
            //        {
            //            cells[0][i].LeftWall = false;
            //            cells[xMax + 1][i].LeftWall = false;
            //        }
            //    }
            //    flagSymmetric = false;
            //    startIndex = -1;
            //}

            if (cells[0][y].LeftWall == cells[xMax][y].LeftWall && y % 2 == 1)
            {
                cells[0][y].LeftWall = false;
                cells[xMax + 1][y].LeftWall = false;
            }
        }

        int yMax = Height - 2;
        flagSymmetric = false;
        startIndex = 0;
        for (int x = 0; x < Width - 1; x++)
        {
            //Более крутое удаление стен, для которого необходима отдельная и осторожная генерация крайних клеток, чтобы гарантировать 
            //    наличие симметричной последовательности, с двух сторон ограниченной левыми стенами
            //if (cells[x][0].LeftWall != cells[x][yMax].LeftWall)
            //{
            //    if (flagSymmetric)
            //    {
            //        for (int i = startIndex; i < x-1; i++)
            //        {
            //            cells[i][0].BottomWall = false;
            //            cells[i][yMax + 1].BottomWall = false;
            //        }
            //    }
            //    flagSymmetric = false;
            //    startIndex = -1;
            //}
            if (cells[x][0].LeftWall == cells[x][yMax].LeftWall && x % 2 == 1)
            {
                cells[x][0].BottomWall = false;
                cells[x][yMax + 1].BottomWall = false;
            }
        }
    }


    private Vector2Int PlaceMaxeExit(MazeCell2D[][] maze)
    {
        MazeCell2D finish = maze[0][0];

        for (int x = 0; x < Width; x++)
        {
            if (maze[x][Height - 2].DistanceFromStart > finish.DistanceFromStart) finish = maze[x][Height - 2];
            if (maze[x][0].DistanceFromStart > finish.DistanceFromStart) finish = maze[x][0];
        }

        for (int y = 0; y < Height; y++)
        {
            if (maze[Width - 2][y].DistanceFromStart > finish.DistanceFromStart) finish = maze[Width - 2][y];
            if (maze[0][y].DistanceFromStart > finish.DistanceFromStart) finish = maze[0][y];
        }

        if (finish.X == 0) finish.LeftWall = false;
        else if (finish.Y == 0) finish.BottomWall = false;
        else if (finish.X == Width - 2) maze[finish.X + 1][finish.Y].LeftWall = false;
        else if (finish.Y == Height - 2) maze[finish.X][finish.Y + 1].BottomWall = false;

        return new Vector2Int(finish.X, finish.Y);
    }

    private void SetColumnTypes(MazeCell2D[][] cells)
    {

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if ((cells[x][y].LeftWall && cells[x][y].BottomWall) ||
                    (x > 0 && cells[x - 1][y].BottomWall && cells[x][y].LeftWall) ||
                    (y > 0 && cells[x][y - 1].LeftWall && cells[x][y].BottomWall) ||
                    (y > 0 && x > 0 && cells[x - 1][y].BottomWall && cells[x][y - 1].LeftWall))
                    cells[x][y].ColumnType = Type.Crossroad;
                else if ((cells[x][y].LeftWall || cells[x][y].BottomWall) ||
                    (x > 0 && cells[x - 1][y].BottomWall) ||
                    (y > 0 && cells[x][y - 1].LeftWall))
                    cells[x][y].ColumnType = Type.Solid;
                else
                    cells[x][y].ColumnType = Type.Nothing;
            }
        }

    }


    //private void SetDistances(MazeCell2D[][] maze)
    //{

    //    MazeCell2D current = maze[0, 0];
    //    current.Visited2 = true;
    //    current.DistanceFromStart = 0;

    //    List<MazeCell2D> listVisited = new List<MazeCell2D>();

    //    do
    //    {
    //        int x = current.X;
    //        int y = current.Y;

    //        List<MazeCell2D> unvisitedNeighbours = new List<MazeCell2D>();

    //        if (x > 0 && maze[x, y].LeftWall && !maze[x - 1, y].Visited2)
    //            unvisitedNeighbours.Add(maze[x - 1, y]);
    //        if (x < Width - 2 && maze[x + 1, y].LeftWall && !maze[x + 1, y].Visited2)
    //            unvisitedNeighbours.Add(maze[x + 1, y]);
    //        if (y > 0 && maze[x, y].BottomWall && !maze[x, y - 1].Visited2)
    //            unvisitedNeighbours.Add(maze[x, y - 1]);
    //        if (y < Height - 2 && maze[x, y + 1].BottomWall && !maze[x, y + 1].Visited2)
    //            unvisitedNeighbours.Add(maze[x, y + 1]);

    //        if (unvisitedNeighbours.Count > 0)
    //        {
    //            MazeCell2D choosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];

    //            choosen.Visited2 = true;
    //            listVisited.Add(choosen);

    //            choosen.DistanceFromStart = current.DistanceFromStart + 1;

    //            current = choosen;
    //        }
    //        else
    //        {

    //            // Находим всех соседей
    //            List<MazeCell2D> neighbours = new List<MazeCell2D>();
    //            if (x > 0 && maze[x, y].LeftWall)
    //                neighbours.Add(maze[x - 1, y]);
    //            if (x < Width - 2 && maze[x + 1, y].LeftWall)
    //                neighbours.Add(maze[x + 1, y]);
    //            if (y > 0 && maze[x, y].BottomWall)
    //                neighbours.Add(maze[x, y - 1]);
    //            if (y < Height - 2 && maze[x, y + 1].BottomWall)
    //                neighbours.Add(maze[x, y + 1]);

    //            // Находим соседа с минимальной дистанцией до начала
    //            MazeCell2D minDisctanceNeighbour = GetNeignbourWithMinDistance(neighbours.ToArray());

    //            // Изменяем дистанцию всех посещённых
    //            if (current.DistanceFromStart > minDisctanceNeighbour.DistanceFromStart+1)
    //                ChangeDistance(minDisctanceNeighbour, listVisited);

    //            listVisited.RemoveAt(listVisited.Count - 1);
    //            if (listVisited.Count > 0)
    //                current = listVisited[listVisited.Count - 1];


    //        }

    //    } while (listVisited.Count > 0);
    //}

    private MazeCell2D GetNeignbourWithMinDistance(MazeCell2D[] neighbours)
    {
        if (neighbours.Length == 0)
            return null;

        int minDistance = neighbours[0].DistanceFromStart, minIndex = 0;
        for (int i = 0; i < neighbours.Length; i++)
            if (minDistance > neighbours[i].DistanceFromStart)
            {
                minDistance = neighbours[i].DistanceFromStart;
                minIndex = i;
            }

        return neighbours[minIndex];
    }

    private void ChangeDistance(MazeCell2D startCell, List<MazeCell2D> visited)
    {
        if (visited.Count < 1)
        {
            return;
        }

        List<MazeCell2D> arrayVisited = visited;

        int index = 0;
        MazeCell2D current = arrayVisited[index];
        MazeCell2D previous = startCell;
        while (current != startCell && index < arrayVisited.Count - 1)
        {
            if (previous.DistanceFromStart > 0)
                current.DistanceFromStart = Mathf.Min(current.DistanceFromStart, previous.DistanceFromStart + 1);

            previous = current;
            current = arrayVisited[index++];

        }

    }

}
