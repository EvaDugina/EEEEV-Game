using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteLineRenderer : MonoBehaviour
{

    public MazeSpawner MazeSpawner;

    private LineRenderer LineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void DrawRoute()
    {
        Vector2Int finishPosition = MazeSpawner.Maze.FinishPosition;
        List<Vector2Int> routePositions = new List<Vector2Int>();

        while (currentPosition != Vector2Int.zero) { 
        }
    }
}
