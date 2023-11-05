using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{

    public GameObject OriginalCellPrefab;
    private GameObject ResizedCellPrefab;


    private Transform CellsFolder;
    private Cell Cell;


    public void Spawn(Transform cellsFolder, Cell cell, MazeCell[][] mazeCells) {

        CellsFolder = cellsFolder;
        Cell = cell;

        ResizedCellPrefab = GetResizedCellPrefab();

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
        Cell2D cell = Instantiate(ResizedCellPrefab,
            CellsFolder.TransformPoint(new Vector3(mazeCell.X * Cell.Width, mazeCell.Y * Cell.Height, 0)),
            Quaternion.identity, CellsFolder)
            .GetComponent<Cell2D>();

        // Показываем стены
        cell.Walls.TopWall.SetActive(mazeCell.WallsStatus.TopWall);
        cell.Walls.LeftWall.SetActive(mazeCell.WallsStatus.LeftWall);
        cell.Walls.BottomWall.SetActive(mazeCell.WallsStatus.BottomWall);
        cell.Walls.RightWall.SetActive(mazeCell.WallsStatus.RightWall);

        // Выбираем материал пола
        cell.Floor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = Cell.Floor.Material;
        cell.Floor.SetActive(true);

        //Отключение/включение колонны, когда рядом нет соседей
        if (mazeCell.ColumnsStatus.TopLeft)
            cell.Columns.TopLeft.transform.GetChild(0).gameObject.SetActive(false);

        if (mazeCell.ColumnsStatus.TopRight)
            cell.Columns.TopRight.transform.GetChild(0).gameObject.SetActive(false);

        if (mazeCell.ColumnsStatus.BottomLeft)
            cell.Columns.BottomLeft.transform.GetChild(0).gameObject.SetActive(false);

        if (mazeCell.ColumnsStatus.BottomRight)
            cell.Columns.BottomRight.transform.GetChild(0).gameObject.SetActive(false);


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


    public GameObject GetResizedCellPrefab() {
        GameObject newCellPrefab = Instantiate(OriginalCellPrefab);
        newCellPrefab.transform.localScale = new Vector3(Cell.Width, Cell.Height, Cell.Length);

        return newCellPrefab;
    }


}
