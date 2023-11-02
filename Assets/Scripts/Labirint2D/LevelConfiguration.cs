using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public struct AreaParams
{
    public AreaType Type;
    public bool Status;

    [Range(0.1f, 1.0f)]
    public int SizeKoeff;

    public Vector2Int CellSize;
}

public class LevelConfiguration
{

    //[Header("Параметры лабиринтов")]
    // TODO: Сделать через массив
    //public List<AreaParams> AreaParams = new();

    [Header("Параметры ROOM-лабиринта")]
    public AreaParams RoomAreaParams;

    [Header("Параметры FIELD-лабиринта")]
    public AreaParams FieldAreaParams;

    [Header("Параметры CORRIDOR-лабиринта")]
    public AreaParams CorridorAreaParams;

    private List<AreaParams> AreasParams;

    public void SetAreaParamsToList() {
        AreasParams.Add(new AreaParams() { 
            Type = AreaType.Main,
            Status = true,
            SizeKoeff = 1,
            CellSize = new Vector2Int(1, 1)
        });
        AreasParams.Add(RoomAreaParams);
        AreasParams.Add(FieldAreaParams);
        AreasParams.Add(CorridorAreaParams);
    }

    public List<AreaParams> GetAreasParams() { 
        return AreasParams;
    }

    public AreaParams GetAreaParamsByType(AreaType type) { 
        foreach (AreaParams areaParams in AreasParams) 
            if (areaParams.Type == type)
                return areaParams;
        return new AreaParams();
    }

}