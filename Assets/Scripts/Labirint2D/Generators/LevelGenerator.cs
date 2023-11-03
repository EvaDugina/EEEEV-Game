using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    public Level Level;

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
        Level = new(CreateArea(ref id, AreaType.Main, 1.0f));
        id++;

        foreach (Parameters parameters in LevelParameters)
        {
            if (parameters.Status)
            {
                if (AreaGenerator.IsValidAreaSize(Width, Height, parameters.Type, parameters.GenerateParams.SizeKoeff))
                {
                    // Генерируем, добавляем Area и заполняем List Portals
                    Level.SecondaryAreas.Add(CreateArea(ref id, parameters.Type, parameters.GenerateParams.SizeKoeff));
                    id++;
                }
            }
        }

        return Level;

    }


    private Area CreateArea(ref int id, AreaType type, float koeff)
    {

        // Высчитываем размер Areas
        int width, height;
        if (type != AreaType.Main)
        {
            width = (int)(Width * koeff);
            if (type != AreaType.Corridor)
                height = (int)(Height * koeff);
            else height = 1;
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
        id++;

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

}
