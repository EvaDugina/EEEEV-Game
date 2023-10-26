
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator2D
{
    private int MainMazeWidth = 33;
    private int MainMazeHeight = 33;


    private struct MazeGenerateInfo
    {
        public int Id;
        public int Width;
        public int Height;
        public List<PortalIn> PortalsIn;
        public List<PortalOut> PortalsOut;
    };
    private List<MazeGenerateInfo> MazesInfo;


    public LevelGenerator2D(int width, int height)
    {
        MainMazeWidth = width;
        MainMazeHeight = height;
        MazesInfo = new List<MazeGenerateInfo>();
    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */

    /// <summary>
    // Генерируем уровень
    /// </summary>
    public Level GenerateLevel()
    {

        int id = 0;
        Level level2D = new Level(CreateMaze(ref id, MazeAreaType.Main));
        //level2D.SecondaryMazes.Add(CreateMaze(ref id, MazeAreaType.Field));
        //level2D.SecondaryMazes.Add(CreateMaze(ref id, MazeAreaType.Room));
        //level2D.SecondaryMazes.Add(CreateMaze(ref id, MazeAreaType.Corridor));
        //SetPortals(level2D);

        MazesInfo = new List<MazeGenerateInfo>();

        return level2D;

    }



    private void SetPortals(Level level2D)
    {
        level2D.MainMaze.SetPortalsIn(MazesInfo[0].PortalsIn);
        level2D.MainMaze.SetPortalsOut(MazesInfo[0].PortalsOut);
        int index = 1;
        foreach (Maze maze in level2D.SecondaryMazes)
        {
            maze.SetPortalsIn(MazesInfo[index].PortalsIn);
            maze.SetPortalsOut(MazesInfo[index].PortalsOut);
            index++;
        }
    }

    private Maze CreateMaze(ref int id, MazeAreaType type)
    {

        int width, height;

        if (type == MazeAreaType.Field)
        {
            width = MainMazeWidth / 4;
            height = MainMazeHeight / 4;
        }
        else if (type == MazeAreaType.Room)
        {
            width = MainMazeWidth / 5;
            height = MainMazeHeight / 5;
        }
        else if (type == MazeAreaType.Corridor)
        {
            width = MainMazeWidth / 2;
            height = 2;
        }
        else
        {
            width = MainMazeWidth;
            height = MainMazeHeight;
        }

        MazeGenerateInfo info = new()
        {
            Id = id,
            Width = width,
            Height = height,
            PortalsIn = new List<PortalIn>(),
            PortalsOut = new List<PortalOut>()
        };
        MazesInfo.Add(info);

        MazeGenerator2D mazeGenerator;
        if (type != MazeAreaType.Main)
        {
            CreatePortals(0, id, MazeAreaType.Main);
            CreatePortals(id, 0, type);
            mazeGenerator = new MazeGenerator2D(id, type, width, height,
                MazesInfo[0].PortalsOut[id - 1].Position - new Vector2Int(width, height));
        }
        else
        {
            mazeGenerator = new MazeGenerator2D(id, type, width, height, Vector2Int.zero);
        }
        id++;

        return mazeGenerator.Generate();


    }

    // Придумываем координаты для порталов
    private List<Vector2Int> GetPortalPositions(int id, MazeAreaType type)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        if (type != MazeAreaType.Main)
        {
            // PortalsIn
            list.Add(new Vector2Int(0, (MazesInfo[id].Height - 1) / 2));

            // PortalsOut
            list.Add(new Vector2Int((MazesInfo[id].Width - 1) / 2, MazesInfo[id].Height - 1));
            list.Add(new Vector2Int((MazesInfo[id].Width - 1) / 2, 0));
            list.Add(new Vector2Int(MazesInfo[id].Width - 1, (MazesInfo[id].Height - 1) / 2));
        }
        else
        {
            int xStartRandom = 2;
            int xEndRandom = MazesInfo[id].Width - 4;
            int yStartRandom = 2;
            int yEndRandom = MazesInfo[id].Height - 4;

            int xRandom = Random.Range(xStartRandom, xEndRandom);
            int yRandom = Random.Range(yStartRandom, yEndRandom);
            while (!CheckUniquePortalPosition(xRandom, yRandom, MazesInfo[id]))
            {
                xRandom = Random.Range(xStartRandom, xEndRandom);
                yRandom = Random.Range(yStartRandom, yEndRandom);
            }
            // PortalsOut
            list.Add(new Vector2Int(xRandom, yRandom));

            // PortalsIn
            list.Add(new Vector2Int(xRandom + 1, yRandom));


        }

        return list;
    }

    private void CreatePortals(int id1, int id2, MazeAreaType type)
    {
        int index = 0;
        foreach (Vector2Int position in GetPortalPositions(id1, type))
        {
            if ((index == 0 && type == MazeAreaType.Main) || (index != 0 && type != MazeAreaType.Main))
            {
                PortalOut portal = new PortalOut()
                {
                    ToMazeId = id2,
                    Position = new Vector2Int(position.x, position.y)
                };
                MazesInfo[id1].PortalsOut.Add(portal);
            }
            else
            {
                PortalIn portal = new PortalIn()
                {
                    FromMazeId = id2,
                    Position = new Vector2Int(position.x, position.y)
                };
                MazesInfo[id1].PortalsIn.Add(portal);

            }
            index++;
        }



    }

    private bool CheckUniquePortalPosition(int x, int y, MazeGenerateInfo info)
    {
        // Проверяем нет ли в диапазоне 1 клетки других точек входа/выхода
        foreach (PortalIn portal in info.PortalsIn)
            if (Mathf.Abs(portal.Position.x - x) <= 1 && Mathf.Abs(portal.Position.y - y) <= 1)
                return false;
        foreach (PortalOut portal in info.PortalsOut)
            if (Mathf.Abs(portal.Position.x - x) <= 1 && Mathf.Abs(portal.Position.y - y) <= 1)
                return false;
        return true;
    }

}
