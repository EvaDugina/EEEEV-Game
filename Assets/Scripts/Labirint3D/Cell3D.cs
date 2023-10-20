using System;
using UnityEngine;


public enum Visibility
{
    Enable,
    Disable
}

public class Cell3D : MonoBehaviour
{

    public GameObject LeftWall;
    public GameObject BottomWall;
    public GameObject Floor;
    public GameObject Column;
    public GameObject TextDistance;

    [NonSerialized] public Visibility ColumnVisibility = Visibility.Disable;

    public Material EnableColumnMaterial;
    public Material DisableColumnMaterial;

}
