using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelSpawner : MonoBehaviour
{

    [SerializeField] private GameObject LevelObject;
    [SerializeField] private AreaSpawner AreaSpawner;

    [Space]
    [SerializeField] private RouteLineRenderer2D RouteLineRenderer;

    Dictionary<int, GameObject> _spawnedAreaObjects = new Dictionary<int, GameObject>();


    public void SpawnLevelWithOptimization(Level2D level2D, Level level, LevelConfiguration levelConfiguration, Area area, Vector2Int playerCellPosition)
    {
        if (!IsSpawnedAreaObjectByArea(area))
        {
            GameObject newAreaObject = AreaSpawner.SpawnAreaWithOptimization(level2D.AreasFolder.transform, area, levelConfiguration.GetAreaSpawnParamsByType(area.Type), playerCellPosition, null);
            foreach (var spawnedItem in _spawnedAreaObjects)
                Destroy(spawnedItem.Value);
            _spawnedAreaObjects.Clear();

            _spawnedAreaObjects.Add(area.Id, newAreaObject);

            Debug.Log("NEW AREA!");
        }
        else {
            AreaSpawner.SpawnAreaWithOptimization(level2D.AreasFolder.transform, area, levelConfiguration.GetAreaSpawnParamsByType(area.Type), playerCellPosition, _spawnedAreaObjects[area.Id]);
        }
    }

    public void SpawnLevel(Level2D level2D, Level level, LevelConfiguration levelConfiguration)
    {
        AreaSpawner.Spawn(level2D.AreasFolder.transform, levelConfiguration, level.MainArea, level.ReflectedArea, level.SecondaryAreas);
    }

    public void SpawnReflectedArea(Level level, LevelConfiguration levelConfiguration)
    {
        AreaSpawner.SpawnArea(level.ReflectedArea, levelConfiguration.GetAreaSpawnParamsByType(AreaType.Main));
    }

    private bool IsSpawnedAreaObjectByArea(Area area)
    {
        foreach (KeyValuePair<int, GameObject> entry in _spawnedAreaObjects)
            if (entry.Key == area.Id)
                return true;
        return false;
    }

}
