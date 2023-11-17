using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AreasController : MonoBehaviour
{

    //[Header("Параметры лабиринтов")]
    // TODO: Сделать через массив
    //public List<AreaParams> AreaParams = new();

    [Header("Параметры ROOM-лабиринта")]
    [SerializeField] private Parameters RoomAreaParams;

    [Header("Параметры FIELD-лабиринта")]
    [SerializeField] private Parameters FieldAreaParams;

    [Header("Параметры CORRIDOR-лабиринта")]
    [SerializeField] private Parameters CorridorAreaParams;

    [SerializeField] private LevelConfiguration LevelConfiguration;

    private void Awake()
    {


        // Заполняем конфигурацию
        RoomAreaParams.Type = AreaType.Room;
        FieldAreaParams.Type = AreaType.Field;
        CorridorAreaParams.Type = AreaType.Corridor;

        LevelConfiguration = new LevelConfiguration();
        LevelConfiguration.AddAreaParamsToList(RoomAreaParams);
        LevelConfiguration.AddAreaParamsToList(FieldAreaParams);
        LevelConfiguration.AddAreaParamsToList(CorridorAreaParams);
    }

    public Vector3Int GetCellSize(AreaType areaType)
    {
        return LevelConfiguration.GetParametersByAreaType(areaType).SpawnParams.CellParameters.Size;
    }

    public List<Parameters> GetLevelParameters()
    {
        return LevelConfiguration.GetParametersList();
    }

    public LevelConfiguration GetLevelConfiguration()
    {
        return LevelConfiguration;
    }



    public GameObject RotateSecondaryAreaRelativelyPlayerMovement(Area destinationArea, Vector2 vectorMovement)
    {
        // Находим нужный нам Area, на который осущетсвляется переход
        GameObject areaObject = transform.GetComponent<Level2D>().AreasFolder.transform.GetChild(destinationArea.Id).gameObject;

        // Если топология - тор, не вращаем
        if (destinationArea.Topology == AreaTopology.Toroid)
            return areaObject;

        // Находим точку, вокруг которой будем вращать Area
        Vector2 pointAroundRotation = TranslateAreaCoordinatesToRealCoordinates(destinationArea.Type, destinationArea.MainMaze.StartPosition);

        Vector3 areaPosition = areaObject.transform.position;
        areaPosition = new Vector3(pointAroundRotation.x, pointAroundRotation.y, areaPosition.z);
        areaObject.transform.position = areaPosition;

        // Поворачиваем его, если надо
        Vector3 areaLocalEulerAngels = areaObject.transform.localEulerAngles;
        if (vectorMovement.y > 0) areaLocalEulerAngels.z = 90;
        else if (vectorMovement.y < 0) areaLocalEulerAngels.z = -90;
        else if (vectorMovement.x < 0) areaLocalEulerAngels.z = 180;
        else areaLocalEulerAngels.z = 0;
        areaObject.transform.localEulerAngles = areaLocalEulerAngels;

        // Меняем позицию после поворота
        areaPosition = areaObject.transform.position;
        Vector3Int cellSize = LevelConfiguration.GetParametersByAreaType(destinationArea.Type).SpawnParams.CellParameters.Size;
        if (vectorMovement.x > 0)
            areaObject.transform.position = new Vector3(areaPosition.x - pointAroundRotation.x, areaPosition.y - pointAroundRotation.y, areaPosition.z);
        else if (vectorMovement.x < 0)
            areaObject.transform.position = new Vector3(areaPosition.x + pointAroundRotation.x, areaPosition.y + pointAroundRotation.y + cellSize.y, areaPosition.z);
        else if (vectorMovement.y > 0)
            areaObject.transform.position = new Vector3(areaPosition.x + pointAroundRotation.x, areaPosition.y + pointAroundRotation.y + cellSize.y, areaPosition.z);
        else if (vectorMovement.y < 0)
            areaObject.transform.position = new Vector3(0, areaPosition.y - pointAroundRotation.y, areaPosition.z);

        return areaObject;
    }

    public Vector2Int GenerateOutPortal(/*Area currentArea, */Vector2Int portalPosition)
    {
        //MazeCell mazeCell = currentArea.GetCell(portalPosition);
        if (/*!mazeCell.WallsStatus.LeftWall &&*/ Input.GetAxis("Horizontal") < 0)
            return new Vector2Int(portalPosition.x - 1, portalPosition.y);

        else if (/*!mazeCell.WallsStatus.RightWall &&*/ Input.GetAxis("Horizontal") > 0)
            return new Vector2Int(portalPosition.x + 1, portalPosition.y);

        else if (/*!mazeCell.WallsStatus.TopWall &&*/ Input.GetAxis("Vertical") > 0)
            return new Vector2Int(portalPosition.x, portalPosition.y + 1);

        else if (/*!mazeCell.WallsStatus.BottomWall &&*/ Input.GetAxis("Vertical") < 0)
            return new Vector2Int(portalPosition.x, portalPosition.y - 1);

        else
            return Vector2Int.zero;

    }

    private Vector2 TranslateAreaCoordinatesToRealCoordinates(AreaType areaType, Vector2Int point)
    {
        Vector3Int cellSize = LevelConfiguration.GetParametersByAreaType(areaType).SpawnParams.CellParameters.Size;

        Vector2 realPoint = new Vector2Int(point.x, point.y);
        realPoint.x *= cellSize.x;
        realPoint.y *= cellSize.y;

        return realPoint;
    }


    public Vector2Int ParseToAreaSizePosition(AreaType areaType, Vector3 playerPosition)
    {
        Vector3Int cellSize = LevelConfiguration.GetParametersByAreaType(areaType).SpawnParams.CellParameters.Size;
        Vector2Int currentPositionInArea = new Vector2Int((int)(playerPosition.x / cellSize.x), (int)(playerPosition.y / cellSize.y));
        return currentPositionInArea;
    }
}