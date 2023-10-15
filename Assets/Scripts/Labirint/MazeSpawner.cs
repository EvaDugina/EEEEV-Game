using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CellPrefab;

    [SerializeField] private float KDoubleWallSquare = 1f;
    [SerializeField] private float KWallHeight = 0.75f;

    private void Awake()
    {
        setCellSize();
    }

    private void Start()
    {

        MazeGenerator mazeGenerator = new MazeGenerator();
        MazeCell[,] maze = mazeGenerator.GenerateMaze();

        Transform parentCellsTransform = GameObject.Find("/Labirint/Cells").transform;

        float cellWidth = CellPrefab.transform.localScale.x;
        float cellHeight = CellPrefab.transform.localScale.z;

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int z = 0; z < maze.GetLength(1); z++)
            {
                Cell cell = Instantiate(CellPrefab, 
                    new Vector3(maze[x, z].X * cellWidth, 0, maze[x, z].Z * cellHeight), 
                    Quaternion.identity, parentCellsTransform).GetComponent<Cell>();

                cell.LeftWall.SetActive(maze[x,z].LeftWall);
                cell.BottomWall.SetActive(maze[x,z].BottomWall);

                cell.XReal = maze[x, z].X * KDoubleWallSquare;
                cell.ZReal = maze[x, z].Z * KDoubleWallSquare;


            }
        }
    }

    private void setCellSize()
    {
        Vector3 cellScale = CellPrefab.transform.localScale;
        cellScale.x = KDoubleWallSquare;
        cellScale.y = KWallHeight;
        cellScale.z = KDoubleWallSquare;
        CellPrefab.transform.localScale = cellScale;
    }
}
