using System;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MazeSpawner2D : MonoBehaviour
{

    [SerializeField] private GameObject LabirintPrefab;
    [SerializeField] private GameObject CellPrefab;

    [Header("Размеры клетки")]
    [SerializeField] private Vector2 CellSize = new Vector2(1, 1);

    [Header("Размеры лабиринта - чётные целые числа")]
    [SerializeField] private int Width = 100;
    [SerializeField] private int Height = 100;

    [SerializeField] private RouteLineRenderer2D RouteLineRenderer;


    [NonSerialized] public Labirint2D MainLabirint;
    [NonSerialized] public Maze2D Maze;


    private void Awake()
    {
        setCellSize();
    }

    private void Start()
    {

        // Создаём структуру с лабиринтом
        GameObject labirintGameObject = Instantiate(LabirintPrefab, new Vector3(0, 0, 0),
                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        labirintGameObject.name = "MainLabirint";

        MainLabirint = labirintGameObject.GetComponent<Labirint2D>();
        MainLabirint.SetParams(Width, Height);


        // Создаём наполнение лабиринта
        MazeGenerator2D mazeGenerator = new MazeGenerator2D(Width, Height);
        Maze = mazeGenerator.GenerateMaze(CellSize);

        SpawnLabirint(MainLabirint, Maze.Cells);

        RouteLineRenderer.DrawRoute();

        SpawnBoundaryLabirints();
    }

    private void SpawnLabirint(Labirint2D labirint, MazeCell2D[][] mazeCells)
    {
        Transform parentCellsTransform = labirint.CellsFolder.transform;

        float cellWidth = CellPrefab.transform.localScale.x;
        float cellHeight = CellPrefab.transform.localScale.y;

        for (int x = 0; x < mazeCells.Length; x++)
        {
            for (int y = 0; y < mazeCells[x].Length; y++)
            {
                Cell2D cell = Instantiate(CellPrefab,
                    labirint.LabirintForm.transform.TransformPoint(new Vector3(mazeCells[x][y].X * cellWidth, mazeCells[x][y].Y * cellHeight, 0)),
                    Quaternion.identity, parentCellsTransform).GetComponent<Cell2D>();

                cell.LeftWall.SetActive(mazeCells[x][y].LeftWall);
                cell.BottomWall.SetActive(mazeCells[x][y].BottomWall);

                if (mazeCells[x][y].Type == CellType.Field)
                {
                    mazeCells[x][y].Floor.transform.GetChild(0).gameObject
                    cell.Floor.SetActive(mazeCells[x][y].Floor);
                }
                else if (mazeCells[x][y].Type == CellType.Room)
                {

                    cell.Floor.SetActive(mazeCells[x][y].Floor);
                }

                //Отключение/включение колонны, когда рядом нет соседей
                ColumnType columnType = mazeCells[x][y].BottomLeftColumnType;
                if (columnType == ColumnType.Nothing)
                    cell.Column.transform.GetChild(0).gameObject.SetActive(false);
                else
                {
                    /// Добавляем колонне другой материал, когда колонна - перекрёсток
                    if (columnType == ColumnType.Crossroad)
                        cell.Column.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cell.EnableColumnMaterial;
                    else
                        cell.Column.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cell.DisableColumnMaterial;
                }



                cell.TextDistance.GetComponent<TextMeshPro>().text = mazeCells[x][y].DistanceFromStart.ToString();

            }
        }
    }

    private void setCellSize()
    {
        Vector2 cellScale = CellPrefab.transform.localScale;
        cellScale.x = CellSize.x;
        cellScale.y = CellSize.y;
        //cellScale.z = CellSize.z;
        CellPrefab.transform.localScale = cellScale;
    }

    private void SpawnBoundaryLabirints()
    {
        SpawnBoundaryLabirint("Left");
        SpawnBoundaryLabirint("Right");
        SpawnBoundaryLabirint("Top");
        SpawnBoundaryLabirint("Bottom");

        SpawnBoundaryLabirint("TopLeft");
        SpawnBoundaryLabirint("TopRight");
        SpawnBoundaryLabirint("BottomLeft");
        SpawnBoundaryLabirint("BottomRight");

    }

    private void SpawnBoundaryLabirint(string side)
    {
        GameObject labirintGameObject;
        if (side == "Left")
            labirintGameObject = Instantiate(LabirintPrefab, MainLabirint.LeftPoint,
                                Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        else if (side == "Right")
            labirintGameObject = Instantiate(LabirintPrefab, MainLabirint.RightPoint,
                                Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        else if (side == "Top")
            labirintGameObject = Instantiate(LabirintPrefab, MainLabirint.TopPoint,
                                Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        else if (side == "Bottom")
            labirintGameObject = Instantiate(LabirintPrefab, MainLabirint.BottomPoint,
                                Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        else
        {
            if (side == "TopLeft")
                labirintGameObject = Instantiate(LabirintPrefab, MainLabirint.LeftPoint + MainLabirint.TopPoint,
                                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
            else if (side == "TopRight")
                labirintGameObject = Instantiate(LabirintPrefab, MainLabirint.RightPoint + MainLabirint.TopPoint,
                                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
            else if (side == "BottomLeft")
                labirintGameObject = Instantiate(LabirintPrefab, MainLabirint.LeftPoint + MainLabirint.BottomPoint,
                                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
            else
                labirintGameObject = Instantiate(LabirintPrefab, MainLabirint.RightPoint + MainLabirint.BottomPoint,
                                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        }

        labirintGameObject.name = side + "Labirint";

        Labirint2D labirint = labirintGameObject.GetComponent<Labirint2D>();
        labirint.SetParams(Width, Height);

        SpawnLabirint(labirint, Maze.GetMazePartBySide(Maze.Cells, side));
    }
}
