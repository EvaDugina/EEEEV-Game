using System.Collections;
using UnityEngine;

[System.Serializable]
public struct DecorationMaterials
{
    public Material Wall;
    public Material Column;
    public Material Floor;
}

public class CellSpawnConfiguration : MonoBehaviour
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