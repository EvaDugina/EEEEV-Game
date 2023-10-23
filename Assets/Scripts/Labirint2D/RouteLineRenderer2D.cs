
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RouteLineRenderer2D : MonoBehaviour
{

    public LabirintsSpawner2D MazeSpawner;

    private Maze MainMaze;
    private LineRenderer LineRenderer;

    // Start is called before the first frame update
    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    public void DrawRoute(Maze mainMaze)
    {
        MainMaze = mainMaze;

        transform.position = new Vector3 (-MainMaze.Width/2, -MainMaze.Height / 2, -1);

        Vector2Int finishPosition = MainMaze.FinishPosition;
        List<Vector3> routePositions = new List<Vector3>();
        Vector2Int currentPosition = finishPosition;

        if (finishPosition.x == 0)
            routePositions.Add(convertToRealVector3(currentPosition + Vector2Int.left));
        else if(finishPosition.y == 0)
            routePositions.Add(convertToRealVector3(currentPosition + Vector2Int.down));
        else if (finishPosition.x == MainMaze.Width - 2)
            routePositions.Add(convertToRealVector3(currentPosition + Vector2Int.right));
        else
            routePositions.Add(convertToRealVector3(currentPosition + Vector2Int.up));

        routePositions.Add(convertToRealVector3(currentPosition));

        int currentDistance;
        while (currentPosition != Vector2Int.zero)
        {

            MazeCell currentCell = MainMaze.Cells[currentPosition.x][currentPosition.y];
            currentDistance = currentCell.DistanceFromStart;

            if (currentDistance <= 0)
                break;

            if (currentPosition.x > 0
                && MainMaze.Cells[currentPosition.x - 1][currentPosition.y].DistanceFromStart == currentDistance - 1
                && !currentCell.LeftWall)
            {
                currentPosition.x -= 1;
            }

            else if (currentPosition.y > 0
                && MainMaze.Cells[currentPosition.x][currentPosition.y - 1].DistanceFromStart == currentDistance - 1
                && !currentCell.BottomWall)
            {
                currentPosition.y -= 1;
            }

            else if (currentPosition.x < MainMaze.Width - 1
                && MainMaze.Cells[currentPosition.x + 1][currentPosition.y].DistanceFromStart == currentDistance - 1
                && !MainMaze.Cells[currentPosition.x + 1][currentPosition.y].LeftWall)
            {
                currentPosition.x += 1;
            }

            else if (currentPosition.y < MainMaze.Height - 1
                && MainMaze.Cells[currentPosition.x][currentPosition.y + 1].DistanceFromStart == currentDistance - 1
                && !MainMaze.Cells[currentPosition.x][currentPosition.y + 1].BottomWall)
            {
                currentPosition.y += 1;
            }

            else
                break;


            //currentDistance = MainMaze.Cells[currentPosition.x, currentPosition.y].DistanceFromStart;

            routePositions.Add(convertToRealVector3(currentPosition));
        }

        //Debug.Log(routePositions);
        LineRenderer.positionCount = routePositions.Count;
        LineRenderer.SetPositions(routePositions.ToArray());
    }

    private Vector3 convertToRealVector3(Vector2Int mazeCorrdinates)
    {
        float xReal = MainMaze.CellSize.x * mazeCorrdinates.x + MainMaze.CellSize.x / 2;
        float yReal = MainMaze.CellSize.y * mazeCorrdinates.y + MainMaze.CellSize.y / 2;
        return new Vector3(xReal, yReal, 0);
    }
}
