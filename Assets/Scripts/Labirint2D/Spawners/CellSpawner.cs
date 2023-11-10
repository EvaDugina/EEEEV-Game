﻿
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    public GameObject CellPrefab;

    private Transform CellsFolder;
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
                SpawnCell(mazeCells[x][y]);

            }
        }

    }

    public void SpawnCell(MazeCell mazeCell)
    {
        GameObject cellObject = Instantiate(CellPrefab,
            CellsFolder.TransformPoint(new Vector3(mazeCell.X * Cell.Width, mazeCell.Y * Cell.Height, 0)),
            Quaternion.identity, CellsFolder);
        cellObject.name = "[" + mazeCell.X.ToString() + "][" + mazeCell.Y.ToString() + "] Cell";
        cellObject.SetActive(true);

        Cell2D cell = cellObject.GetComponent<Cell2D>();

        SetVisibilityToWallsAndColumns(cell, mazeCell.WallsStatus, mazeCell.ColumnsStatus);

        // Выбираем материал пола
        cell.Floor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = Cell.Floor.Material;
        cell.Floor.SetActive(true);


        //else
        //{
        //    /// Добавляем колонне другой материал, когда колонна - перекрёсток
        //    if (columnType == ColumnType.Crossroad)
        //        cell.Column.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cell.EnableColumnMaterial;
        //    else
        //        cell.Column.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cell.DisableColumnMaterial;
        //}



        //cell.TextDistance.GetComponent<TextMeshPro>().text = maze.Cells[x][y].DistanceFromStart.ToString();
    }


    public GameObject GetResizedCellPrefab(float width, float height, float length, Transform mazesFolder)
    {
        GameObject resizedCellPrefab = Instantiate(CellPrefab, mazesFolder);
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

    private void SetVisibilityToWallsAndColumns(Cell2D cell, MazeCellWallsStatus wallsStatus, MazeCellColumnsStatus columnsStatus) {

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
