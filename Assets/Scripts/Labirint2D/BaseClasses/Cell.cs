using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;


public class Cell
{

    public List<Wall> Walls;
    public List<Column> Columns;
    public Floor Floor;

    public float Width;
    public float Length;
    public float Height;

    public Cell(List<Wall> walls, List<Column> columns, Floor floor)
    {
        Walls = walls;
        Columns = columns;
        Floor = floor;

        Width = 1.0f;
        Height = 1.0f;
        Length = 1.0f;
    }

    public Material GetFloorMaterial()
    {
        return Floor.Material;
    }

    public void SetSize(Vector3 size) {
        Width = size.x;
        Height = size.y;
        Length = size.z;
    }
}