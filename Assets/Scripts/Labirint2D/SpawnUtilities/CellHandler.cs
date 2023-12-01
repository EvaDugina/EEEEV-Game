using System.Collections.Generic;
using UnityEngine;


public class CellHandler : MonoBehaviour
{
    public CellsSpawnConfiguration CellSpawnConfiguration;


    public Cell CreateCellByDecoration(CellDecoration decoration)
    {
        DecorationMaterials materials = CellSpawnConfiguration.GetMaterialsByDecoration(decoration);

        List<Wall> walls = new List<Wall>() {
            new Wall(WallType.Top, materials.Wall),
            new Wall(WallType.Right, materials.Wall),
            new Wall(WallType.Bottom, materials.Wall),
            new Wall(WallType.Left, materials.Wall),
        };

        List<Column> columns = new List<Column>() {
            new Column(ColumnType.TopLeft, materials.Column, materials.Column),
            new Column(ColumnType.TopRight, materials.Column, materials.Column),
            new Column(ColumnType.BottomRight, materials.Column, materials.Column),
            new Column(ColumnType.BottomLeft, materials.Column, materials.Column),
        };

        Floor floor = new Floor(materials.Floor);

        return new Cell(walls, columns, floor);
    }

}
