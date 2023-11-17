using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Level2D))]
public class LevelController : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private LevelSpawner LevelSpawner;
    [SerializeField] private AreasController AreasController;
    //public LabirintsSpawner2D LabirintsSpawner2D;

    [Header("Размеры лабиринта - целые, нечётные числа")]

    [Range(21, 99)]
    [SerializeField] private int Width;
    [Range(21, 99)]
    [SerializeField] private int Height;


    private Level Level;
    private Area CurrentArea;
    private Transform CurrenAreaTransform;
    private MazeCell CurrentMazeCell;

    private bool IsEnableToHorizintalTeleport = false;
    private bool IsEnableToVerticalTeleport = false;

    private void Awake()
    {

        // Проеряем лабиринт на чётность и если чётный - делаем нечётным
        GetComponent<LevelController>().Width -= (Width + 1) % 2;
        GetComponent<LevelController>().Height -= (Height + 1) % 2;

    }


    private void Start()
    {

        Vector3 cameraPosition = MainCamera.transform.position;
        cameraPosition.x = Width / 2;
        cameraPosition.y = Height / 2;
        MainCamera.transform.position = cameraPosition;

        // Генерируем уровень
        LevelGenerator levelGenerator = new(Width, Height, AreasController.GetLevelParameters());
        Level = levelGenerator.GenerateLevel();

        // Отрисовываем уровень
        LevelSpawner.SpawnLevel(transform.GetComponent<Level2D>(), Level, AreasController.GetLevelConfiguration());

        // Помещаем игрока в начальную клетку
        GameObject areaObject = RotateArea(Level.MainArea);
        TeleportPlayer(areaObject, Level.MainArea);
    }

    //Update is called once per frame
    private void LateUpdate()
    {

        Vector3 playerPosition = Player.transform.position;
        RefreshCurrentMazeCell(playerPosition);

        Vector2Int currentCellPosition = new Vector2Int(CurrentMazeCell.X, CurrentMazeCell.Y);
        //MazeCellType currentCellType = CurrentArea.GetCell(currentCellPosition).Type;
        Debug.Log(currentCellPosition);
        if (CurrentMazeCell.Type == MazeCellType.Portal)
        {
            // Определяем Area для телепортации
            Debug.Log("Клетка " + currentCellPosition + " - портал!");
            Portal portal = CurrentArea.GetPortalByPosition(currentCellPosition);
            Area destinationArea = Level.GetAreaById(portal.ToAreaId);

            // Поворачиваем Destination Area, если оно - не Main
            GameObject areaObject = RotateArea(destinationArea);

            TeleportPlayer(areaObject, destinationArea);
            return;
        }

        if (CurrentArea.Topology == AreaTopology.Toroid)
        {
            Vector3 newPlayerPosition = TranslatePlayer(playerPosition);
            if (newPlayerPosition != playerPosition)
                Player.transform.position = newPlayerPosition;
        }
    }

    private GameObject RotateArea(Area destinationArea) {
        GameObject areaObject;
        if (destinationArea.Type == AreaType.Main)
        {
            areaObject = transform.GetComponent<Level2D>().AreasFolder.transform.GetChild(destinationArea.Id).gameObject;
            return areaObject;
        }

        Vector2 vectorMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        areaObject = AreasController.RotateSecondaryAreaRelativelyPlayerMovement(destinationArea, vectorMovement);
        return areaObject;
    }

    private Vector3 TranslatePlayer(Vector3 playerPosition)
    {

        if (IsEnableToHorizintalTeleport)
        {
            if (2 <= Mathf.Abs(playerPosition.x) && Mathf.Abs(playerPosition.x) <= CurrentArea.MainMaze.Width - 2)
                IsEnableToHorizintalTeleport = false;
        }
        else if (playerPosition.x > CurrentArea.MainMaze.Width + 1)
        {
            playerPosition.x = Mathf.Abs(playerPosition.x) - CurrentArea.MainMaze.Width;
            IsEnableToHorizintalTeleport = true;
        }
        else if (playerPosition.x < -1)
        {
            playerPosition.x = CurrentArea.MainMaze.Width - Mathf.Abs(playerPosition.x);
            IsEnableToHorizintalTeleport = true;
        }


        if (IsEnableToVerticalTeleport)
        {
            if (2 <= Mathf.Abs(playerPosition.y) && Mathf.Abs(playerPosition.y) <= CurrentArea.MainMaze.Height - 2)
                IsEnableToVerticalTeleport = false;
        }
        else if (playerPosition.y > CurrentArea.MainMaze.Height + 1)
        {
            playerPosition.y = Mathf.Abs(playerPosition.y) - CurrentArea.MainMaze.Height;
            IsEnableToVerticalTeleport = true;
        }
        else if (playerPosition.y < -1)
        {
            playerPosition.y = CurrentArea.MainMaze.Height - Mathf.Abs(playerPosition.y);
            IsEnableToVerticalTeleport = true;
        }


        return playerPosition;
    }


    private void RefreshCurrentMazeCell(Vector3 playerPosition)
    {
        Vector2Int position = AreasController.ParseToAreaSizePosition(CurrentArea.Type, CurrenAreaTransform.TransformPoint(playerPosition));
        CurrentMazeCell = CurrentArea.GetCell(position);

    }


    public void TeleportPlayer(GameObject areaObject, Area destinationArea)
    {
        Area lastArea = CurrentArea;

        CurrentArea = Level.GetAreaById(destinationArea.Id);
        CurrenAreaTransform = areaObject.transform;

        Vector2Int areaPlayerPosition;
        if (CurrentArea.Type == AreaType.Main && lastArea != null)
        {
            Portal portal = CurrentArea.GetPortalByToAreaId(lastArea.Id);
            areaPlayerPosition = AreasController.GenerateOutPortal(portal.Position);
            if (areaPlayerPosition == Vector2Int.zero)
                areaPlayerPosition = CurrentArea.MainMaze.StartPosition;
        }
        else
            areaPlayerPosition = CurrentArea.MainMaze.StartPosition;

        Vector3Int cellSize = AreasController.GetCellSize(CurrentArea.Type);
        SetPlayerToCell(CurrentArea.GetCell(areaPlayerPosition), CurrentArea.ZIndex, cellSize.x, cellSize.y);
    }

    public void SetPlayerToCell(MazeCell mazeCell, int zIndex, float cellWidth, float cellHeight)
    {
        Vector3 playerPosition = Player.transform.position;

        playerPosition.x = mazeCell.X * cellWidth + cellWidth / 2;
        playerPosition.y = mazeCell.Y * cellHeight + cellHeight / 2;
        playerPosition.z = 0;

        Player.transform.position = CurrenAreaTransform.TransformPoint(playerPosition);

        CurrentMazeCell = CurrentArea.GetCell(new Vector2Int(mazeCell.X, mazeCell.Y));
    }


}
