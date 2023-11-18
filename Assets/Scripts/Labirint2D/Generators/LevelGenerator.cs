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
        LevelParameters = levelParams;
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

        Level.SetReflectedArea(CreateReflectedArea(id, area));
        id++;
        

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

        return Level;

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

        //// Создаём ячейку, для хранения всех порталов текущего Area
        //AreasConfig info = new()
        //{
        //    Id = id,
        //    Width = width,
        //    Height = height,
        //    Portals = new List<Portal>()
        //};
        //AreasConfigList.Add(info);

        AreaGenerator areaGenerator = new AreaGenerator(id, type, width, height);

        // Генерируем Area
        Area area = areaGenerator.GenerateArea();

        // Создаём и добавляем порталы
        if (area.Type != AreaType.Main) {

            List<Portal> portals = AreaGenerator.CreatePortals(Level.MainArea, area.Id);
            Level.MainArea.AddPortals(portals);

            portals = AreaGenerator.CreatePortals(area, Level.MainArea.Id);
            area.AddPortals(portals);
        }

        return area;
    }

    public Area CreateReflectedArea(int id, Area prototypeArea) {
        return AreaGenerator.GenerateReflectedToroidalArea(id, prototypeArea);
    }

}
