using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Level2D))]
public class LevelController : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private Camera MiniMapCamera;
    [SerializeField] private LevelSpawner LevelSpawner;
    [SerializeField] private AreasController AreasController;
    //public LabirintsSpawner2D LabirintsSpawner2D;

    [Header("Размеры лабиринта - целые, нечётные числа")]

    [Range(21, 99)]
    [SerializeField] private int Width;
    [Range(21, 99)]
    [SerializeField] private int Length;


    private Level Level;
    private Area CurrentArea;
    private GameObject CurrenAreaObject;
    private MazeCell CurrentMazeCell;

    //точность до милисекунды
    private float PeriodTeleportToReflectedArea = 60.0f;
    private float NextActionTime = 15.0f;

    private void Awake()
    {

        // Проеряем лабиринт на чётность и если чётный - делаем нечётным
        GetComponent<LevelController>().Width -= (Width + 1) % 2;
        GetComponent<LevelController>().Length -= (Length + 1) % 2;

    }


    private void Start()
    {
        SetCameraPosition(new Vector2(Width / 2, Length / 2));

        // Генерируем уровень
        LevelGenerator levelGenerator = new(Width, Length, AreasController.GetLevelParameters());
        Level = levelGenerator.GenerateLevel();

        // Отрисовываем уровень
        LevelSpawner.SpawnLevel(transform.GetComponent<Level2D>(), Level, AreasController.GetLevelConfiguration());

        //// Отдельно отрисовываем ReflectedArea
        //Area reflectedArea = levelGenerator.CreateReflectedArea(4, Level.MainArea);
        //Level.SetReflectedArea(reflectedArea);
        //LevelSpawner.SpawnReflectedArea(Level, AreasController.GetLevelConfiguration());

        // Помещаем игрока в начальную клетку
        GameObject areaObject = RotateArea(Level.MainArea);
        TeleportPlayer(areaObject, Level.MainArea, false);
    }

    //Update is called once per frame
    private void LateUpdate()
    {

        Vector3 playerPosition = Player.transform.position;
        RefreshCurrentMazeCell(playerPosition);

        Vector2Int currentCellPosition = new Vector2Int(CurrentMazeCell.X, CurrentMazeCell.Y);
        Debug.Log(currentCellPosition);
        if (CurrentMazeCell.Type == MazeCellType.Portal)
        {
            // Определяем Area для телепортации
            Debug.Log("Клетка " + currentCellPosition + " - портал!");
            Portal portal = CurrentArea.GetPortalByPosition(currentCellPosition);
            Area destinationArea = Level.GetAreaById(portal.ToAreaId);

            // Поворачиваем Destination Area, если оно - не Main
            GameObject areaObject = RotateArea(destinationArea);

            TeleportPlayer(areaObject, destinationArea, false);
            return;
        }

        // пермещаем Player на Reflected Area
        //else if (Time.time > NextActionTime)
        //{
        //    Debug.Log(Time.time + " ? " + NextActionTime + " + " + PeriodTeleportToReflectedArea + " = " + (NextActionTime + PeriodTeleportToReflectedArea));
        //    NextActionTime += PeriodTeleportToReflectedArea;

        //    if (CurrentArea.Type == AreaType.Main || CurrentArea.Type == AreaType.ReflectedMain)
        //    {

        //        Area destinationArea;
        //        if (CurrentArea.Type == AreaType.Main)
        //            destinationArea = Level.ReflectedArea;
        //        else
        //        {
        //            destinationArea = Level.MainArea;
        //        }

        //        // Поворачиваем Destination Area, если оно - не Main и не ReflectedMain
        //        GameObject areaObject = RotateArea(destinationArea);

        //        TeleportPlayer(areaObject, destinationArea, true);
        //    }
        //}

        Vector3 newPlayerPosition = playerPosition;
        if (CurrentArea.Topology == AreaTopology.Toroid)
        {
            newPlayerPosition = TranslatePlayer(playerPosition);
            if (newPlayerPosition != playerPosition)
                Player.transform.position = newPlayerPosition;
        }

        MoveCamera(newPlayerPosition);
    }

    public void SetCameraPosition(Vector3 newPosition) {
        Vector3 cameraPosition = MiniMapCamera.transform.position;
        cameraPosition.x = newPosition.x;
        cameraPosition.y = 10;
        cameraPosition.z = newPosition.z;
        MiniMapCamera.transform.position = cameraPosition;
    }

    public void MoveCamera(Vector3 targetPosition) {
        Vector3 cameraPosition = MiniMapCamera.transform.position;
        cameraPosition.x = targetPosition.x;
        cameraPosition.y = 10;
        cameraPosition.z = targetPosition.z;
        MiniMapCamera.transform.position = cameraPosition;
    }

    private GameObject RotateArea(Area destinationArea)
    {
        GameObject areaObject;
        if (destinationArea.Type == AreaType.Main || destinationArea.Type == AreaType.ReflectedMain)
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

        if (playerPosition.x > CurrentArea.MainMaze.Width + Random.Range(0, CurrentArea.MainMaze.Width / 4))
        {
            playerPosition.x = Mathf.Abs(playerPosition.x) - CurrentArea.MainMaze.Width;
            //IsEnableToHorizintalTeleport = true;
        }
        else if (playerPosition.x < -1)
        {
            playerPosition.x = CurrentArea.MainMaze.Width - Mathf.Abs(playerPosition.x);
            //IsEnableToHorizintalTeleport = true;
        }

        if (playerPosition.z > CurrentArea.MainMaze.Height + Random.Range(0, CurrentArea.MainMaze.Width / 4))
        {
            playerPosition.z = Mathf.Abs(playerPosition.z) - CurrentArea.MainMaze.Height;
            //IsEnableToVerticalTeleport = true;
        }
        else if (playerPosition.z < -1)
        {
            playerPosition.z = CurrentArea.MainMaze.Height - Mathf.Abs(playerPosition.z);
            //IsEnableToVerticalTeleport = true;
        }


        return playerPosition;
    }


    private void RefreshCurrentMazeCell(Vector3 playerPosition)
    {
        Vector2Int position = AreasController.ParseToAreaSizePosition(CurrentArea.Type, CurrenAreaObject.transform.TransformPoint(playerPosition));
        CurrentMazeCell = CurrentArea.GetCell(position);

    }


    public void TeleportPlayer(GameObject areaObject, Area destinationArea, bool reflectedTeleport)
    {
        Area lastArea = CurrentArea;
        bool isFirstSpawn = lastArea == null;

        CurrentArea = Level.GetAreaById(destinationArea.Id);

        if (CurrenAreaObject != null)
            CurrenAreaObject.SetActive(false);
        CurrenAreaObject = areaObject;
        areaObject.SetActive(true);

        Vector2Int areaPlayerPosition;
        if (!reflectedTeleport)
        {
            if (CurrentArea.Type == AreaType.Main && !isFirstSpawn)
            {
                Portal portal = CurrentArea.GetPortalByToAreaId(lastArea.Id);
                areaPlayerPosition = AreasController.GenerateOutPortal(portal.Position);
                if (areaPlayerPosition == Vector2Int.zero)
                    areaPlayerPosition = CurrentArea.MainMaze.StartPosition;
            }
            else
                areaPlayerPosition = CurrentArea.MainMaze.StartPosition;
        }
        else
        {
            if (CurrentMazeCell.Y > Length / 2)
            {
                int offset = (CurrentMazeCell.Y - 1) % (Length / 2);
                areaPlayerPosition = new Vector2Int(CurrentMazeCell.X, 0 + offset);
            }
            else if (CurrentMazeCell.Y < Length / 2)
            {
                int offset = CurrentMazeCell.Y % (Length / 2);
                areaPlayerPosition = new Vector2Int(CurrentMazeCell.X, Length / 2 + offset + 1);
            }
            else
                areaPlayerPosition = new Vector2Int(CurrentMazeCell.X, Length / 2);
        }

        Vector3Int cellSize = AreasController.GetCellSize(CurrentArea.Type);
        SetPlayerToCell(CurrentArea.GetCell(areaPlayerPosition), cellSize.x, cellSize.y, isFirstSpawn);
    }

    public void SetPlayerToCell(MazeCell mazeCell, float cellWidth, float cellHeight, bool isFirstSpawn)
    {
        Vector3 playerPosition = Player.transform.position;

        float positionXInCell, positionYInCell;
        if (!isFirstSpawn)
        {
            positionXInCell = (playerPosition.x - ((int)playerPosition.x)) % cellWidth;
            positionYInCell = (playerPosition.z - ((int)playerPosition.z)) % cellHeight;
        }
        else {
            positionXInCell = cellWidth / 2;
            positionYInCell = cellHeight / 2;
        }

        
        playerPosition.x = mazeCell.X * cellWidth + positionXInCell;

        // Для игры
        //playerPosition.y = Player.transform.localScale.y / 2;

        // Для тестирования
        //playerPosition.y = playerPosition.y;

        playerPosition.z = mazeCell.Y * cellHeight + positionYInCell;

        SetCameraPosition(playerPosition);
        Player.transform.position = CurrenAreaObject.transform.TransformPoint(playerPosition);

        CurrentMazeCell = mazeCell;
    }


}
