﻿
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public struct CellInfo
{
    public bool Visited;
}

public class MazeGenerator
{

    private int Width;
    private int Height;

    private MazeStructure Structure;

    private Vector2Int StartPosition;
    private Vector2Int FinishPosition;

    public MazeCell[][] Cells;
    public CellInfo[][] CellsInfo;


    public MazeGenerator(int width, int height, MazeStructure structure)
    {

        Width = width;
        Height = height;

        //Maze = new Maze(width, height);
        Structure = structure;

        CellsInfo = new CellInfo[width][];
        for (int x = 0; x < width; x++)
        {
            CellsInfo[x] = new CellInfo[height];
            for (int y = 0; y < height; y++)
            {
                CellsInfo[x][y].Visited = false;
            }
        }

    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */


    /// <summary>
    // Генерируем ГЛАВНЫЙ лабиринт
    /// </summary>

    public Maze GenerateMainMaze(StaticPositionParameter start, DynamicPositionParameter finish)
    {
        MazeSide mazeSide = MazeSide.Center;

        int width = Width;
        int height = Height;

        StartPosition = AreaStructureHandler.GetPositionByStaticParameter(start, Width, Height);

        GenerateStructure();

        //RemoveBoundaryCells();
        FinishPosition = GetMazeFinish(finish);

        // Показываем / отключаем колонны
        SetColumnTypes();

        Maze maze = new Maze(width, height, mazeSide);
        maze.Cells = Cells;

        if (maze.Type != MazeType.Boundary)
        {
            maze.SetStartPosition(StartPosition);
            maze.SetFinishPosition(FinishPosition);
        }

        //Назначаем клеткам дистанции до старта
        //SetDistances(maze);

        return maze;
    }

    public List<Maze> GenerateBoundaryMazes()
    {
        List<Maze> boundaryMazes = new List<Maze>
        {
            GenerateBoundaryMaze(MazeSide.Left),
            GenerateBoundaryMaze(MazeSide.Right),
            GenerateBoundaryMaze(MazeSide.Top),
            GenerateBoundaryMaze(MazeSide.Bottom),

            GenerateBoundaryMaze(MazeSide.TopLeft),
            GenerateBoundaryMaze(MazeSide.TopRight),
            GenerateBoundaryMaze(MazeSide.BottomRight),
            GenerateBoundaryMaze(MazeSide.BottomLeft)
        };

        return boundaryMazes;
    }

    private Maze GenerateBoundaryMaze(MazeSide side)
    {
        // Убираем точки старта и финиша с основного Maze, чтобы сгенерировать boundary Maze без них
        Cells[StartPosition.x][StartPosition.y].Type = MazeCellType.Default;
        Cells[FinishPosition.x][FinishPosition.y].Type = MazeCellType.Default;

        MazeCell[][] cells = MazeGenerateUtilities.GetMazePartBySide(Cells, side);

        // Возвращаем точки старта и финиша на основной Maze
        Cells[StartPosition.x][StartPosition.y].Type = MazeCellType.Start;
        Cells[FinishPosition.x][FinishPosition.y].Type = MazeCellType.Finish;

        //Vector2 mazePosition = GetBoundaryMazePosition(side, cells.Length, cells[0].Length);
        Maze boundaryMaze = new(cells.Length, cells[0].Length, side)
        {
            Cells = cells
        };

        return boundaryMaze;
    }


    private void GenerateStructure()
    {
        GenerateForm();
        GenerateRouting();
        GenerateTexture();
    }

    private void GenerateForm()
    {
        if (Structure.Form == MazeForm.Triangle)
            Cells = MazeGenerateUtilities.DefineTriangleMaze(Width, Height);
        else
            Cells = MazeGenerateUtilities.DefineDefaultMaze(Width, Height);
    }

    private void GenerateRouting()
    {
        GenerateRoutingParticallyBraid();

        //if (Structure.Routing == MazeRouting.ParticallyBraid)
        //{
        //    GenerateRoutingParticallyBraid();
        //}
        //else if (Structure.Routing == MazeRouting.HighSparse)
        //{
        //    GenerateRoutingHighSparse();
        //}
        //else
        //{
        //    GenerateRoutingNone();
        //}

    }

    private void GenerateTexture()
    {
        if (Structure.Texture == MazeTexture.Longitudinall)
        {

        }
    }





    //private void RemoveBoundaryCells()
    //{
    //    int newWidth = Width - 1;
    //    int newHeight = Height - 1;
    //    MazeCell[][] newMaze = new MazeCell[newWidth][];
    //    Array.Copy(Cells, newMaze, newWidth);
    //    for (int x = 0; x < newWidth; x++)
    //    {
    //        newMaze[x] = new MazeCell[newHeight];
    //        Array.Copy(Cells[x], newMaze[x], newHeight);
    //    }
    //    Cells = newMaze;
    //    Width -= 1;
    //    Height -= 1;
    //}

    /// <summary>
    // Генерируем лабиринт, удаляя стены
    /// </summary>

    public void GenerateRoutingNone()
    {

        // Удаляем все стены
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cells[x][y].RemoveAllWalls();
            }
        }
    }


    public void GenerateRoutingParticallyBraid()
    {

        MazeCell current = Cells[StartPosition.x][StartPosition.y];
        CellsInfo[StartPosition.x][StartPosition.y].Visited = true;
        current.DistanceFromStart = 0;

        Stack<MazeCell> stackVisited = new Stack<MazeCell>();

        do
        {
            int x = current.X;
            int y = current.Y;

            List<MazeCell> unvisitedNeighbours = new List<MazeCell>();

            if (x > 0 && !CellsInfo[x - 1][y].Visited)
                unvisitedNeighbours.Add(Cells[x - 1][y]);
            if (y > 0 && !CellsInfo[x][y - 1].Visited)
                unvisitedNeighbours.Add(Cells[x][y - 1]);
            if (x < Width - 1 && !CellsInfo[x + 1][y].Visited)
                unvisitedNeighbours.Add(Cells[x + 1][y]);
            if (y < Height - 1 && !CellsInfo[x][y + 1].Visited)
                unvisitedNeighbours.Add(Cells[x][y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeCell choosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveBetweenWalls(current, choosen);

                CellsInfo[choosen.X][choosen.Y].Visited = true;
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
                    randomNeighbor = Cells[x - 1][y];
                else if (x < Width - 2 && indexRandomNeighbor == 1)
                    randomNeighbor = Cells[x + 1][y];
                else if (y > 2 && indexRandomNeighbor == 2)
                    randomNeighbor = Cells[x][y - 1];
                else if (y < Height - 2 && indexRandomNeighbor == 3)
                    randomNeighbor = Cells[x][y + 1];

                if (randomNeighbor != null)
                {
                    RemoveBetweenWalls(current, randomNeighbor);
                }
            }

        } while (stackVisited.Count > 0);

        //// Убираем лишние крайние стены
        AddBoundaryWalls();

        // Добавляем проходы для расположения лабиринта на торе
        CreateBoundaryPasseges();

    }

    public void GenerateRoutingHighSparse()
    {

    }


    //private void GenerateFieldsInsideMaze(MazeCell[][] mazeCells)
    //{
    //    int fieldWidth = Width / 5;
    //    int fieldHeight = Height / 5;

    //    int startCellX = Random.Range(fieldWidth + 1, Width - fieldWidth - 2);
    //    int startCellY = Random.Range(fieldHeight + 1, Height - fieldHeight - 2);

    //    for (int x = startCellX; x < startCellX + fieldWidth; x++)
    //    {
    //        for (int y = startCellY; y < startCellY + fieldHeight; y++)
    //        {
    //            mazeCells[x][y].RemoveAllWalls();
    //        }
    //    }


    //    // Добавляем вертикальные границы лабиринту
    //    int randomEnter = Random.Range(0, fieldWidth - 1);
    //    for (int x = startCellX; x < startCellX + fieldWidth; x++)
    //    {
    //        if (x != startCellX + randomEnter)
    //        {
    //            mazeCells[x][startCellY].BottomWall = true;
    //            mazeCells[x][startCellY - 1].LeftWall = false;
    //        }
    //        if (x != startCellX + randomEnter)
    //        {
    //            mazeCells[x][startCellY + fieldHeight].BottomWall = true;
    //            mazeCells[x][startCellY + fieldHeight].LeftWall = false;
    //        }
    //    }

    //    // Добавляем горизонтальные границы лабиринту
    //    randomEnter = Random.Range(0, fieldHeight - 1);
    //    for (int y = startCellY; y < startCellY + fieldHeight; y++)
    //    {
    //        if (y != startCellY + randomEnter)
    //        {
    //            mazeCells[startCellX][y].LeftWall = true;
    //            mazeCells[startCellX - 1][y].BottomWall = false;
    //        }
    //        if (y != startCellY + randomEnter)
    //        {
    //            mazeCells[startCellX + fieldWidth][y].LeftWall = true;
    //            mazeCells[startCellX + fieldWidth][y].BottomWall = false;
    //        }
    //    }

    //}


    private void AddBoundaryWalls()
    {
        /// Добавляем стены снизу и сверху для граничных клеток
        int yMax = Height - 1;
        for (int x = 0; x < Width; x++)
        {
            Cells[x][0].WallsStatus.BottomWall = true;
            Cells[x][yMax].WallsStatus.TopWall = true;
        }

        /// Добавляем стены слева и справа для граничных клеток
        int xMax = Width - 1;
        for (int y = 0; y < Height; y++)
        {
            Cells[0][y].WallsStatus.LeftWall = true;
            Cells[xMax][y].WallsStatus.RightWall = true;

        }
    }

    //private void RemoveBoundaryWalls()
    //{
    //    /// Убираем лишние стены сверху
    //    int yMax =Height - 1;
    //    for (int x = 0; x <Width; x++)
    //    {
    //        //Cells[x][yMax].LeftWall = false;
    //        Cells[x][yMax].Floor = false;
    //    }

    //    /// Убираем лишние стены справа
    //    int xMax =Width - 1;
    //    for (int y = 0; y <Height; y++)
    //    {
    //        //Cells[xMax][y].BottomWall = false;
    //        Cells[xMax][y].Floor = false;
    //    }
    //}

    private void RemoveBetweenWalls(MazeCell a, MazeCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y)
            {
                a.WallsStatus.BottomWall = false;
                b.WallsStatus.TopWall = false;
            }
            else
            {
                b.WallsStatus.BottomWall = false;
                a.WallsStatus.TopWall = false;
            }
        }
        else if (a.Y == b.Y)
        {
            if (a.X > b.X)
            {
                a.WallsStatus.LeftWall = false;
                b.WallsStatus.RightWall = false;
            }
            else
            {
                b.WallsStatus.LeftWall = false;
                a.WallsStatus.RightWall = false;
            }
        }
    }


    public void CreateBoundaryPasseges()
    {
        int xMax = Width - 1;
        //bool flagSymmetric = false;
        //int startIndex = 0;
        for (int y = 0; y < Height; y++)
        {
            //Более крутое удаление стен, для которого необходима отдельная и осторожная генерация крайних клеток, чтобы гарантировать 
            //    наличие симметричной последовательности, с двух сторон ограниченной левыми стенами
            //if (Cells[0][y].BottomWall != Cells[xMax][y].BottomWall)
            //{
            //    if (flagSymmetric)
            //    {
            //        for (int i = startIndex; i < y-1; i++)
            //        {
            //            Cells[0][i].LeftWall = false;
            //            Cells[xMax + 1][i].LeftWall = false;
            //        }
            //    }
            //    flagSymmetric = false;
            //    startIndex = -1;
            //}

            //Debug.Log(Width + "(" + Cells.Length + ")" + " " + Height + "(" + Cells[0].Length + ")" + ": (" + xMax + ", " + y + ")");
            if (Cells[0][y].WallsStatus.BottomWall == Cells[xMax][y].WallsStatus.BottomWall
                && Cells[0][y].WallsStatus.TopWall == Cells[xMax][y].WallsStatus.TopWall
                )
            {
                Cells[0][y].WallsStatus.LeftWall = false;
                Cells[xMax][y].WallsStatus.RightWall = false;
            }
            else
            {
                Cells[0][y].WallsStatus.LeftWall = true;
                Cells[xMax][y].WallsStatus.RightWall = true;
            }
        }

        int yMax = Height - 1;
        //flagSymmetric = false;
        //startIndex = 0;
        for (int x = 0; x < Width; x++)
        {
            //Более крутое удаление стен, для которого необходима отдельная и осторожная генерация крайних клеток, чтобы гарантировать 
            //    наличие симметричной последовательности, с двух сторон ограниченной левыми стенами
            //if (Cells[x][0].LeftWall != Cells[x][yMax].LeftWall)
            //{
            //    if (flagSymmetric)
            //    {
            //        for (int i = startIndex; i < x-1; i++)
            //        {
            //            Cells[i][0].BottomWall = false;
            //            Cells[i][yMax + 1].BottomWall = false;
            //        }
            //    }
            //    flagSymmetric = false;
            //    startIndex = -1;
            //}
            if (Cells[x][0].WallsStatus.LeftWall == Cells[x][yMax].WallsStatus.LeftWall
                && Cells[x][0].WallsStatus.RightWall == Cells[x][yMax].WallsStatus.RightWall
                )
            {
                Cells[x][0].WallsStatus.BottomWall = false;
                Cells[x][yMax].WallsStatus.TopWall = false;
            }
            else
            {
                Cells[x][0].WallsStatus.BottomWall = true;
                Cells[x][yMax].WallsStatus.TopWall = true;
            }
        }
    }

    private Vector2Int GetMazeFinish(DynamicPositionParameter areaFinish)
    {

        switch (areaFinish)
        {
            case DynamicPositionParameter.ByDistance:
                return FindPointByDistance();
            default:
                return -Vector2Int.one;

        }
    }

    private Vector2Int FindPointByDistance()
    {
        MazeCell finish = Cells[0][0];

        for (int x = 0; x < Width; x++)
        {
            if (Cells[x][Height - 1].DistanceFromStart > finish.DistanceFromStart)
                finish = Cells[x][Height - 1];
            if (Cells[x][0].DistanceFromStart > finish.DistanceFromStart)
                finish = Cells[x][0];
        }

        for (int y = 0; y < Height; y++)
        {
            if (Cells[Width - 1][y].DistanceFromStart > finish.DistanceFromStart)
                finish = Cells[Width - 1][y];
            if (Cells[0][y].DistanceFromStart > finish.DistanceFromStart)
                finish = Cells[0][y];
        }

        if (finish.X == 0)
            finish.WallsStatus.LeftWall = false;
        else if (finish.Y == 0)
            finish.WallsStatus.BottomWall = false;
        else if (finish.X == Width - 2)
            Cells[finish.X + 1][finish.Y].WallsStatus.LeftWall = false;
        else if (finish.Y == Height - 2)
            Cells[finish.X][finish.Y + 1].WallsStatus.BottomWall = false;

        return new Vector2Int(finish.X, finish.Y);
    }

    private Vector2Int FindPointByForwardSide(MazeSide side)
    {
        switch (side)
        {
            case MazeSide.Top:
                return new Vector2Int(Width / 2, Height - 1);
            case MazeSide.Right:
                return new Vector2Int(Width - 1, Height / 2);
            case MazeSide.Bottom:
                return new Vector2Int(Width / 2, 0);
            case MazeSide.Left:
                return new Vector2Int(0, Height / 2);
            default:
                return Vector2Int.zero;
        }
    }

    private void SetColumnTypes()
    {

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (!Cells[x][y].WallsStatus.LeftWall && !Cells[x][y].WallsStatus.TopWall)
                    Cells[x][y].ColumnsStatus.TopLeft = false;

                if (!Cells[x][y].WallsStatus.LeftWall && !Cells[x][y].WallsStatus.BottomWall)
                    Cells[x][y].ColumnsStatus.BottomLeft = false;

                if (!Cells[x][y].WallsStatus.RightWall && !Cells[x][y].WallsStatus.TopWall)
                    Cells[x][y].ColumnsStatus.TopRight = false;

                if (!Cells[x][y].WallsStatus.RightWall && !Cells[x][y].WallsStatus.BottomWall)
                    Cells[x][y].ColumnsStatus.BottomRight = false;

            }
        }

    }


    //private void SetDistances(MazeCell2D[][] maze)
    //{

    //    MazeCell2D current = Cells[0, 0];
    //    current.Visited2 = true;
    //    current.DistanceFromStart = 0;

    //    List<MazeCell2D> listVisited = new List<MazeCell2D>();

    //    do
    //    {
    //        int x = current.X;
    //        int y = current.Y;

    //        List<MazeCell2D> unvisitedNeighbours = new List<MazeCell2D>();

    //        if (x > 0 && Cells[x, y].LeftWall && !Cells[x - 1, y].Visited2)
    //            unvisitedNeighbours.Add(Cells[x - 1, y]);
    //        if (x <Width - 2 && Cells[x + 1, y].LeftWall && !Cells[x + 1, y].Visited2)
    //            unvisitedNeighbours.Add(Cells[x + 1, y]);
    //        if (y > 0 && Cells[x, y].BottomWall && !Cells[x, y - 1].Visited2)
    //            unvisitedNeighbours.Add(Cells[x, y - 1]);
    //        if (y <Height - 2 && Cells[x, y + 1].BottomWall && !Cells[x, y + 1].Visited2)
    //            unvisitedNeighbours.Add(Cells[x, y + 1]);

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
    //            if (x > 0 && Cells[x, y].LeftWall)
    //                neighbours.Add(Cells[x - 1, y]);
    //            if (x <Width - 2 && Cells[x + 1, y].LeftWall)
    //                neighbours.Add(Cells[x + 1, y]);
    //            if (y > 0 && Cells[x, y].BottomWall)
    //                neighbours.Add(Cells[x, y - 1]);
    //            if (y <Height - 2 && Cells[x, y + 1].BottomWall)
    //                neighbours.Add(Cells[x, y + 1]);

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