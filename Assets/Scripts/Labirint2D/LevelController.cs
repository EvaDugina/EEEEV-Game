using System;
using UnityEngine;

[RequireComponent(typeof(Level2D))]
public class LevelController : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private LevelSpawner LevelSpawner;
    //public LabirintsSpawner2D LabirintsSpawner2D;

    [Header("Размеры лабиринта - целые, нечётные числа")]

    [Range(21, 99)]
    [SerializeField] private int Width;
    [Range(21, 99)]
    [SerializeField] private int Height;

    //[Header("Параметры лабиринтов")]
    // TODO: Сделать через массив
    //public List<AreaParams> AreaParams = new();

    [Header("Параметры ROOM-лабиринта")]
    [SerializeField] private Parameters RoomAreaParams;

    [Header("Параметры FIELD-лабиринта")]
    [SerializeField] private Parameters FieldAreaParams;

    [Header("Параметры CORRIDOR-лабиринта")]
    [SerializeField] private Parameters CorridorAreaParams;


    private Level Level;
    private LevelConfiguration LevelConfiguration;

    private bool flagX = false;
    private bool flagY = false;

    private void Awake()
    {

        // Проеряем лабиринт на чётность и если чётный - делаем нечётным
        GetComponent<LevelController>().Width -= (Width + 1) % 2;
        GetComponent<LevelController>().Height -= (Height + 1) % 2;


        // Заполняем конфигурацию
        RoomAreaParams.Type = AreaType.Room;
        FieldAreaParams.Type = AreaType.Field;
        CorridorAreaParams.Type = AreaType.Corridor;

        LevelConfiguration = new LevelConfiguration();
        LevelConfiguration.AddAreaParamsToList(RoomAreaParams);
        LevelConfiguration.AddAreaParamsToList(FieldAreaParams);
        LevelConfiguration.AddAreaParamsToList(CorridorAreaParams);
    }


    private void Start()
    {

        // Генерируем уровень
        LevelGenerator levelGenerator = new(Width, Height, LevelConfiguration.GetParametersList());
        Level level = levelGenerator.GenerateLevel();

        // Отрисовываем уровень
        LevelSpawner.SpawnLevel(transform.GetComponent<Level2D>(), level, LevelConfiguration);

        // Устанавливаем радиус
        Width /= 2;
        Height /= 2;

        // Помещаем игрока в начальную клетку
        Vector3Int cellSize = LevelConfiguration.GetParametersList()[0].SpawnParams.CellParameters.Size;
        SetPlayerToCell(level.MainArea.MainMaze.Cells[0][0], level.MainArea.ZIndex, cellSize.x, cellSize.y);

    }

    //Update is called once per frame
    private void LateUpdate()
    {
        Vector3 playerPosition = Player.transform.position;


        int signX = (int)Mathf.Sign(playerPosition.x);
        int ceilPositiveX = (int)playerPosition.x * signX;

        if (flagX)
        {
            if (ceilPositiveX != Width - 2)
                flagX = false;
        }
        else if (ceilPositiveX > Width + 1)
        {
            playerPosition.x = -1 * signX * (Width - ceilPositiveX % Width - playerPosition.x * signX + ceilPositiveX);
            flagX = true;
        }


        int signY = (int)Mathf.Sign(playerPosition.y);
        int ceilPositiveY = (int)playerPosition.y * signY;

        if (flagY)
        {
            if (ceilPositiveY != Height - 2)
                flagY = false;
        }
        else if (ceilPositiveY > Height + 1)
        {
            playerPosition.y = -1 * signY * (Height - ceilPositiveY % Height - playerPosition.y * signY + ceilPositiveY);
            flagY = true;
        }

        //if (flagY && randomResidueY != -1 &&  ceilPositiveY % Height > 6 || ceilPositiveY % Height < 4)
        //{
        //    flagY = false;
        //}
        //else if (ceilPositiveX > Height)
        //{
        //    int residueY = Random.Range(0, 3);
        //    if (ceilPositiveY != residueY && ceilPositiveY % Height == residueY)
        //    {
        //        flagY = true;
        //        playerPosition.y *= -1;
        //        randomResidueY = residueY;
        //    }
        //}


        Player.transform.position = playerPosition;
    }

    public void SetPlayerToCell(MazeCell mazeCell, int zIndex, float cellWidth, float cellHeight)
    {
        Vector3 playerPosition = Player.transform.position;

        playerPosition.x = mazeCell.X * cellWidth + cellWidth / 2;
        playerPosition.y = mazeCell.Y * cellHeight + cellHeight / 2;
        playerPosition.z = zIndex;

        Player.transform.position = playerPosition;
    }
}
