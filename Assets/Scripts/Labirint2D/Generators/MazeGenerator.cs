using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public struct CellInfo
{
    public bool Visited;
}

public class MazeGenerator
{

    private int Width;
    private int Height;

    private Vector2Int StartPosition;
    private Vector2Int FinishPosition;

    private MazeCell[][] Cells;

    private MazeStructureGenerator MazeStructureGenerator;


    public MazeGenerator(int width, int height, MazeStructure structure)
    {
        Width = width;
        Height = height;

        MazeStructureGenerator = new MazeStructureGenerator(Width, Height, structure);

    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */


    /// <summary>
    // Генерируем ГЛАВНЫЙ лабиринт
    /// </summary>
    /// 

    public Maze GenerateMainMaze(StaticPositionParameter start, DynamicPositionParameter finish)
    {
        MazeSide mazeSide = MazeSide.Center;

        int width = Width;
        int height = Height;

        StartPosition = AreaStructureHandler.GetPositionByStaticParameter(start, Width, Height);

        Cells = MazeStructureGenerator.GenerateStructure(StartPosition);

        AddBoundaryWalls();

        //RemoveBoundaryCells();
        FinishPosition = GetMazeFinish(finish);

        // Показываем / отключаем колонны
        SetColumnTypes();

        Maze maze = new Maze(width, height, mazeSide);
        maze.SetCells(Cells);

        if (maze.Type != MazeType.Boundary)
        {
            maze.SetStartPosition(StartPosition);
            if (FinishPosition != -Vector2Int.one)
                maze.SetFinishPosition(FinishPosition);
        }

        //Назначаем клеткам дистанции до старта
        //SetDistances(maze);

        return maze;
    }


    public List<Maze> GenerateBoundaryMazes(MazeRouting routing)
    {
        // Добавляем проходы для расположения лабиринта на торе
        if (routing == MazeRouting.None)
            RemoveBoundaryWalls();
        else
            CreateBoundaryPasseges();

        SetColumnTypes();

        List<Maze> boundaryMazes = new List<Maze>
        {
            GenerateBoundaryMaze(MazeSide.Left, Cells),
            GenerateBoundaryMaze(MazeSide.Right, Cells),
            GenerateBoundaryMaze(MazeSide.Top, Cells),
            GenerateBoundaryMaze(MazeSide.Bottom, Cells),

            GenerateBoundaryMaze(MazeSide.TopLeft, Cells),
            GenerateBoundaryMaze(MazeSide.TopRight, Cells),
            GenerateBoundaryMaze(MazeSide.BottomRight, Cells),
            GenerateBoundaryMaze(MazeSide.BottomLeft, Cells)
        };

        return boundaryMazes;
    }


    public static Maze GenerateReflectedMainMaze(Maze maze)
    {

        Maze bottomPartMaze = GenerateBoundaryMaze(MazeSide.Top, maze.Cells);
        Maze topPartMaze = GenerateBoundaryMaze(MazeSide.Bottom, maze.Cells);

        Maze reflectedMainMaze = new Maze(maze.Width, maze.Height, MazeSide.Center);
        MazeCell[][] reflectedMazeCells = new MazeCell[maze.Width][];

        int yMiddle = maze.Height / 2;

        for (int x = 0; x < maze.Width; x++)
        {

            reflectedMazeCells[x] = new MazeCell[maze.Height];
            int index = 0;

            for (int y = 0; y < topPartMaze.Height - 1; y++)
            {
                reflectedMazeCells[x][index] = (MazeCell)topPartMaze.Cells[x][y].Clone();
                reflectedMazeCells[x][index].SetXY(x, index);
                index += 1;
            }

            // Убираем стены у последней строки нижнего maze
            reflectedMazeCells[x][index] = (MazeCell)topPartMaze.Cells[x][topPartMaze.Height - 1].Clone();
            reflectedMazeCells[x][index].SetXY(x, index);
            if (maze.Cells[x][yMiddle].WallsStatus.BottomWall == false)
            {
                reflectedMazeCells[x][index].DisableTopWall();
            }
            index += 1;

            // Добавляем осевые клетки, которых нет на Boundary Mazes (берём их из основного Maze)
            reflectedMazeCells[x][index] = (MazeCell)maze.Cells[x][index].Clone();
            reflectedMazeCells[x][index].SetXY(x, index);
            if (bottomPartMaze.Cells[x][0].WallsStatus.BottomWall == topPartMaze.Cells[x][topPartMaze.Height - 1].WallsStatus.TopWall == false)
            {
                reflectedMazeCells[x][index].DisableTopWall();
                reflectedMazeCells[x][index].DisableBottomWall();
            }
            index += 1;

            // Убираем стены у первой строки верхнего maze
            reflectedMazeCells[x][index] = (MazeCell)bottomPartMaze.Cells[x][0].Clone();
            reflectedMazeCells[x][index].SetXY(x, index);
            if (maze.Cells[x][yMiddle].WallsStatus.TopWall == false)
            {
                reflectedMazeCells[x][index].DisableBottomWall();
            }
            index += 1;

            for (int y = 1; y < bottomPartMaze.Height; y++)
            {
                reflectedMazeCells[x][index] = (MazeCell)bottomPartMaze.Cells[x][y].Clone();
                reflectedMazeCells[x][index].SetXY(x, index);
                index += 1;
            }



        }

        reflectedMainMaze.SetCells(reflectedMazeCells);
        return reflectedMainMaze;
    }

    public static List<Maze> GenerateBoundaryMazesForReflectedMaze(List<Maze> boundaryMazes)
    {

        // Меняем местами topMaze и bottomMaze
        List<Maze> newBoundaryMazes = new List<Maze>();
        foreach (Maze maze in boundaryMazes)
        {
            // Меняем местами верхний и нижний, левый и правый и тд
            if (maze.Side == MazeSide.Top)
            {
                Maze changedMaze = MazeGenerateUtilities.GenerateCloneMaze(maze);
                changedMaze.SetSide(MazeSide.Bottom);
                newBoundaryMazes.Add(changedMaze);
            }
            else if (maze.Side == MazeSide.Bottom)
            {
                Maze changedMaze = MazeGenerateUtilities.GenerateCloneMaze(maze);
                changedMaze.SetSide(MazeSide.Top);
                newBoundaryMazes.Add(changedMaze);
            }
            else if (maze.Side == MazeSide.Right)
            {
                Maze changedMaze = MazeGenerateUtilities.GenerateCloneMaze(maze);
                changedMaze.SetSide(MazeSide.Left);
                newBoundaryMazes.Add(changedMaze);
            }
            else if (maze.Side == MazeSide.Left)
            {
                Maze changedMaze = MazeGenerateUtilities.GenerateCloneMaze(maze);
                changedMaze.SetSide(MazeSide.Right);
                newBoundaryMazes.Add(changedMaze);
            }

            else
            {
                newBoundaryMazes.Add(maze);
            }
        }

        return newBoundaryMazes;

    }


    private static Maze GenerateBoundaryMaze(MazeSide side, MazeCell[][] cells)
    {
        // Убираем точки старта и финиша с основного Maze, чтобы сгенерировать boundary Maze без них
        //cells[startPosition.x][startPosition.y].SetType(MazeCellType.Default);
        //if (finishPosition != -Vector2Int.one)
        //    cells[finishPosition.x][finishPosition.y].SetType(MazeCellType.Default);

        MazeCell[][] newCells = MazeGenerateUtilities.GetMazePartBySide(cells, side);

        // Возвращаем точки старта и финиша на основной Maze
        //cells[startPosition.x][startPosition.y].SetType(MazeCellType.Start);
        //if (finishPosition != -Vector2Int.one)
        //    cells[finishPosition.x][finishPosition.y].SetType(MazeCellType.Finish);

        //Vector2 mazePosition = GetBoundaryMazePosition(side, cells.Length, cells[0].Length);
        Maze boundaryMaze = new(newCells.Length, newCells[0].Length, side);
        boundaryMaze.SetCells(newCells);

        return boundaryMaze;
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

    private void RemoveBoundaryWalls()
    {
        /// Убираем стены снизу и сверху для граничных клеток
        int yMax = Height - 1;
        for (int x = 0; x < Width; x++)
        {
            Cells[x][0].WallsStatus.BottomWall = false;
            Cells[x][yMax].WallsStatus.TopWall = false;
        }

        /// Убираем стены слева и справа для граничных клеток
        int xMax = Width - 1;
        for (int y = 0; y < Height; y++)
        {
            Cells[0][y].WallsStatus.LeftWall = false;
            Cells[xMax][y].WallsStatus.RightWall = false;

        }
    }


    public void CreateBoundaryPasseges()
    {
        int xMax = Width - 1;
        for (int y = 0; y < Height; y++)
        {
            if (Cells[0][y].WallsStatus.BottomWall == Cells[xMax][y].WallsStatus.BottomWall
                && Cells[0][y].WallsStatus.TopWall == Cells[xMax][y].WallsStatus.TopWall
                && y % 2 == 1)
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
        for (int x = 0; x < Width; x++)
        {
            if (Cells[x][0].WallsStatus.LeftWall == Cells[x][yMax].WallsStatus.LeftWall
                && Cells[x][0].WallsStatus.RightWall == Cells[x][yMax].WallsStatus.RightWall
                && x % 2 == 1)
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
                if (Cells[x][y].WallsStatus.LeftWall || Cells[x][y].WallsStatus.TopWall)
                    Cells[x][y].ColumnsStatus.TopLeft = true;
                else if (x > 0 && y < Height - 1 && Cells[x - 1][y].WallsStatus.TopWall && Cells[x][y + 1].WallsStatus.LeftWall)
                    Cells[x][y].ColumnsStatus.TopLeft = true;
                else
                    Cells[x][y].ColumnsStatus.TopLeft = false;

                if (Cells[x][y].WallsStatus.LeftWall || Cells[x][y].WallsStatus.BottomWall)
                    Cells[x][y].ColumnsStatus.BottomLeft = true;
                else if (x > 0 && y > 0 && Cells[x - 1][y].WallsStatus.BottomWall && Cells[x][y - 1].WallsStatus.LeftWall)
                    Cells[x][y].ColumnsStatus.BottomLeft = true;
                else
                    Cells[x][y].ColumnsStatus.BottomLeft = false;

                if (Cells[x][y].WallsStatus.RightWall || Cells[x][y].WallsStatus.TopWall)
                    Cells[x][y].ColumnsStatus.TopRight = true;
                else if (x < Width - 1 && y < Height - 1 && Cells[x + 1][y].WallsStatus.TopWall && Cells[x][y + 1].WallsStatus.RightWall)
                    Cells[x][y].ColumnsStatus.TopRight = true;
                else
                    Cells[x][y].ColumnsStatus.TopRight = false;

                if (Cells[x][y].WallsStatus.RightWall || Cells[x][y].WallsStatus.BottomWall)
                    Cells[x][y].ColumnsStatus.BottomRight = true;
                else if (x < Width - 1 && y > 0 && Cells[x + 1][y].WallsStatus.BottomWall && Cells[x][y - 1].WallsStatus.RightWall)
                    Cells[x][y].ColumnsStatus.BottomRight = true;
                else
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

    //private MazeCell GetNeignbourWithMinDistance(MazeCell[] neighbours)
    //{
    //    if (neighbours.Length == 0)
    //        return null;

    //    int minDistance = neighbours[0].DistanceFromStart, minIndex = 0;
    //    for (int i = 0; i < neighbours.Length; i++)
    //        if (minDistance > neighbours[i].DistanceFromStart)
    //        {
    //            minDistance = neighbours[i].DistanceFromStart;
    //            minIndex = i;
    //        }

    //    return neighbours[minIndex];
    //}

    //private void ChangeDistance(MazeCell startCell, List<MazeCell> visited)
    //{
    //    if (visited.Count < 1)
    //    {
    //        return;
    //    }

    //    List<MazeCell> arrayVisited = visited;

    //    int index = 0;
    //    MazeCell current = arrayVisited[index];
    //    MazeCell previous = startCell;
    //    while (current != startCell && index < arrayVisited.Count - 1)
    //    {
    //        if (previous.DistanceFromStart > 0)
    //            current.SetDistanceFromStart(Mathf.Min(current.DistanceFromStart, previous.DistanceFromStart + 1));

    //        previous = current;
    //        current = arrayVisited[index++];

    //    }

    //}

}
