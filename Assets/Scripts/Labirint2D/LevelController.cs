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

    // Для перемещения раз в определённое время на ReflectedArea
    private float PeriodTeleportToReflectedArea = 10.0f;
    private float NextActionTime = 5.0f;

    private StaticPositionParameter PlayerMovingSideInMainMaze;

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

        //foreach (Portal portal in Level.MainArea.Portals)
        //    Debug.Log(portal.Position);

        // Отрисовываем уровень
        LevelSpawner.SpawnLevelWithOptimization(transform.GetComponent<Level2D>(), Level, AreasController.GetLevelConfiguration(), Level.MainArea, Level.MainArea.MainMaze.StartPosition);

        // Помещаем игрока в начальную клетку
        GameObject areaObject = RotateArea(Level.MainArea);
        TeleportPlayer(areaObject, Level.MainArea, StaticPositionParameter.None, false);
    }

    //Update is called once per frame
    private void LateUpdate()
    {

        Vector3 playerPosition = Player.transform.position;

        RefreshCurrentMazeCell(playerPosition);
        //Debug.Log(currentCellPosition);

        Vector2Int currentCellPosition = new Vector2Int(CurrentMazeCell.X, CurrentMazeCell.Y);
        if (CurrentMazeCell.Type == MazeCellType.Portal)
        {
            // Определяем Area для телепортации
            Debug.Log("Клетка " + currentCellPosition + " - портал!");
            Portal portal = CurrentArea.GetPortalByPosition(currentCellPosition);
            Area destinationArea = Level.GetAreaById(portal.ToAreaId);

            if (!destinationArea.Visited || destinationArea.Type == AreaType.Main || destinationArea.Type == AreaType.ReflectedMain)
            {

                // Поворачиваем Destination Area, если оно - не Main
                GameObject areaObject = RotateArea(destinationArea);

                StaticPositionParameter portalSide = CurrentArea.GetPortalSide(portal);
                portalSide = ChooseSideAfterReturningFromPortal(portalSide);

                TeleportPlayer(areaObject, destinationArea, portalSide, false);
                return;
            }
        }

        // пермещаем Player на ReflectedArea
        else if (Level.ReflectedArea != null && (CurrentArea.Type == AreaType.Main || CurrentArea.Type == AreaType.ReflectedMain))
        {
            if (Time.time > NextActionTime)
            {
                Debug.Log(Time.time + " ? " + NextActionTime + " + " + PeriodTeleportToReflectedArea + " = " + (NextActionTime + PeriodTeleportToReflectedArea));
                NextActionTime += PeriodTeleportToReflectedArea;
                TeleportPlayerToBetweenMainAndReflectedAreas();
            }

            // Перемещаем принудительно, если игрок находится на границе ReflectedMainMaze
            else if (CurrentArea.Type == AreaType.ReflectedMain && (CurrentMazeCell.X > CurrentArea.Width - 5 || CurrentMazeCell.Y > CurrentArea.Height - 5))
            {
                Debug.Log("Принудительное перемещение с ReflectedArea на MainArea");
                NextActionTime += PeriodTeleportToReflectedArea;
                TeleportPlayerToBetweenMainAndReflectedAreas();
            }

        }

        Vector3 newPlayerPosition = playerPosition;
        if (CurrentArea.Topology == AreaTopology.Toroid)
        {
            newPlayerPosition = TranslatePlayer(playerPosition);
            if (newPlayerPosition != playerPosition)
                Player.transform.position = newPlayerPosition;
        }

        MoveMiniMapCamera(newPlayerPosition);
    }

    public StaticPositionParameter ChooseSideAfterReturningFromPortal(StaticPositionParameter portalSide)
    {
        if (PlayerMovingSideInMainMaze == StaticPositionParameter.Top)
        {
            if (portalSide == StaticPositionParameter.Top)
                return StaticPositionParameter.Top;
            else if (portalSide == StaticPositionParameter.Bottom)
                return StaticPositionParameter.Bottom;
            else if (portalSide == StaticPositionParameter.Left)
                return StaticPositionParameter.Left;
            else if (portalSide == StaticPositionParameter.Right)
                return StaticPositionParameter.Right;
        }
        else if (PlayerMovingSideInMainMaze == StaticPositionParameter.Bottom)
        {
            if (portalSide == StaticPositionParameter.Top)
                return StaticPositionParameter.Bottom;
            else if (portalSide == StaticPositionParameter.Bottom)
                return StaticPositionParameter.Top;
            else if (portalSide == StaticPositionParameter.Left)
                return StaticPositionParameter.Right;
            else if (portalSide == StaticPositionParameter.Right)
                return StaticPositionParameter.Left;
        }
        else if (PlayerMovingSideInMainMaze == StaticPositionParameter.Left)
        {
            if (portalSide == StaticPositionParameter.Top)
                return StaticPositionParameter.Left;
            else if (portalSide == StaticPositionParameter.Bottom)
                return StaticPositionParameter.Right;
            else if (portalSide == StaticPositionParameter.Right)
                return StaticPositionParameter.Top;
            else if (portalSide == StaticPositionParameter.Left)
                return StaticPositionParameter.Bottom;
        }
        else
        {
            if (portalSide == StaticPositionParameter.Top)
                return StaticPositionParameter.Right;
            else if (portalSide == StaticPositionParameter.Bottom)
                return StaticPositionParameter.Left;
            else if (portalSide == StaticPositionParameter.Right)
                return StaticPositionParameter.Bottom;
            else if (portalSide == StaticPositionParameter.Left)
                return StaticPositionParameter.Top;
        }

        return PlayerMovingSideInMainMaze;
    }

    public void SetCameraPosition(Vector3 newPosition)
    {
        Vector3 cameraPosition = MiniMapCamera.transform.position;
        cameraPosition.x = newPosition.x;
        cameraPosition.y = newPosition.y;
        cameraPosition.z = newPosition.z;
        MiniMapCamera.transform.position = cameraPosition;
    }

    public void MoveMiniMapCamera(Vector3 targetPosition)
    {
        Vector3 cameraPosition = MiniMapCamera.transform.position;
        cameraPosition.x = targetPosition.x;
        cameraPosition.y = targetPosition.y + 5;
        cameraPosition.z = targetPosition.z;
        MiniMapCamera.transform.position = cameraPosition;
    }

    private GameObject RotateArea(Area destinationArea)
    {
        GameObject areaObject;

        // Для 2D:
        //if (destinationArea.Type == AreaType.Main || destinationArea.Type == AreaType.ReflectedMain)
        //{
        //    areaObject = transform.GetComponent<Level2D>().AreasFolder.transform.GetChild(destinationArea.Id).gameObject;
        //    return areaObject;
        //}

        //Vector2 vectorMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //areaObject = AreasController.RotateSecondaryAreaRelativelyPlayerMovement2D(destinationArea, vectorMovement);

        //Для 3D:
        areaObject = transform.GetComponent<Level2D>().AreasFolder.transform.GetChild(destinationArea.Id).gameObject;

        return areaObject;
    }

    private Vector3 TranslatePlayer(Vector3 playerPosition)
    {
        Vector3Int cellSize = AreasController.GetCellSize(CurrentArea.Type);

        if (playerPosition.x > (CurrentArea.MainMaze.Width + Random.Range(0, CurrentArea.MainMaze.Width / 4)) * cellSize.x)
        {
            playerPosition.x = Mathf.Abs(playerPosition.x) - CurrentArea.MainMaze.Width * cellSize.x;
        }
        else if (playerPosition.x < -1)
        {
            playerPosition.x = CurrentArea.MainMaze.Width * cellSize.x - Mathf.Abs(playerPosition.x);
        }

        if (playerPosition.z > (CurrentArea.MainMaze.Height + Random.Range(0, CurrentArea.MainMaze.Width / 4)) * cellSize.z)
        {
            playerPosition.z = Mathf.Abs(playerPosition.z) - CurrentArea.MainMaze.Height * cellSize.z;
        }
        else if (playerPosition.z < -1)
        {
            playerPosition.z = CurrentArea.MainMaze.Height * cellSize.z - Mathf.Abs(playerPosition.z);
        }


        return playerPosition;
    }


    private void RefreshCurrentMazeCell(Vector3 playerPosition)
    {
        Vector2Int position = AreasController.ParseToAreaSizePosition(CurrentArea.Type, CurrenAreaObject.transform.TransformPoint(playerPosition));

        MazeCell previousMazeCell = CurrentMazeCell;
        CurrentMazeCell = CurrentArea.GetCell(position);

        if (previousMazeCell != null && (CurrentArea.Type == AreaType.Main || CurrentArea.Type == AreaType.ReflectedMain))
        {
            if (CurrentArea.GetMazeByPosition(position).Type == MazeType.Main)
            {
                if (previousMazeCell.Y < CurrentMazeCell.Y)
                    PlayerMovingSideInMainMaze = StaticPositionParameter.Top;
                else if (previousMazeCell.Y > CurrentMazeCell.Y)
                    PlayerMovingSideInMainMaze = StaticPositionParameter.Bottom;
                else if (previousMazeCell.X < CurrentMazeCell.X)
                    PlayerMovingSideInMainMaze = StaticPositionParameter.Right;
                else if (previousMazeCell.X > CurrentMazeCell.X)
                    PlayerMovingSideInMainMaze = StaticPositionParameter.Left;
            }
        }

        if (previousMazeCell.X != CurrentMazeCell.X || previousMazeCell.Y != CurrentMazeCell.Y)
            LevelSpawner.SpawnLevelWithOptimization(transform.GetComponent<Level2D>(), Level, AreasController.GetLevelConfiguration(), CurrentArea, new Vector2Int(CurrentMazeCell.X, CurrentMazeCell.Y));

        Debug.Log(PlayerMovingSideInMainMaze + ": (" + CurrentMazeCell.X + ", " + CurrentMazeCell.Y + ")");

    }


    public void TeleportPlayer(GameObject areaObject, Area destinationArea, StaticPositionParameter portalParameter, bool reflectedTeleport)
    {
        Area lastArea = CurrentArea;
        bool isFirstSpawn = lastArea == null;

        CurrentArea = Level.GetAreaById(destinationArea.Id);
        CurrentMazeCell = null;

        if (CurrenAreaObject != null)
            CurrenAreaObject.SetActive(false);
        CurrenAreaObject = areaObject;
        areaObject.SetActive(true);

        CurrentArea.SetVisited(true);


        Vector2Int areaPlayerPosition;
        float eulerAnglesZ = 0f;
        if (!reflectedTeleport)
        {
            if (isFirstSpawn)
            {
                areaPlayerPosition = CurrentArea.MainMaze.StartPosition;
                eulerAnglesZ = 0f;
            }
            else if (CurrentArea.Type == AreaType.Main)
            {
                Portal portal = CurrentArea.GetPortalByToAreaId(lastArea.Id);
                areaPlayerPosition = AreasController.GenerateOutPortal(portal.Position, portalParameter);
                if (areaPlayerPosition == Vector2Int.zero)
                    areaPlayerPosition = CurrentArea.MainMaze.StartPosition;

                if (portalParameter == StaticPositionParameter.Top)
                    eulerAnglesZ = 0;
                else if (portalParameter == StaticPositionParameter.Bottom)
                    eulerAnglesZ = 180;
                else if (portalParameter == StaticPositionParameter.Right)
                    eulerAnglesZ = 90;
                else if (portalParameter == StaticPositionParameter.Left)
                    eulerAnglesZ = -90;
                else
                    eulerAnglesZ = Player.transform.localEulerAngles.z;
            }
            else
            {
                areaPlayerPosition = CurrentArea.MainMaze.StartPosition;
                eulerAnglesZ = 90f;
            }
        }

        // Для ReflectedArea:
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
        SetPlayerToCell(CurrentArea.GetCell(areaPlayerPosition), cellSize, reflectedTeleport);
        RotatePlayerToSecondaryArea(eulerAnglesZ);
    }

    private void TeleportPlayerToBetweenMainAndReflectedAreas()
    {
        Area destinationArea;
        if (CurrentArea.Type == AreaType.Main) destinationArea = Level.ReflectedArea;
        else destinationArea = Level.MainArea;

        // Не телепортируем на ReflectedArea, если игрок находится на граничной клетке
        if (CurrentArea.Type == AreaType.Main && (CurrentMazeCell.X > CurrentArea.Width - 10 || CurrentMazeCell.Y > CurrentArea.Height - 10))
        {
        }
        else
        {
            // Поворачиваем Destination Area, если оно - не Main и не ReflectedMain
            GameObject areaObject = RotateArea(destinationArea);
            TeleportPlayer(areaObject, destinationArea, StaticPositionParameter.None, true);
        }
    }

    public void SetPlayerToCell(MazeCell mazeCell, Vector3 cellSize, bool reflectedTeleport)
    {
        Vector3 playerPosition = Player.transform.position;

        float positionXInCell, positionZInCell;
        if (reflectedTeleport)
        {
            positionXInCell = (playerPosition.x - ((int)playerPosition.x)) % cellSize.x;
            positionZInCell = (playerPosition.z - ((int)playerPosition.z)) % cellSize.z;

            Vector3 rotation = Player.transform.localEulerAngles;
            rotation.x = 180 - rotation.x;
            rotation.y = 180 - rotation.y;
            rotation.z = 180 - rotation.z;

            Player.transform.localEulerAngles = rotation;

        }
        else
        {
            positionXInCell = cellSize.x / 2;
            positionZInCell = cellSize.z / 2;
        }


        playerPosition.x = mazeCell.X * cellSize.x + positionXInCell;

        // Для игры
        //playerPosition.y = Player.transform.localScale.y / 2;

        // Для тестирования
        playerPosition.y = 5;

        playerPosition.z = mazeCell.Y * cellSize.z + positionZInCell;

        Player.transform.position = CurrenAreaObject.transform.TransformPoint(playerPosition);
        SetCameraPosition(Player.transform.position);

        CurrentMazeCell = mazeCell;
    }

    public void RotatePlayerToSecondaryArea(float eulerAnglesZ)
    {
        Vector3 playerLocalEulerAngels = Player.transform.localEulerAngles;
        playerLocalEulerAngels.y = eulerAnglesZ;
        Player.transform.localEulerAngles = playerLocalEulerAngels;
    }


}
