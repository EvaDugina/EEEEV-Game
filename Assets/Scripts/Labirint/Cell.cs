using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject LeftWall;
    public GameObject BottomWall;

    [NonSerialized] public float XReal;
    [NonSerialized] public float ZReal;
}
