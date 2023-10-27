using System;
using UnityEngine;

[RequireComponent(typeof(LabirintView))]
public class Labirint2D : MonoBehaviour
{
    public GameObject LabirintForm;
    public GameObject QuadFloor;
    public GameObject CellsFolder;

    [NonSerialized] public LabirintView LabirintView;


    private void Awake()
    {
        LabirintView = GetComponent<LabirintView>();
    }

}
