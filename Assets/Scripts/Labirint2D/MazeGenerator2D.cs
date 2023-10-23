
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator2D
{
    private MazeStructureType Type;

    private int Width;
    private int Height;

    private Vector2Int CellSize;

    public MazeGenerator2D(MazeStructureType type, int width, int height)
    {
        Type = type;
        Width = width;
        Height = height;
        CellSize = new Vector2Int(1, 1);
    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */


    /// <summary>
    // Генерируем лабиринт
    /// </summary>
    public Maze Generate()
    {
        MazeCell[][] cells = new MazeCell[Width][];

        for (int x = 0; x < Width; x++)
        {
            cells[x] = new MazeCell[Height];
            for (int y = 0; y < Height; y++)
            {

                cells[x][y] = new MazeCell { X = x, Y = y/*, XReal = x * CellSize.x, YReal = y * CellSize.y*/ };
            }
        }

        RemoveWalls(cells);

        // Показываем / отключаем колонны
        SetColumnTypes(cells);

        Maze maze = new(Width, Height, Width / 2, Height / 2, Type)
        {
            Cells = cells,
            CellSize = CellSize,
            FinishPosition = PlaceMazeExit(cells)
        };

        //Назначаем клеткам дистанции до старта
        //SetDistances(maze);


        return maze;
    }


    /// <summary>
    // Генерируем лабиринт, удаляя стены
    /// </summary>
    public void RemoveWalls(MazeCell[][] maze)
    {

        MazeCell current = maze[0][0];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<MazeCell> stackVisited = new Stack<MazeCell>();

        do
        {
            int x = current.X;
            int y = current.Y;

            List<MazeCell> unvisitedNeighbours = new List<MazeCell>();

            if (x > 0 && !maze[x - 1][y].Visited)
                unvisitedNeighbours.Add(maze[x - 1][y]);
            if (x < Width - 1 && !maze[x + 1][y].Visited)
                unvisitedNeighbours.Add(maze[x + 1][y]);
            if (y > 0 && !maze[x][y - 1].Visited)
                unvisitedNeighbours.Add(maze[x][y - 1]);
            if (y < Height - 1 && !maze[x][y + 1].Visited)
                unvisitedNeighbours.Add(maze[x][y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeCell choosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
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
                MazeCell randomNeighbor = null;

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

        GenerateFields(maze);
        //GenerateRooms(maze);

        RemoveBoundaryWalls(maze);

        // Добавляем проходы для расположения лабиринта на торе
        CreateBoundaryPasseges(maze);

    }

    private void GenerateFields(MazeCell[][] mazeCells)
    {
        int fieldWidth = Width / 5;
        int fieldHeight = Height / 5;

        int startCellX = Random.Range(fieldWidth + 1, Width - fieldWidth - 2);
        int startCellY = Random.Range(fieldHeight + 1, Height - fieldHeight - 2);

        for (int x = startCellX; x < startCellX + fieldWidth; x++)
        {
            for (int y = startCellY; y < startCellY + fieldHeight; y++)
            {
                mazeCells[x][y].RemoveAllWalls();
                mazeCells[x][y].Type = MazeStructureType.Field;
            }
        }


        // Добавляем вертикальные границы лабиринту
        int randomEnter = Random.Range(0, fieldWidth - 1);
        for (int x = startCellX; x < startCellX + fieldWidth; x++)
        {
            if (x != startCellX + randomEnter)
            {
                mazeCells[x][startCellY].BottomWall = true;
                mazeCells[x][startCellY - 1].LeftWall = false;
            }
            if (x != startCellX + randomEnter)
            {
                mazeCells[x][startCellY + fieldHeight].BottomWall = true;
                mazeCells[x][startCellY + fieldHeight].LeftWall = false;
            }
        }

        // Добавляем горизонтальные границы лабиринту
        randomEnter = Random.Range(0, fieldHeight - 1);
        for (int y = startCellY; y < startCellY + fieldHeight; y++)
        {
            if (y != startCellY + randomEnter)
            {
                mazeCells[startCellX][y].LeftWall = true;
                mazeCells[startCellX - 1][y].BottomWall = false;
            }
            if (y != startCellY + randomEnter)
            {
                mazeCells[startCellX + fieldWidth][y].LeftWall = true;
                mazeCells[startCellX + fieldWidth][y].BottomWall = false;
            }
        }

    }

    private void RemoveBoundaryWalls(MazeCell[][] maze)
    {
        /// Убираем лишние стены сверху
        int yMax = Height - 1;
        for (int x = 0; x < Width; x++)
        {
            //maze[x][yMax].LeftWall = false;
            maze[x][yMax].Floor = false;
        }

        /// Убираем лишние стены справа
        int xMax = Width - 1;
        for (int y = 0; y < Height; y++)
        {
            //maze[xMax][y].BottomWall = false;
            maze[xMax][y].Floor = false;
        }
    }

    private void RemoveWall(MazeCell a, MazeCell b)
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


    public void CreateBoundaryPasseges(MazeCell[][] cells)
    {
        int xMax = Width - 2;
        //bool flagSymmetric = false;
        //int startIndex = 0;
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
        //flagSymmetric = false;
        //startIndex = 0;
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


    private Vector2Int PlaceMazeExit(MazeCell[][] maze)
    {
        MazeCell finish = maze[0][0];

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

    private void SetColumnTypes(MazeCell[][] cells)
    {

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if ((cells[x][y].LeftWall && cells[x][y].BottomWall) ||
                    (x > 0 && cells[x - 1][y].BottomWall && cells[x][y].LeftWall) ||
                    (y > 0 && cells[x][y - 1].LeftWall && cells[x][y].BottomWall) ||
                    (y > 0 && x > 0 && cells[x - 1][y].BottomWall && cells[x][y - 1].LeftWall))
                    cells[x][y].BottomLeftColumnType = ColumnType.Crossroad;
                else if ((cells[x][y].LeftWall || cells[x][y].BottomWall) ||
                    (x > 0 && cells[x - 1][y].BottomWall) ||
                    (y > 0 && cells[x][y - 1].LeftWall))
                    cells[x][y].BottomLeftColumnType = ColumnType.Solid;
                else
                    cells[x][y].BottomLeftColumnType = ColumnType.Default;
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

    private MazeCell GetNeignbourWithMinDistance(MazeCell[] neighbours)
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

    private void ChangeDistance(MazeCell startCell, List<MazeCell> visited)
    {
        if (visited.Count < 1)
        {
            return;
        }

        List<MazeCell> arrayVisited = visited;

        int index = 0;
        MazeCell current = arrayVisited[index];
        MazeCell previous = startCell;
        while (current != startCell && index < arrayVisited.Count - 1)
        {
            if (previous.DistanceFromStart > 0)
                current.DistanceFromStart = Mathf.Min(current.DistanceFromStart, previous.DistanceFromStart + 1);

            previous = current;
            current = arrayVisited[index++];

        }

    }

}
