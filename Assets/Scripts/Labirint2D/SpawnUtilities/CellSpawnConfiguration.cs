using System.Collections;
using UnityEngine;

[System.Serializable]
public struct DecorationMaterials
{
    public GameObject Wall;
    public GameObject Column;
    public GameObject Floor;
}

public class CellSpawnConfiguration : MonoBehaviour
{
    public DecorationMaterials Decoration;

}