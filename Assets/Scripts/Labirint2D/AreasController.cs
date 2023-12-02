using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AreasController : MonoBehaviour
{

    //[Header("Параметры лабиринтов")]
    // TODO: Сделать через массив
    //public List<AreaParams> AreaParams = new();

    [Header("Параметры ROOM-лабиринта")]
    [SerializeField] private Parameters MainAreaParams;

    [Header("Параметры ROOM-лабиринта")]
    [SerializeField] private Parameters RoomAreaParams;

    [Header("Параметры FIELD-лабиринта")]
    [SerializeField] private Parameters FieldAreaParams;

    [Header("Параметры CORRIDOR-лабиринта")]
    [SerializeField] private Parameters CorridorAreaParams;

    private LevelConfiguration LevelConfiguration;

    private void Awake()
    {

        MainAreaParams.Status = true;

        // Заполняем конфигурацию
        RoomAreaParams.Type = AreaType.Main;
        RoomAreaParams.Type = AreaType.Room;
        FieldAreaParams.Type = AreaType.Field;
        CorridorAreaParams.Type = AreaType.Corridor;

        LevelConfiguration = new LevelConfiguration();
        LevelConfiguration.AddAreaParamsToList(MainAreaParams);
        LevelConfiguration.AddAreaParamsToList(RoomAreaParams);
        LevelConfiguration.AddAreaParamsToList(FieldAreaParams);
        LevelConfiguration.AddAreaParamsToList(CorridorAreaParams);
    }

    public Vector3Int GetCellSize(AreaType areaType)
    {
        if (areaType == AreaType.ReflectedMain)
            areaType = AreaType.Main;
        return LevelConfiguration.GetParametersByAreaType(areaType).SpawnParams.CellSize;
    }

    public List<Parameters> GetLevelParameters()
    {
        return LevelConfiguration.GetParametersList();
    }

    public LevelConfiguration GetLevelConfiguration()
    {
        return LevelConfiguration;
    } 



    public GameObject RotateSecondaryAreaRelativelyPlayerMovement2D(Area destinationArea, Vector2 vectorMovement)
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
        if (vectorMovement.y > 0) areaLocalEulerAngels.y = 90;
        else if (vectorMovement.y < 0) areaLocalEulerAngels.y = -90;
        else if (vectorMovement.x < 0) areaLocalEulerAngels.y = 180;
        else areaLocalEulerAngels.y = 0;
        areaObject.transform.localEulerAngles = areaLocalEulerAngels;

        // Меняем позицию после поворота
        areaPosition = areaObject.transform.position;
        Vector3Int cellSize = LevelConfiguration.GetParametersByAreaType(destinationArea.Type).SpawnParams.CellSize;
        if (vectorMovement.x > 0)
            areaObject.transform.position = new Vector3(areaPosition.x - pointAroundRotation.x, areaPosition.y, areaPosition.z - pointAroundRotation.y);
        else if (vectorMovement.x < 0)
            areaObject.transform.position = new Vector3(areaPosition.x + pointAroundRotation.x, areaPosition.y, areaPosition.z + pointAroundRotation.y + cellSize.z);
        else if (vectorMovement.y > 0)
            areaObject.transform.position = new Vector3(areaPosition.x + pointAroundRotation.x, areaPosition.y, areaPosition.z + pointAroundRotation.y + cellSize.z);
        else if (vectorMovement.y < 0)
            areaObject.transform.position = new Vector3(0, areaPosition.y, areaPosition.z - pointAroundRotation.y);

        return areaObject;
    }

    public Vector2Int GenerateOutPortal(/*Area currentArea, */Vector2Int portalPosition, StaticPositionParameter portalParameter)
    {
        Debug.Log(Input.GetAxis("Vertical"));
        Debug.Log(Input.GetAxis("Horizontal"));
        if (portalParameter == StaticPositionParameter.Left)
            return new Vector2Int(portalPosition.x - 1, portalPosition.y);

        else if (portalParameter == StaticPositionParameter.Right)
            return new Vector2Int(portalPosition.x + 1, portalPosition.y);

        else if (portalParameter == StaticPositionParameter.Top)
            return new Vector2Int(portalPosition.x, portalPosition.y + 1);

        else if (portalParameter == StaticPositionParameter.Bottom)
            return new Vector2Int(portalPosition.x, portalPosition.y - 1);

        else
            return Vector2Int.zero;

    }

    private Vector2 TranslateAreaCoordinatesToRealCoordinates(AreaType areaType, Vector2Int point)
    {
        if (areaType == AreaType.ReflectedMain)
            areaType = AreaType.Main;

        Vector3Int cellSize = LevelConfiguration.GetParametersByAreaType(areaType).SpawnParams.CellSize;

        Vector2 realPoint = new Vector2Int(point.x, point.y);
        realPoint.x *= cellSize.x;
        realPoint.y *= cellSize.y;

        return realPoint;
    }


    public Vector2Int ParseToAreaSizePosition(AreaType areaType, Vector3 playerPosition)
    {
        if (areaType == AreaType.ReflectedMain)
            areaType = AreaType.Main;

        Vector3Int cellSize = LevelConfiguration.GetParametersByAreaType(areaType).SpawnParams.CellSize;
        Vector2Int currentPositionInArea = new Vector2Int((int)(playerPosition.x / cellSize.x), (int)(playerPosition.z / cellSize.z));
        return currentPositionInArea;
    }

}