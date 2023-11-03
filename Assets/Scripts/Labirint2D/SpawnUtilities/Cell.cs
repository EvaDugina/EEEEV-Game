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


public enum CellDecoration
{
    Empty, WheatField, BirchField, RedRoom
}


public class CellHandler
{

    public CellSpawnConfiguration CellSpawnConfugiration;

    public CellHandler(CellSpawnConfiguration cellSpawnConfugiration)
    {
        CellSpawnConfugiration = cellSpawnConfugiration;
    }

    public Cell GetCellByDecoration(CellDecoration decoration)
    {
        DecorationMaterials materials = CellSpawnConfugiration.GetMaterialsByDecoration(decoration);

        List<Wall> walls = new List<Wall>() {
            new Wall(WallType.Top, materials.Wall),
            new Wall(WallType.Right, materials.Wall),
            new Wall(WallType.Bottom, materials.Wall),
            new Wall(WallType.Left, materials.Wall),
        };

        List<Column> columns = new List<Column>() {
            new Column(ColumnType.TopLeft, materials.EnableColumn, materials.DisableColumn),
            new Column(ColumnType.TopRight, materials.EnableColumn, materials.DisableColumn),
            new Column(ColumnType.BottomRight, materials.EnableColumn, materials.DisableColumn),
            new Column(ColumnType.BottomLeft, materials.EnableColumn, materials.DisableColumn),
        };

        Floor floor = new Floor(materials.Floor);

        return new Cell(walls, columns, floor);
    }

}