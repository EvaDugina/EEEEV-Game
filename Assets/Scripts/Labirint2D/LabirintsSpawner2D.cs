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
        SpawnBoundaryLabirints(level.MainMaze);

        foreach (Maze maze in level.SecondaryMazes)
        {
            SpawnLabirint(maze);
        }
    }


    private void SpawnLabirint(Maze maze)
    {
        setCellSize(maze.CellSize);

        // Создаём структуру с лабиринтом
        GameObject labirintGameObject = Instantiate(LabirintPrefab, new Vector3(maze.X, maze.Y, 0),
                    Quaternion.identity, GameObject.Find("Level/Labirints").transform);
        labirintGameObject.name = maze.Id + "_" + maze.GetMazeStructureTypeAsText() + "Labirint";

        Labirint2D labirint = labirintGameObject.GetComponent<Labirint2D>();

        SpawnMaze(labirint.LabirintForm.transform, labirint.CellsFolder.transform, labirint.LabirintView, maze);
    }

    private void SpawnBoundaryLabirints(Maze maze)
    {
        foreach (Maze boundaryMaze in maze.BoundaryMazes)
        {
            SpawnLabirint(boundaryMaze);
        }

    }


    private void SpawnMaze(Transform labirintFormTransform, Transform cellsFolderTransform, LabirintView labirintView, Maze maze)
    {

        float cellWidth = CellPrefab.transform.localScale.x;
        float cellHeight = CellPrefab.transform.localScale.y;

        for (int x = 0; x < maze.Cells.Length; x++)
        {
            for (int y = 0; y < maze.Cells[x].Length; y++)
            {
                Cell2D cell = Instantiate(CellPrefab,
                    labirintFormTransform.TransformPoint(new Vector3(maze.Cells[x][y].X * cellWidth, maze.Cells[x][y].Y * cellHeight, maze.ZIndex)),
                    Quaternion.identity, cellsFolderTransform).GetComponent<Cell2D>();

                cell.LeftWall.SetActive(maze.Cells[x][y].LeftWall);
                cell.BottomWall.SetActive(maze.Cells[x][y].BottomWall);

                // Выбираем материал пола
                Material floorMaterial = GetFloorMaterial(maze.Cells[x][y], labirintView);
                cell.Floor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = floorMaterial;
                cell.Floor.SetActive(maze.Cells[x][y].Floor);

                //Отключение/включение колонны, когда рядом нет соседей
                ColumnType columnType = maze.Cells[x][y].BottomLeftColumnType;
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



                cell.TextDistance.GetComponent<TextMeshPro>().text = maze.Cells[x][y].DistanceFromStart.ToString();

            }
        }
    }

    private void setCellSize(Vector2Int cellSize)
    {
        Vector3 cellScale = CellPrefab.transform.localScale;
        cellScale.x = cellSize.x;
        cellScale.y = cellSize.y;
        //cellScale.z = CellSize.z;
        CellPrefab.transform.localScale = cellScale;
    }

    private Material GetFloorMaterial(MazeCell mazeCell, LabirintView labirintView) {
        if (mazeCell.Type == CellType.Start)
            return labirintView.Start;
        else if (mazeCell.Type == CellType.Finish)
            return labirintView.Finish;
        else if (mazeCell.Type == CellType.Portal)
            return labirintView.Portal;
        else
        {
            if (mazeCell.FloorType == CellFloorType.Wheat)
                return labirintView.Field;
            else 
                return labirintView.Default;
        }
    }

}
