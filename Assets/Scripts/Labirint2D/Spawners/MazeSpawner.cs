using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MazeSpawner : MonoBehaviour
{
    public GameObject MazePrefab;
    public Maze2D Maze2D;

    public CellSpawner CellSpawner;

    private Transform MazesFolder;
    private Cell Cell; 

    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/

    public void Spawn(Transform mazeFioder, Cell cellTemplate, Maze mainMaze, List<Maze> boundaryMazes) {
        MazesFolder = mazeFioder;
        Cell = cellTemplate;

        SpawnMaze(mainMaze);

        foreach (Maze maze in boundaryMazes)
        {
            SpawnMaze(maze);
        }
    }


    public void SpawnMaze(Maze maze)
    {
        Vector2 mazePosition = maze.GetMazePositionInsideArea(maze.Side);

        // Ставим каркас Maze
        GameObject mazeObject = Instantiate(MazePrefab,
            MazesFolder.TransformPoint(new Vector3(mazePosition.x, mazePosition.y, 0)),
            Quaternion.identity, MazesFolder);
        mazeObject.name = maze.GetMazeSideAsText() + "Maze";

        Maze2D maze2D = mazeObject.GetComponent<Maze2D>();

        CellSpawner.Spawn(maze2D.CellsFolder.transform, Cell, maze.Cells);
    }


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Utilities
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/


}
