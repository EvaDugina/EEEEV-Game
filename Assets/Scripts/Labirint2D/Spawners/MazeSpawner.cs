using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MazeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject MazePrefab;
    [SerializeField] private CellSpawner CellSpawner;

    private Transform MazesFolder;
    private Cell Cell;
    private GameObject CellPrefab;

    private int MainMazeWidth;
    private int MainMazeHeight;

    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/

    public void Spawn(Transform mazeFolder, Cell cellTemplate, Maze mainMaze, List<Maze> boundaryMazes) {
        MazesFolder = mazeFolder;
        Cell = cellTemplate;

        CellPrefab = CellSpawner.GetResizedCellPrefab(Cell.CellPrefab, Cell.Width, Cell.Height, Cell.Length, MazesFolder);

        SpawnMaze(mainMaze);

        MainMazeWidth = mainMaze.Width;
        MainMazeHeight = mainMaze.Height;

        foreach (Maze maze in boundaryMazes)
        {
            SpawnMaze(maze);
        }
    }


    public void SpawnMaze(Maze maze)
    {
        Vector2 mazePosition;
        if (maze.Type == MazeType.Main)
            mazePosition = Vector2.zero;
        else
            mazePosition = MazeGenerateUtilities.GetBoundaryMazePositionInsideArea(MainMazeWidth, MainMazeHeight, maze.Side);

        mazePosition = new Vector2(mazePosition.x * Cell.Width, mazePosition.y * Cell.Length);

        // Ставим каркас Maze
        GameObject mazeObject = Instantiate(MazePrefab,
            MazesFolder.TransformPoint(new Vector3(mazePosition.x, 0, mazePosition.y)),
            Quaternion.identity, MazesFolder);
        mazeObject.name = maze.GetMazeSideAsText() + "Maze";

        Maze2D maze2D = mazeObject.GetComponent<Maze2D>();

        CellSpawner.Spawn(maze2D.CellsFolder.transform, Cell, CellPrefab, maze.Cells);
    }


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Utilities
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/


}
