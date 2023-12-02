using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;


public class Cell
{

    public GameObject CellPrefab;

    public int Width;
    public int Height;
    public int Length;

    public Cell(GameObject cellDecorationPrefab, Vector3Int cellSize) {
        CellPrefab = cellDecorationPrefab;
        Width = cellSize.x;
        Height = cellSize.y;
        Length = cellSize.z;
    }
}