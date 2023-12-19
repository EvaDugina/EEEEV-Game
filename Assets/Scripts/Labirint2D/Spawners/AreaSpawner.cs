using System;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CellsSpawnPrefabs))]
public class AreaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject AreaPrefab;
    [SerializeField] private MazeSpawner MazeSpawner;
    [SerializeField] private CellsSpawnPrefabs CellsSpawnPrefabs;
    //[SerializeField] private CellHandler CellHandler;

    [SerializeField] private GameObject BoundaryCenterMazePrefab;

    private Transform AreasFolder;



    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Public Methods
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/

    public void Spawn(Transform areasFolder, LevelConfiguration levelConfiguration, Area mainArea, Area reflectedArea, List<Area> secondaryAreas)
    {
        AreasFolder = areasFolder;

        SpawnArea(mainArea, levelConfiguration.GetAreaSpawnParamsByType(AreaType.Main));

        if (reflectedArea != null)
            SpawnArea(reflectedArea, levelConfiguration.GetAreaSpawnParamsByType(AreaType.Main));

        foreach (Area area in secondaryAreas)
        {
            SpawnArea(area, levelConfiguration.GetAreaSpawnParamsByType(area.Type));
        }
    }

    public void SpawnArea(Area area, SpawnParams areaParams)
    {

        //Cell cellTemplate = CellHandler.CreateCellByDecoration(areaParams.Decoration);
        GameObject cellDecorationPrefab = CellsSpawnPrefabs.GetCellDecorationByDecoration(areaParams.Decoration);
        Cell cellTemplate = new Cell(cellDecorationPrefab, areaParams.CellSize);

        // Ставим каркас Area
        GameObject areaObject = Instantiate(AreaPrefab,
            AreasFolder.TransformPoint(new Vector3(0, area.ZIndex * 10, 0)),
            Quaternion.identity, AreasFolder);
        areaObject.name = area.Id + area.GetAreaTypeAsText() + "Area";
        areaObject.SetActive(false);

        Area2D area2D = areaObject.GetComponent<Area2D>();

        MazeSpawner.Spawn(area2D.MazeFolder.transform, cellTemplate, area.MainMaze, area.BoundaryMazes);

        if (area.Topology == AreaTopology.Toroid)
        {
            SpawnBoundaryCenterMaze(areaObject.transform, area, areaParams.CellSize);
        }

    }


    private void SpawnBoundaryCenterMaze(Transform areaObjectTransform, Area area, Vector3Int cellSize)
    {
        GameObject emptyObject = new GameObject();
        GameObject boundaryCenterMazeFolderObject = Instantiate(emptyObject, areaObjectTransform);
        boundaryCenterMazeFolderObject.name = area.Id + area.GetAreaTypeAsText() + "BoundaryWalls";
        Destroy(emptyObject);

        // TopBoundary
        GameObject TopBoundaryCenterMazeObject = Instantiate(BoundaryCenterMazePrefab,
            areaObjectTransform.TransformPoint(new Vector3(area.Width / 2 * cellSize.x, area.ZIndex * 10 + 5, area.Height * cellSize.z)),
            Quaternion.identity, boundaryCenterMazeFolderObject.transform);
        TopBoundaryCenterMazeObject.name = "Top" + "Boundary";
        TopBoundaryCenterMazeObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        TopBoundaryCenterMazeObject.transform.localScale = new Vector3((area.Width + 1) * cellSize.x, 10, 1 * cellSize.z);

        // RightBoundary
        GameObject RightBoundaryCenterMazeObject = Instantiate(BoundaryCenterMazePrefab,
            areaObjectTransform.TransformPoint(new Vector3(area.Width * cellSize.x, area.ZIndex * 10 + 5, area.Height / 2 * cellSize.z)),
            Quaternion.identity, boundaryCenterMazeFolderObject.transform);
        RightBoundaryCenterMazeObject.name = "Right" + "Boundary";
        RightBoundaryCenterMazeObject.transform.localEulerAngles = new Vector3(0, 90, 0);
        RightBoundaryCenterMazeObject.transform.localScale = new Vector3((area.Height + 1) * cellSize.x, 10, 1 * cellSize.z);

        // BottomBoundary
        GameObject BottomBoundaryCenterMazeObject = Instantiate(BoundaryCenterMazePrefab,
            areaObjectTransform.TransformPoint(new Vector3(area.Width / 2 * cellSize.x, area.ZIndex * 10 + 5, 0)),
            Quaternion.identity, boundaryCenterMazeFolderObject.transform);
        BottomBoundaryCenterMazeObject.name = "Bottom" + "Boundary";
        BottomBoundaryCenterMazeObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        BottomBoundaryCenterMazeObject.transform.localScale = new Vector3((area.Width + 1) * cellSize.x, 10, 1 * cellSize.z);

        // LeftBoundary
        GameObject LeftBoundaryCenterMazeObject = Instantiate(BoundaryCenterMazePrefab,
            areaObjectTransform.TransformPoint(new Vector3(0, area.ZIndex * 10 + 5, area.Height / 2 * cellSize.z)),
            Quaternion.identity, boundaryCenterMazeFolderObject.transform);
        LeftBoundaryCenterMazeObject.name = "Left" + "Boundary";
        LeftBoundaryCenterMazeObject.transform.localEulerAngles = new Vector3(0, -90, 0);
        LeftBoundaryCenterMazeObject.transform.localScale = new Vector3(area.Height * cellSize.z, 10, 1 * cellSize.z);


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
