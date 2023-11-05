using System.Collections.Generic;
using UnityEngine;


public class CellHandler : MonoBehaviour
{
    public CellSpawnConfiguration CellSpawnConfugiration;


    public Cell CreateCellByDecoration(CellDecoration decoration)
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
