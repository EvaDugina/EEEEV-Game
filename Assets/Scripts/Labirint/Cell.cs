using System;
using Unity.VisualScripting;
using UnityEngine;


public enum Visibility
{
    Enable,
    Disable
}

public class Cell : MonoBehaviour
{

    public GameObject LeftWall;
    public GameObject BottomWall;
    public GameObject Floor;
    public GameObject Column;

    [NonSerialized] public Visibility ColumnVisibility = Visibility.Disable;

    public Material EnableColumnMaterial;
    public Material DisableColumnMaterial;

    [NonSerialized] public float XReal;
    [NonSerialized] public float ZReal;
}
