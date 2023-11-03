using UnityEngine;

public class LevelSpawner : MonoBehaviour
{

    [SerializeField] private GameObject LevelPrefab;
    public AreaSpawner AreaSpawner;

    [Space]
    [SerializeField] private RouteLineRenderer2D RouteLineRenderer;


    public Level2D Level2D;


    public void SpawnLevel(Level level, LevelConfiguration levelConfiguration)
    {
        // Создаём структуру с лабиринтом
        GameObject levelObject = Instantiate(LevelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        levelObject.name = "Level";

        AreaSpawner.Spawn(Level2D.AreasFolder.transform, levelConfiguration, level.MainArea, level.SecondaryAreas);
    }

}
