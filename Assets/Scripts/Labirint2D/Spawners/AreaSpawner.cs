using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject AreaPrefab;
    public Area2D Area2D;

    public MazeSpawner MazeSpawner;


    public CellSpawnConfiguration CellSpawnConfiguration;

    private Transform AreasFolder;



    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Public Methods
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/

    public void Spawn(Transform areasFolder, LevelConfiguration levelConfiguration, Area mainArea, List<Area> secondaryAreas) {
        AreasFolder = areasFolder;

        SpawnArea(mainArea, levelConfiguration.GetAreaSpawnParamsByType(AreaType.Main));

        foreach (Area area in secondaryAreas)
        {
            SpawnArea(area, levelConfiguration.GetAreaSpawnParamsByType(area.Type));
        }
    }


    public void SpawnArea(Area area, SpawnParams areaParams)
    {
        CellHandler cellHandler = new CellHandler(CellSpawnConfiguration);

        Cell cellTemplate = cellHandler.GetCellByDecoration(areaParams.CellParameters.Decoration);
        cellTemplate.SetSize(areaParams.CellParameters.Size);

        // Ставим каркас Area
        GameObject areaObject = Instantiate(AreaPrefab,
            AreasFolder.TransformPoint(new Vector3(area.X, area.Y, area.ZIndex)),
            Quaternion.identity, AreasFolder);
        areaObject.name = area.Id + area.GetAreaTypeAsText() + "Area";

        Area2D area2D = areaObject.GetComponent<Area2D>();

        MazeSpawner.Spawn(area2D.MazeFolder.transform, cellTemplate, area.MainMaze, area.BoundaryMazes);
    }


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Private Methods
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/


    //private void SetCellSize(Vector2Int cellSize)
    //{
    //    Vector3 cellScale = CellPrefab.transform.localScale;
    //    cellScale.x = cellSize.x;
    //    cellScale.y = cellSize.y;
    //    //cellScale.z = CellSize.z;
    //    CellPrefab.transform.localScale = cellScale;
    //}
}
