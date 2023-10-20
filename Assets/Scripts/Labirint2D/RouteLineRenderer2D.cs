using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RouteLineRenderer2D : MonoBehaviour
{

    public MazeSpawner2D MazeSpawner;

    private LineRenderer LineRenderer;

    // Start is called before the first frame update
    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    public void DrawRoute()
    {
        transform.position = new Vector3 (-MazeSpawner.Maze.Width/2, -MazeSpawner.Maze.Height / 2, -1);

        Vector2Int finishPosition = MazeSpawner.Maze.FinishPosition;
        List<Vector3> routePositions = new List<Vector3>();
        Vector2Int currentPosition = finishPosition;

        if (finishPosition.x == 0)
            routePositions.Add(convertToRealVector3(currentPosition + Vector2Int.left));
        else if(finishPosition.y == 0)
            routePositions.Add(convertToRealVector3(currentPosition + Vector2Int.down));
        else if (finishPosition.x == MazeSpawner.Maze.Width - 2)
            routePositions.Add(convertToRealVector3(currentPosition + Vector2Int.right));
        else
            routePositions.Add(convertToRealVector3(currentPosition + Vector2Int.up));

        routePositions.Add(convertToRealVector3(currentPosition));

        int currentDistance;
        while (currentPosition != Vector2Int.zero)
        {

            MazeCell2D currentCell = MazeSpawner.Maze.Cells[currentPosition.x][currentPosition.y];
            currentDistance = currentCell.DistanceFromStart;

            if (currentDistance <= 0)
                break;

            if (currentPosition.x > 0
                && MazeSpawner.Maze.Cells[currentPosition.x - 1][currentPosition.y].DistanceFromStart == currentDistance - 1
                && !currentCell.LeftWall)
            {
                currentPosition.x -= 1;
            }

            else if (currentPosition.y > 0
                && MazeSpawner.Maze.Cells[currentPosition.x][currentPosition.y - 1].DistanceFromStart == currentDistance - 1
                && !currentCell.BottomWall)
            {
                currentPosition.y -= 1;
            }

            else if (currentPosition.x < MazeSpawner.Maze.Width - 1
                && MazeSpawner.Maze.Cells[currentPosition.x + 1][currentPosition.y].DistanceFromStart == currentDistance - 1
                && !MazeSpawner.Maze.Cells[currentPosition.x + 1][currentPosition.y].LeftWall)
            {
                currentPosition.x += 1;
            }

            else if (currentPosition.y < MazeSpawner.Maze.Height - 1
                && MazeSpawner.Maze.Cells[currentPosition.x][currentPosition.y + 1].DistanceFromStart == currentDistance - 1
                && !MazeSpawner.Maze.Cells[currentPosition.x][currentPosition.y + 1].BottomWall)
            {
                currentPosition.y += 1;
            }

            else
                break;


            //currentDistance = MazeSpawner.Maze.Cells[currentPosition.x, currentPosition.y].DistanceFromStart;

            routePositions.Add(convertToRealVector3(currentPosition));
        }

        //Debug.Log(routePositions);
        LineRenderer.positionCount = routePositions.Count;
        LineRenderer.SetPositions(routePositions.ToArray());
    }

    private Vector3 convertToRealVector3(Vector2Int mazeCorrdinates)
    {
        float xReal = MazeSpawner.Maze.CellWidth * mazeCorrdinates.x + MazeSpawner.Maze.CellWidth / 2;
        float yReal = MazeSpawner.Maze.CellHeight * mazeCorrdinates.y + MazeSpawner.Maze.CellHeight / 2;
        return new Vector3(xReal, yReal, 0);
    }
}
