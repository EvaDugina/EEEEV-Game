using System.Collections;
using UnityEngine;


public class CellsSpawnPrefabs : MonoBehaviour
{

    public GameObject EmptyDecorationPrefab;
    public GameObject WheatFieldDecorationPrefab;
    public GameObject BirchFieldDecorationPrefab;
    public GameObject RedRoomDecorationPrefab;

    public GameObject GetCellDecorationByDecoration(CellDecoration decoration)
    {
        if (decoration == CellDecoration.WheatField)
            return WheatFieldDecorationPrefab;
        else if (decoration == CellDecoration.BirchField)
            return BirchFieldDecorationPrefab;
        else if (decoration == CellDecoration.RedRoom)
            return RedRoomDecorationPrefab;
        else
            return EmptyDecorationPrefab;
    }

}