using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class MazeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject MazePrefab;
    [SerializeField] private CellSpawner CellSpawner;
    [SerializeField] private int SpawnBlockRadius;

    private Transform MazesFolder;
    private Cell Cell;
    private GameObject CellPrefab;

    private int MainMazeWidth;
    private int MainMazeHeight;

    Dictionary<int, GameObject[][]> _spawnedCellObjects = null;


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/

    public void Spawn(Transform mazeFolder, Cell cellTemplate, Maze mainMaze, List<Maze> boundaryMazes)
    {
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


    public GameObject SpawnWithOptimization(Transform mazeFolder, Cell cellTemplate, Maze mainMaze, List<Maze> boundaryMazes, Vector2Int playerCellPosition, GameObject mazeObject)
    {
        MazesFolder = mazeFolder;
        Cell = cellTemplate;


        if (_spawnedCellObjects == null)
        {
            _spawnedCellObjects = new Dictionary<int, GameObject[][]>();
            CellPrefab = CellSpawner.GetResizedCellPrefab(Cell.CellPrefab, Cell.Width, Cell.Height, Cell.Length, MazesFolder);
        }

        if (!IsSpawnedMazeCellsObjectsByCell(mainMaze))
        {
            GameObject[][] mazeCells = new GameObject[mainMaze.Width][];
            for (int x = 0; x < mainMaze.Width; x++)
            {
                mazeCells[x] = new GameObject[mainMaze.Height];
                for (int y = 0; y < mainMaze.Height; y++)
                    mazeCells[x][y] = null;
            }
            _spawnedCellObjects.Add(mainMaze.Id, mazeCells);
            Debug.Log("NEW MAZE CELLS!");
        }

        Debug.Log("SPAWN MAZE CELLS!");
        mazeObject = SpawnMazeWithOptimization(mainMaze, playerCellPosition, mazeObject);


        //if (playerCellPosition.x < mainMaze.Width && playerCellPosition.y < mainMaze.Height)

        //MainMazeWidth = mainMaze.Width;
        //MainMazeHeight = mainMaze.Height;

        //foreach (Maze maze in boundaryMazes)
        //{
        //    SpawnMaze(maze);
        //}

        return mazeObject;
    }

    public GameObject SpawnMazeWithOptimization(Maze maze, Vector2Int playerCellPosition, GameObject mazeObject)
    {
        if (mazeObject == null)
        {
            Vector2 mazePosition;
            if (maze.Type == MazeType.Main)
                mazePosition = Vector2.zero;
            else
                mazePosition = MazeGenerateUtilities.GetBoundaryMazePositionInsideArea(MainMazeWidth, MainMazeHeight, maze.Side);
            mazePosition = new Vector2(mazePosition.x * Cell.Width, mazePosition.y * Cell.Length);


            // Ставим каркас Maze
            mazeObject = Instantiate(MazePrefab,
                MazesFolder.TransformPoint(new Vector3(mazePosition.x, 0, mazePosition.y)),
                Quaternion.identity, MazesFolder);
            mazeObject.name = maze.GetMazeSideAsText() + "Maze";
        }

        Maze2D maze2D = mazeObject.GetComponent<Maze2D>();

        GameObject[][] newSpawnedCellObjects = CellSpawner.SpawnWithOptimization(maze2D.CellsFolder.transform, Cell, CellPrefab, maze.Cells, playerCellPosition, SpawnBlockRadius, _spawnedCellObjects[maze.Id]);
        for (int x = 0; x < maze.Width; x++)
        {
            for (int y = 0; y < maze.Height; y++)
            {
                if (_spawnedCellObjects[maze.Id][x][y] != null && newSpawnedCellObjects[x][y] == null)
                {
                    Destroy(_spawnedCellObjects[maze.Id][x][y]);
                    _spawnedCellObjects[maze.Id][x][y] = null;
                }
            }
        }
        _spawnedCellObjects[maze.Id] = newSpawnedCellObjects;

        return mazeObject;

    }

    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Utilities
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */

    private bool IsSpawnedMazeCellsObjectsByCell(Maze maze)
    {
        if (_spawnedCellObjects == null || !_spawnedCellObjects.ContainsKey(maze.Id))
            return false;
        return true;

        //foreach (KeyValuePair<int, GameObject[][]> entry in _spawnedCellObjects)
        //    if (entry.Key == maze.Id)
        //        return true;
        //return false;
    }

    private bool IsSpawnedCellObjectByCell(Maze maze, MazeCell cell)
    {

        foreach (KeyValuePair<int, GameObject[][]> entry in _spawnedCellObjects)
            if (entry.Key == maze.Id && entry.Value[cell.X][cell.Y] != null)
                return true;
        return false;
    }


}
