using System.Collections;
using UnityEngine;

public class CellsSpawnConfiguration : MonoBehaviour
{

    public DecorationMaterials EmptyDecoration;
    public DecorationMaterials WheatFieldDecoration;
    public DecorationMaterials BirchFieldDecoration;
    public DecorationMaterials RedRoomDecoration;

    public DecorationMaterials GetMaterialsByDecoration(CellDecoration decoration)
    {
        if (decoration == CellDecoration.WheatField)
            return WheatFieldDecoration;
        else if (decoration == CellDecoration.BirchField)
            return BirchFieldDecoration;
        else if (decoration == CellDecoration.RedRoom)
            return RedRoomDecoration;
        else
            return EmptyDecoration;
    }

}