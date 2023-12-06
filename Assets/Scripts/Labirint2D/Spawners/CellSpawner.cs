
using System;
using TMPro;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{

    [SerializeField] private Material StartMaterial;
    [SerializeField] private Material FinishMaterial;
    [SerializeField] private Material PortalMaterial;

    private Transform CellsFolder;
    private GameObject CellPrefab;
    private Cell Cell;

    public void Spawn(Transform cellsFolder, Cell cell, GameObject cellPrefab, MazeCell[][] mazeCells)
    {

        CellsFolder = cellsFolder;
        Cell = cell;
        CellPrefab = cellPrefab;

        for (int x = 0; x < mazeCells.Length; x++)
        {
            for (int y = 0; y < mazeCells[x].Length; y++)
            {
                mazeCells[x][y].SetXY(x, y);
                SpawnCell(mazeCells[x][y]);

            }
        }

    }

    public void SpawnCell(MazeCell mazeCell)
    {
        if (mazeCell.Status == MazeCellStatus.Disable) return;

        GameObject cellObject = Instantiate(CellPrefab,
            CellsFolder.TransformPoint(new Vector3(mazeCell.X * Cell.Width, 0, mazeCell.Y * Cell.Length)),
            Quaternion.identity, CellsFolder);
        cellObject.name = "[" + mazeCell.X.ToString() + "][" + mazeCell.Y.ToString() + "] Cell";
        cellObject.SetActive(true);

        Cell3D cell = cellObject.GetComponent<Cell3D>();

        //SetMaterialsToWallsAndColumns(cell);
        SetVisibilityToWallsAndColumns(cell, mazeCell.WallsStatus, mazeCell.ColumnsStatus);

        // Выбираем материал пола
        //if (mazeCell.Type == MazeCellType.Default)
        //    cell.Floor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = Cell.Floor.Material;
        if (mazeCell.Type != MazeCellType.Default)
        {
            if (mazeCell.Type == MazeCellType.Start)
                cell.Floor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = StartMaterial;
            else if (mazeCell.Type == MazeCellType.Finish)
                cell.Floor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = FinishMaterial;
            else
                cell.Floor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = PortalMaterial;
        }
        cell.Floor.SetActive(true);


        //else
        //{
        //    /// Добавляем колонне другой материал, когда колонна - перекрёсток
        //    if (columnType == ColumnType.Crossroad)
        //        cell.Column.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cell.EnableColumnMaterial;
        //    else
        //        cell.Column.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cell.DisableColumnMaterial;
        //}



        cell.TextDistance.GetComponent<TextMeshPro>().text = mazeCell.DistanceFromStart.ToString();
    }


    //private void SetMaterialsToWallsAndColumns(Cell3D cell)
    //{
    //    foreach (Wall wall in Cell.Walls)
    //    {
    //        GameObject wallObject;
    //        if (wall.Type == WallType.Top) wallObject = cell.Walls.TopWall;
    //        else if (wall.Type == WallType.Left) wallObject = cell.Walls.LeftWall;
    //        else if (wall.Type == WallType.Right) wallObject = cell.Walls.RightWall;
    //        else wallObject = cell.Walls.BottomWall;

    //        wallObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = wall.Material;
    //    }

    //    foreach (Column column in Cell.Columns)
    //    {
    //        GameObject columnObject;
    //        if (column.Type == ColumnType.TopLeft) columnObject = cell.Columns.TopLeft;
    //        else if (column.Type == ColumnType.TopRight) columnObject = cell.Columns.TopRight;
    //        else if (column.Type == ColumnType.BottomLeft) columnObject = cell.Columns.BottomLeft;
    //        else columnObject = cell.Columns.BottomRight;

    //        columnObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = column.ColumnMaterial;
    //    }
    //}

    public GameObject GetResizedCellPrefab(GameObject cellPrefab, float width, float height, float length, Transform mazesFolder)
    {
        GameObject resizedCellPrefab = Instantiate(cellPrefab, mazesFolder);
        resizedCellPrefab.transform.localScale = new Vector3(width, height, length);
        resizedCellPrefab.name = "Template Cell";

        resizedCellPrefab.SetActive(false);

        return resizedCellPrefab;
    }


    /* 
─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Utilities
───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
*/

    private void SetVisibilityToWallsAndColumns(Cell3D cell, MazeCellWallsStatus wallsStatus, MazeCellColumnsStatus columnsStatus)
    {

        // Показываем стены
        cell.Walls.TopWall.SetActive(wallsStatus.TopWall);
        cell.Walls.LeftWall.SetActive(wallsStatus.LeftWall);
        cell.Walls.BottomWall.SetActive(wallsStatus.BottomWall);
        cell.Walls.RightWall.SetActive(wallsStatus.RightWall);

        //Отключаем / включаем колонны, когда рядом нет соседей)
        cell.Columns.TopLeft.SetActive(columnsStatus.TopLeft);
        cell.Columns.TopRight.SetActive(columnsStatus.TopRight);
        cell.Columns.BottomLeft.SetActive(columnsStatus.BottomLeft);
        cell.Columns.BottomRight.SetActive(columnsStatus.BottomRight);
    }

}
