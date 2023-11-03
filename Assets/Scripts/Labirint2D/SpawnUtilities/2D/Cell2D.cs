
using UnityEngine;

public struct WallsGameObjects
{
    public GameObject LeftWall;
    public GameObject TopWall;
    public GameObject RightWall;
    public GameObject BottomWall;
}

public struct ColumnsGameObjects
{
    public GameObject TopLeft;
    public GameObject TopRight;
    public GameObject BottomLeft;
    public GameObject BottomRight;
}

public class Cell2D : MonoBehaviour
{
    public WallsGameObjects Walls;
    public ColumnsGameObjects Columns;
    public GameObject Floor;
    public GameObject TextDistance;

}
