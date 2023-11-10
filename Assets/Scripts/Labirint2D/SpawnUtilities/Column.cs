
using UnityEngine;

public enum ColumnType
{ 
    TopLeft, TopRight, BottomLeft, BottomRight
}

//public enum ColumnStatus { 
//    None, Default, Crossroad, Solid
//}

public class Column
{
    public ColumnType Type;
    public Material ColumnMaterial; 

    public Column(ColumnType type, Material enableColumn, Material disableColumn)
    {
        Type = type;
        ColumnMaterial = enableColumn;
    }
}
