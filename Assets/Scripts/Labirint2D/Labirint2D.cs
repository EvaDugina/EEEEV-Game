using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labirint2D : MonoBehaviour
{
    public GameObject LabirintForm;
    public GameObject QuadFloor;
    public GameObject CellsFolder;

    public Vector3 LeftPoint = Vector3.zero;
    public Vector3 RightPoint = Vector3.zero;
    public Vector3 TopPoint = Vector3.zero;
    public Vector3 BottomPoint = Vector3.zero;


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


        LeftPoint.x = -Width / 2;
        LeftPoint.y = Height / 2;
        LeftPoint = LabirintForm.transform.TransformPoint(LeftPoint);

        RightPoint.x = Width + Width / 2;
        RightPoint.y = Height / 2;
        RightPoint = LabirintForm.transform.TransformPoint(RightPoint);

        TopPoint.x = Width / 2;
        TopPoint.y = Height + Height / 2;
        TopPoint = LabirintForm.transform.TransformPoint(TopPoint);

        BottomPoint.x = Width / 2;
        BottomPoint.y = -Height / 2;
        BottomPoint = LabirintForm.transform.TransformPoint(BottomPoint);
    }
}
