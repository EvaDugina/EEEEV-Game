using TMPro;
using UnityEngine;


public class LabirintsSpawner2D : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private GameObject LabirintPrefab;
    [SerializeField] private GameObject CellPrefab;

    [Space]
    [SerializeField] private RouteLineRenderer2D RouteLineRenderer;

    //[NonSerialized] public Maze2D Maze;

    public void SpawnLabirints(Level level)
    {
        SpawnLabirint(level.MainMaze);

        foreach (Maze maze in level.SecondaryMazes)
        {
            SpawnLabirint(maze);
        }
    }


    private void SpawnLabirint(Maze maze)
    {
        setCellSize(maze.CellSize);

        // Создаём структуру с лабиринтом
        GameObject labirintGameObject = Instantiate(LabirintPrefab, new Vector3(0, 0, 0),
                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        labirintGameObject.name = maze.GetMazeStructureTypeAsText() + "Labirint";

        Labirint2D labirint = labirintGameObject.GetComponent<Labirint2D>();
        labirint.SetParams(maze.Width, maze.Height);

        SpawnMaze(labirint.LabirintForm.transform, labirint.CellsFolder.transform, maze.Cells, maze.Info);
        if (maze.Info.StructureType == MazeStructureType.Main)
            SpawnBoundaryLabirints(labirint.ConnectionPoints, maze);
    }

    private void SpawnMaze(Transform labirintFormTransform, Transform cellsFolderTransform, MazeCell[][] mazeCells, MazeInfo info)
    {

        float cellWidth = CellPrefab.transform.localScale.x;
        float cellHeight = CellPrefab.transform.localScale.y;

        for (int x = 0; x < mazeCells.Length; x++)
        {
            for (int y = 0; y < mazeCells[x].Length; y++)
            {
                Cell2D cell = Instantiate(CellPrefab,
                    labirintFormTransform.TransformPoint(new Vector3(mazeCells[x][y].X * cellWidth, mazeCells[x][y].Y * cellHeight, 0)),
                    Quaternion.identity, cellsFolderTransform).GetComponent<Cell2D>();

                cell.LeftWall.SetActive(mazeCells[x][y].LeftWall);
                cell.BottomWall.SetActive(mazeCells[x][y].BottomWall);

                if (mazeCells[x][y].Type == MazeStructureType.Field)
                {
                    cell.Floor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cell.Field;
                    cell.Floor.SetActive(mazeCells[x][y].Floor);
                }
                else if (mazeCells[x][y].Type == MazeStructureType.Room)
                {

                    cell.Floor.SetActive(mazeCells[x][y].Floor);
                }

                //Отключение/включение колонны, когда рядом нет соседей
                ColumnType columnType = mazeCells[x][y].BottomLeftColumnType;
                if (columnType == ColumnType.Default)
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

    private void setCellSize(Vector2Int cellSize)
    {
        Vector2 cellScale = CellPrefab.transform.localScale;
        cellScale.x = cellSize.x;
        cellScale.y = cellSize.y;
        //cellScale.z = CellSize.z;
        CellPrefab.transform.localScale = cellScale;
    }

    //private void SpawnMainLabirint() {
    //    // Создаём структуру с лабиринтом
    //    GameObject labirintGameObject = Instantiate(LabirintPrefab, new Vector3(0, 0, 0),
    //                Quaternion.identity, GameObject.Find("Level/Labirints").transform);
    //    labirintGameObject.name = "MainLabirint";

    //    Labirint2D mainLabirint = labirintGameObject.GetComponent<Labirint2D>();
    //    mainLabirint.SetParams(Width, Height);

    //    SpawnMaze(mainLabirint.LabirintForm.transform, mainLabirint.CellsFolder.transform, Level.MainMaze.Cells, Level.MainMaze.Info);
    //    SpawnBoundaryLabirints(mainLabirint.ConnectionPoints);
    //}


    private void SpawnBoundaryLabirints(LabirintSiblingConnectionPoints connectionPoints, Maze maze)
    {
        SpawnBoundaryLabirint(connectionPoints, maze, "Left");
        SpawnBoundaryLabirint(connectionPoints, maze, "Right");
        SpawnBoundaryLabirint(connectionPoints, maze, "Top");
        SpawnBoundaryLabirint(connectionPoints, maze, "Bottom");

        SpawnBoundaryLabirint(connectionPoints, maze, "TopLeft");
        SpawnBoundaryLabirint(connectionPoints, maze, "TopRight");
        SpawnBoundaryLabirint(connectionPoints, maze, "BottomLeft");
        SpawnBoundaryLabirint(connectionPoints, maze, "BottomRight");

    }

    private void SpawnBoundaryLabirint(LabirintSiblingConnectionPoints connectionPoints, Maze maze, string side)
    {
        GameObject labirintGameObject;
        if (side == "Left")
            labirintGameObject = Instantiate(LabirintPrefab, connectionPoints.LeftPoint,
                                Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        else if (side == "Right")
            labirintGameObject = Instantiate(LabirintPrefab, connectionPoints.RightPoint,
                                Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        else if (side == "Top")
            labirintGameObject = Instantiate(LabirintPrefab, connectionPoints.TopPoint,
                                Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        else if (side == "Bottom")
            labirintGameObject = Instantiate(LabirintPrefab, connectionPoints.BottomPoint,
                                Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        else
        {
            if (side == "TopLeft")
                labirintGameObject = Instantiate(LabirintPrefab, connectionPoints.LeftPoint + connectionPoints.TopPoint,
                                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
            else if (side == "TopRight")
                labirintGameObject = Instantiate(LabirintPrefab, connectionPoints.RightPoint + connectionPoints.TopPoint,
                                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
            else if (side == "BottomLeft")
                labirintGameObject = Instantiate(LabirintPrefab, connectionPoints.LeftPoint + connectionPoints.BottomPoint,
                                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
            else
                labirintGameObject = Instantiate(LabirintPrefab, connectionPoints.RightPoint + connectionPoints.BottomPoint,
                                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        }

        labirintGameObject.name = side + "MainLabirint";

        Labirint2D labirint = labirintGameObject.GetComponent<Labirint2D>();
        labirint.SetParams(maze.Width, maze.Height);

        SpawnMaze(labirint.LabirintForm.transform, labirint.CellsFolder.transform, MazeUtilities.GetMazePartBySide(maze.Cells, side), maze.Info);
    }
}
