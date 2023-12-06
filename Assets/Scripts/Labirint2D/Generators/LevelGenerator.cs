using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LevelGenerator
{
    private Level Level;

    private int Width;
    private int Height;

    private List<Parameters> LevelParameters;

    public LevelGenerator(int width, int height, List<Parameters> levelParams)
    {
        Width = width;
        Height = height;
        LevelParameters = SortLevelParametersByOrder(levelParams);
    }

    private List<Parameters> SortLevelParametersByOrder(List<Parameters> levelParams)
    {
        levelParams.Sort((p1, p2) => p1.Order.CompareTo(p2.Order));
        return levelParams;
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
        Area area = CreateArea(id, AreaType.Main, LevelParameters[0].GenerateParams.SizeKoeff);
        Level = new Level(area);
        id++;

        //Level.SetReflectedArea(CreateReflectedArea(id, area));
        //id++;

        foreach (Parameters parameters in LevelParameters)
        {
            if (parameters.Type != AreaType.Main && parameters.Status)
            {
                if (AreaGenerator.IsValidAreaSize(Width, Height, parameters.Type, parameters.GenerateParams.SizeKoeff))
                {

                    // Генерируем, добавляем Area и заполняем List Portals
                    area = CreateArea(id, parameters.Type, parameters.GenerateParams.SizeKoeff);
                    Level.SecondaryAreas.Add(area);
                    id++;
                }
            }
        }

        // Создаём и добавляем порталы
        AddPortals(Level);

        return Level;

    }

    private void AddPortals(Level level)
    {
        Vector2Int finishPosition = Level.MainArea.MainMaze.FinishPosition;
        PortalsHandler portalsHandler = new PortalsHandler(level.SecondaryAreas.Count, Level.MainArea.MainMaze.Cells[finishPosition.x][finishPosition.y].DistanceFromStart / (level.SecondaryAreas.Count + 1));


        for (int i = level.SecondaryAreas.Count-1; i >= 0; i--)
        {
            List<Portal> portals = CreatePortals(portalsHandler, Level.MainArea, level.SecondaryAreas[i]);
            Level.MainArea.AddPortals(portals);

            portals = CreatePortals(portalsHandler, level.SecondaryAreas[i], Level.MainArea);
            level.SecondaryAreas[i].AddPortals(portals);
        }
    }

    private Area CreateArea(int id, AreaType type, float koeff)
    {

        // Высчитываем размер Areas
        int width, height;
        if (type != AreaType.Main)
        {
            width = (int)(Width * koeff);
            if (type != AreaType.Corridor)
                height = (int)(Height * koeff);
            else height = 1;

            // Гарантируем нечётность сторон лабиринта
            if (width % 2 == 0) width += 1;
            if (height % 2 == 0) height += 1;
        }
        else
        {
            width = Width;
            height = Height;
        }

        AreaGenerator areaGenerator = new AreaGenerator(id, type, width, height);

        // Генерируем Area
        Area area = areaGenerator.GenerateArea();

        return area;
    }

    public Area CreateReflectedArea(int id, Area prototypeArea)
    {
        return AreaGenerator.GenerateReflectedToroidalArea(id, prototypeArea);
    }


    public List<Portal> CreatePortals(PortalsHandler portalHandler, Area fromArea, Area toArea)
    {
        AreaStructure fromAreaStructure = AreaStructureHandler.GetAreaStructureByAreaType(fromArea.Type);
        AreaStructure toAreaStructure = AreaStructureHandler.GetAreaStructureByAreaType(toArea.Type);

        List<Portal> portals = new List<Portal>();
        foreach (StaticPositionParameter parameter in fromAreaStructure.PortalOutParameters)
        {
            Portal portalOut = portalHandler.CreatePortalIn(fromArea, toArea.Id, parameter, toAreaStructure, fromArea.Portals);
            portals.Add(portalOut);
        }
        return portals;
    }

}
