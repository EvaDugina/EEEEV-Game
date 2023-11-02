﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PortalsHandler
{
    public static Portal CreatePortalOut(Area fromArea, int toAreaId, StaticPositionParameter parameter, List<Portal> areaPortals)
    {

        Vector2Int portalPosition = FindPortalPosition(fromArea.Width, fromArea.Height, parameter, areaPortals);
        
        return new Portal()
        {
            FromAreaId = fromArea.Id,
            ToAreaId = toAreaId,
            Position = new Vector2Int(portalPosition.x, portalPosition.y)
        };
    }

    public static Vector2Int FindPortalPosition(int width, int height, StaticPositionParameter parameter, List<Portal> areaPortals) {
        Vector2Int position = AreaStructureHandler.GetPositionByStaticParameter(parameter, width, height);
        while (!CheckUniquePortalPosition(position, areaPortals))
        {
            position = AreaStructureHandler.GetPositionByStaticParameter(parameter, width, height);
        }

        return position;
    }


    public static bool CheckUniquePortalPosition(Vector2Int position, List<Portal> areaPortals)
    {
        // Проверяем нет ли в диапазоне 1 клетки других точек входа/выхода
        foreach (Portal portal in areaPortals)
            if (Mathf.Abs(portal.Position.x - position.x) <= 1 && Mathf.Abs(portal.Position.y - position.y) <= 1)
                return false;
        return true;
    }
}