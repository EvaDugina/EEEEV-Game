
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

        MazeGenerator mazeGenerator = new MazeGenerator(Area.Width, Area.Height, MazeStructure);

        // Генерируем лабиринт с точкой финиша
        Area.SetMainMaze(mazeGenerator.GenerateMainMaze(AreaStructure.StartParameters, AreaStructure.FinishParameter));

        // Генерируем граничные лабиринты
        if (Area.Topology == AreaTopology.Toroid)
            Area.SetBoundaryMazes(mazeGenerator.GenerateBoundaryMazes(MazeStructure.Routing));

        return Area;

    }

    public static Area GenerateReflectedToroidalArea(int id, Area prototypeArea)
    {
        Area area = new Area(id, AreaType.ReflectedMain, Vector2Int.zero, prototypeArea.Width, prototypeArea.Height);

        // Генерируем лабиринт с точкой финиша
        area.SetMainMaze(MazeGenerator.GenerateReflectedMainMaze(prototypeArea.MainMaze));

        // Генерируем граничные лабиринты
        //if (prototypeArea.Topology == AreaTopology.Toroid)
        //    area.SetBoundaryMazes(MazeGenerator.GenerateBoundaryMazesForReflectedMaze(prototypeArea.GetBoundaryMazes()));

        return area;
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