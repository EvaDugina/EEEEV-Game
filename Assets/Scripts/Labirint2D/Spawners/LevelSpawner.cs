using UnityEngine;

public class LevelSpawner : MonoBehaviour
{

    [SerializeField] private GameObject LevelObject;
    [SerializeField] private AreaSpawner AreaSpawner;

    [Space]
    [SerializeField] private RouteLineRenderer2D RouteLineRenderer;



    public void SpawnLevel(Level2D level2D, Level level, LevelConfiguration levelConfiguration)
    {
        AreaSpawner.Spawn(level2D.AreasFolder.transform, levelConfiguration, level.MainArea, level.ReflectedArea, level.SecondaryAreas);
    }

}
