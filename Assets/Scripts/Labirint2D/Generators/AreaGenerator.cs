
using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaGenerator
{

    private Area Area;

    private AreaStructure AreaStructure;
    private MazeStructure MazeStructure;

    public static bool IsValidAreaSize(int width, int height, AreaType type, float sizeKoeff)
    {
        if (type != AreaType.Corridor)
        {
            if (width / sizeKoeff < 3 || height / sizeKoeff < 3)
                return false;
        }
        else
        {
            if (width / sizeKoeff < 3)
                return false;
        }

        return true;
    }



    public AreaGenerator(int id, AreaType areaType, int width, int height)
    {
        Area = new Area(id, areaType, Vector2Int.zero, width, height);
        AreaStructure = AreaStructureHandler.GetAreaStructureByAreaType(Area.Type);
        MazeStructure = MazeStructureHandler.GetMazeStructureByAreaType(Area.Type);
        
    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */

    public Area GenerateArea() {

        int id = Area.Id;
        MazeGenerator mazeGenerator = new MazeGenerator(Area.Width, Area.Height, MazeStructure);

        // Генерируем лабиринт с точкой финиша
        Area.MainMaze = mazeGenerator.GenerateMainMaze(AreaStructure.StartParameters, AreaStructure.FinishParameter);

        // Генерируем граничные лабиринты
        if (Area.Topology == AreaTopology.Toroid)
            Area.BoundaryMazes = mazeGenerator.GenerateBoundaryMazes();
        else
            Area.MainMaze.Cells = mazeGenerator.GenerateBoundaryWalls();

        return Area;

    }

    public static List<Portal> CreatePortals(Area fromArea, int toAreaId)
    {
        AreaStructure areaStructure = AreaStructureHandler.GetAreaStructureByAreaType(fromArea.Type);

        List<Portal> portals = new List<Portal>();
        foreach (StaticPositionParameter parameter in areaStructure.PortalOutParameters) {
            Portal portalOut = PortalsHandler.CreatePortalOut(fromArea, toAreaId, parameter, fromArea.Portals);
            portals.Add(portalOut);
        }
        return portals;
    }



}