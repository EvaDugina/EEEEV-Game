using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class MazeStructureGenerator
{
    private int Width;
    private int Height;

    private Vector2Int StartPosition;

    private MazeStructure Structure;
    private CellInfo[][] CellsInfo;
    private MazeCell[][] Cells;

    public MazeStructureGenerator(int width, int height, MazeStructure structure) {
        Width = width;
        Height = height;
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

    public MazeCell[][] GenerateStructure(Vector2Int startPosition)
    {
        StartPosition = startPosition;

        GenerateForm();
        GenerateRouting();
        GenerateTexture();

        return Cells;
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

        if (Structure.Routing == MazeRouting.ParticallyBraid)
        {
            GenerateRoutingParticallyBraid();
        }
        else if (Structure.Routing == MazeRouting.HighSparse)
        {
            GenerateRoutingParticallyBraid();
            //GenerateRoutingHighSparse();
        }
        else
        {
            GenerateRoutingNone();
        }

    }

    private void GenerateTexture()
    {
        if (Structure.Texture == MazeTexture.Longitudinall)
        {

        }
    }

    /// <summary>
    // Генерируем лабиринт, удаляя стены
    /// </summary>


    public void GenerateRoutingParticallyBraid()
    {

        MazeCell current = Cells[StartPosition.x][StartPosition.y];
        CellsInfo[StartPosition.x][StartPosition.y].Visited = true;
        current.SetDistanceFromStart(0);

        Stack<MazeCell> stackVisited = new Stack<MazeCell>();

        do
        {
            int x = current.X;
            int y = current.Y;

            List<MazeCell> unvisitedNeighbours = new List<MazeCell>();

            if (x > 0 && !CellsInfo[x - 1][y].Visited && Cells[x - 1][y].Status != MazeCellStatus.Disable)
                unvisitedNeighbours.Add(Cells[x - 1][y]);
            if (y > 0 && !CellsInfo[x][y - 1].Visited && Cells[x][y - 1].Status != MazeCellStatus.Disable)
                unvisitedNeighbours.Add(Cells[x][y - 1]);
            if (x < Width - 1 && !CellsInfo[x + 1][y].Visited && Cells[x + 1][y].Status != MazeCellStatus.Disable)
                unvisitedNeighbours.Add(Cells[x + 1][y]);
            if (y < Height - 1 && !CellsInfo[x][y + 1].Visited && Cells[x][y + 1].Status != MazeCellStatus.Disable)
                unvisitedNeighbours.Add(Cells[x][y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeCell choosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveBetweenWalls(current, choosen);

                CellsInfo[choosen.X][choosen.Y].Visited = true;
                stackVisited.Push(choosen);

                choosen.SetDistanceFromStart(current.DistanceFromStart + 1);

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

                if (x > 2 && indexRandomNeighbor == 0 && Cells[x - 1][y].Status != MazeCellStatus.Disable)
                    randomNeighbor = Cells[x - 1][y];
                else if (x < Width - 2 && indexRandomNeighbor == 1 && Cells[x + 1][y].Status != MazeCellStatus.Disable)
                    randomNeighbor = Cells[x + 1][y];
                else if (y > 2 && indexRandomNeighbor == 2 && Cells[x][y - 1].Status != MazeCellStatus.Disable)
                    randomNeighbor = Cells[x][y - 1];
                else if (y < Height - 2 && indexRandomNeighbor == 3 && Cells[x][y + 1].Status != MazeCellStatus.Disable)
                    randomNeighbor = Cells[x][y + 1];

                if (randomNeighbor != null)
                {
                    RemoveBetweenWalls(current, randomNeighbor);
                }
            }

        } while (stackVisited.Count > 0);

    }

    public void GenerateRoutingHighSparse()
    {

    }

    public void GenerateRoutingNone()
    {

        // Удаляем все стены
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cells[x][y].DisableAllWalls();
            }
        }
    }


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

}