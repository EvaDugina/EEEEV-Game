using System;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{

    [NonSerialized] public Maze Maze;

    [SerializeField] private GameObject CellPrefab;
    [SerializeField] private Vector3 CellSize = new Vector3(1, 1, 1);

    [SerializeField] private int Width = 23;
    [SerializeField] private int Height = 15;

    //[SerializeField] private float KDoubleWallSquare = 1f;
    //[SerializeField] private float KWallHeight = 0.75f;

    private void Awake()
    {
        setCellSize();
    }

    private void Start()
    {

        MazeGenerator mazeGenerator = new MazeGenerator(Width, Height);
        Maze = mazeGenerator.GenerateMaze();

        Transform parentCellsTransform = GameObject.Find("/Labirint/Cells").transform;

        float cellWidth = CellPrefab.transform.localScale.x;
        float cellHeight = CellPrefab.transform.localScale.z;

        for (int x = 0; x < Maze.Cells.GetLength(0); x++)
        {
            for (int z = 0; z < Maze.Cells.GetLength(1); z++)
            {
                Cell cell = Instantiate(CellPrefab,
                    new Vector3(Maze.Cells[x, z].X * cellWidth, 0, Maze.Cells[x, z].Z * cellHeight),
                    Quaternion.identity, parentCellsTransform).GetComponent<Cell>();

                cell.LeftWall.SetActive(Maze.Cells[x, z].LeftWall);
                cell.BottomWall.SetActive(Maze.Cells[x, z].BottomWall);
                cell.Floor.SetActive(Maze.Cells[x, z].Floor);

                /// Добавляем колонне другой материал, когда колонна - перекрёсток
                if (
                    (Maze.Cells[x, z].LeftWall && Maze.Cells[x, z].BottomWall) ||
                    (x > 0 && Maze.Cells[x - 1, z].BottomWall && Maze.Cells[x, z].LeftWall) ||
                    (z > 0 && Maze.Cells[x, z - 1].LeftWall && Maze.Cells[x, z].BottomWall) ||
                    (z > 0 && x > 0 && Maze.Cells[x-1, z].BottomWall && Maze.Cells[x, z-1].LeftWall)
                    )
                    cell.ColumnVisibility = Visibility.Enable;

                if (cell.ColumnVisibility == Visibility.Enable)
                    cell.Column.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cell.EnableColumnMaterial;
                else
                    cell.Column.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cell.DisableColumnMaterial;

                cell.XReal = Maze.Cells[x, z].X * CellSize.x;
                cell.ZReal = Maze.Cells[x, z].Z * CellSize.y;


            }
        }
    }

    private void setCellSize()
    {
        Vector3 cellScale = CellPrefab.transform.localScale;
        cellScale.x = CellSize.x;
        cellScale.y = CellSize.y;
        cellScale.z = CellSize.z;
        CellPrefab.transform.localScale = cellScale;
    }
}
