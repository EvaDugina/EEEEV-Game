using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PortalsHandler
{
    int CountSecondaryAreas;
    int DistanceBetweenOrederedPortals;

    public PortalsHandler(int countSecondaryAreas, int distanceBetweenOrederedPortals) {
        CountSecondaryAreas = countSecondaryAreas;
        DistanceBetweenOrederedPortals = distanceBetweenOrederedPortals;
    }

    public Portal CreatePortalIn(Area fromArea, int toAreaId, StaticPositionParameter fromAreaParameter, AreaStructure toAreaStructure, List<Portal> areaPortals)
    {

        Vector2Int portalPosition;
        if (fromAreaParameter == StaticPositionParameter.Order)
        {
            portalPosition = FindPortalPositionByOrder(fromArea, toAreaStructure);
        } 
        else
            portalPosition = FindPortalPosition(fromArea, fromAreaParameter, toAreaStructure, areaPortals, fromArea.MainMaze.StartPosition, fromArea.MainMaze.FinishPosition);

        return new Portal()
        {
            FromAreaId = fromArea.Id,
            ToAreaId = toAreaId,
            Position = new Vector2Int(portalPosition.x, portalPosition.y)
        };
    }

    public Vector2Int FindPortalPositionByOrder(Area fromArea, AreaStructure toAreaStructure)
    {
        return AreaStructureHandler.GeneratePortalOrderedPosition(fromArea, DistanceBetweenOrederedPortals, CountSecondaryAreas);
    }

    public static Vector2Int FindPortalPosition(Area fromArea, StaticPositionParameter fromAreaParameter, AreaStructure toAreaStructure, List<Portal> areaPortals, Vector2Int startPosition, Vector2Int finishPosition)
    {
        Vector2Int position = AreaStructureHandler.GetPositionByStaticParameter(fromAreaParameter, fromArea.Width, fromArea.Height);
        while ((position.x == startPosition.x && position.y == startPosition.y) || (position.x == finishPosition.x && position.y == finishPosition.y)
            || !CheckCellPasseges(fromArea, toAreaStructure, position) || !CheckUniquePortalPosition(position, areaPortals))
        {
            position = AreaStructureHandler.GetPositionByStaticParameter(fromAreaParameter, fromArea.Width, fromArea.Height);
            Debug.Log(fromArea.MainMaze.Cells[position.x][position.y].WallsStatus);
        }

        return position;
    }

    public static bool CheckCellPasseges(Area fromArea, AreaStructure toAreaStructure, Vector2Int portalPosition)
    {
        if (fromArea.Type != AreaType.Main)
            return true;

        if (AreaStructureHandler.GetCountPortalsOutByAreaStructure(toAreaStructure) == 1
            && HasOnlyStraightPassages(fromArea, portalPosition))
            return true;
        else if (AreaStructureHandler.GetCountPortalsOutByAreaStructure(toAreaStructure) >= 3
            && IsCrossroad(fromArea, portalPosition))
            return true;
        return false;
    }

    public static bool IsCrossroad(Area area, Vector2Int cellPosition)
    {
        return !area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.TopWall
            && !area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.BottomWall
            && !area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.RightWall
            && !area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.LeftWall;
    }

    public static bool HasOnlyStraightPassages(Area area, Vector2Int cellPosition)
    {
        bool way1 = (!area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.TopWall
            && !area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.BottomWall)
            || (area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.TopWall
            && area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.BottomWall);

        bool way2 = (!area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.RightWall
            && !area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.LeftWall)
            || (area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.RightWall
            && area.MainMaze.Cells[cellPosition.x][cellPosition.y].WallsStatus.LeftWall);

        return way1 && way2;
    }

    //public static bool HasOnlyStraightPassage(Area area, Vector2Int cellPosition)
    //{
    //    bool horizontalPassage = HasHorizontalStraightPassage(area, cellPosition);
    //    bool verticalPassage = HasVerticalStraightPassage(area, cellPosition);

    //    // true - если либо есть прямой проход, либо нет не прямых развилок буквой Т. Те. либо "-", либо "+"
    //    return (horizontalPassage && verticalPassage);

    //}

    //private static bool HasVerticalStraightPassage(Area area, Vector2Int cellPosition)
    //{
    //    if (0 < cellPosition.y && cellPosition.y < area.Height - 1)
    //    {
    //        Vector2Int position1, position2;
    //        position1 = cellPosition - Vector2Int.up;
    //        position2 = cellPosition - Vector2Int.down;

    //        bool way1 = area.MainMaze.HasWayBetweenCells(position1, cellPosition);
    //        bool way2 = area.MainMaze.HasWayBetweenCells(cellPosition, position2);

    //        return way1 && way2;
    //    }
    //    return false;

    //}

    //private static bool HasHorizontalStraightPassage(Area area, Vector2Int cellPosition)
    //{
    //    if (0 < cellPosition.x && cellPosition.x < area.Width - 1)
    //    {
    //        Vector2Int position1, position2;
    //        position1 = cellPosition - Vector2Int.right;
    //        position2 = cellPosition - Vector2Int.left;

    //        bool way1 = area.MainMaze.HasWayBetweenCells(position1, cellPosition);
    //        bool way2 = area.MainMaze.HasWayBetweenCells(cellPosition, position2);

    //        return way1 && way2;
    //    }

    //    return false;

    //}

    public static bool CheckUniquePortalPosition(Vector2Int position, List<Portal> areaPortals)
    {
        // Проверяем нет ли в диапазоне 1 клетки других точек входа/выхода
        foreach (Portal portal in areaPortals)
            if (Mathf.Abs(portal.Position.x - position.x) <= 1 && Mathf.Abs(portal.Position.y - position.y) <= 1)
                return false;
        return true;
    }
}