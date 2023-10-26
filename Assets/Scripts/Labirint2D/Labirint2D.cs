using System;
using UnityEngine;


public struct LabirintSiblingConnectionPoints
{
    public Vector3 LeftPoint;
    public Vector3 RightPoint;
    public Vector3 TopPoint;
    public Vector3 BottomPoint;
};

[RequireComponent(typeof(LabirintView))]
public class Labirint2D : MonoBehaviour
{
    public GameObject LabirintForm;
    public GameObject QuadFloor;
    public GameObject CellsFolder;

    public LabirintSiblingConnectionPoints ConnectionPoints;

    [NonSerialized] public LabirintView LabirintView;
    //public LabirintCellTypeMaterials CellTypeMaterials;


    private void Awake()
    {
        LabirintView = GetComponent<LabirintView>();

        ConnectionPoints.LeftPoint = Vector3.zero;
        ConnectionPoints.RightPoint = Vector3.zero;
        ConnectionPoints.TopPoint = Vector3.zero;
        ConnectionPoints.BottomPoint = Vector3.zero;
    }


    public void SetParams(int Width, int Height)
    {
        SetQuadSize(Width, Height);
        SetDynamicPositions(Width, Height);
    }

    private void SetQuadSize(int Width, int Height)
    {
        Vector3 scale;

        scale = QuadFloor.transform.localScale;
        scale.x = Width;
        scale.y = Height;
        QuadFloor.transform.localScale = scale;
    }

    private void SetDynamicPositions(int Width, int Height)
    {
        Vector3 position = LabirintForm.transform.position;
        position.x = -Width / 2;
        position.y = -Height / 2;
        LabirintForm.transform.position = transform.TransformPoint(position);


        ConnectionPoints.LeftPoint.x = -Width / 2;
        ConnectionPoints.LeftPoint.y = Height / 2;
        ConnectionPoints.LeftPoint = LabirintForm.transform.TransformPoint(ConnectionPoints.LeftPoint);

        ConnectionPoints.RightPoint.x = Width + Width / 2;
        ConnectionPoints.RightPoint.y = Height / 2;
        ConnectionPoints.RightPoint = LabirintForm.transform.TransformPoint(ConnectionPoints.RightPoint);

        ConnectionPoints.TopPoint.x = Width / 2;
        ConnectionPoints.TopPoint.y = Height + Height / 2;
        ConnectionPoints.TopPoint = LabirintForm.transform.TransformPoint(ConnectionPoints.TopPoint);

        ConnectionPoints.BottomPoint.x = Width / 2;
        ConnectionPoints.BottomPoint.y = -Height / 2;
        ConnectionPoints.BottomPoint = LabirintForm.transform.TransformPoint(ConnectionPoints.BottomPoint);
    }
}
