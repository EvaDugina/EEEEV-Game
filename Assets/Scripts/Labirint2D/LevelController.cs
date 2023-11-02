using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public GameObject Player;
    public LevelSpawner LevelSpawner;
    //public LabirintsSpawner2D LabirintsSpawner2D;

    [Header("Размеры лабиринта - целые, нечётные числа")]

    [Range(21, 99)]
    public int Width;
    [Range(21, 99)]
    public int Height;

    public LevelConfiguration LevelConfiguration;

    [NonSerialized] public Level Level;

    private bool flagX = false;
    private bool flagY = false;

    private void Awake()
    {

        // Проеряем лабиринт на чётность и если чётный - делаем нечётным
        GetComponent<LevelController>().Width -= Width % 2;
        GetComponent<LevelController>().Height -= Height % 2;

        LevelConfiguration.SetAreaParamsToList();
    }


    private void Start()
    {

        // Генерируем уровень
        LevelGenerator levelGenerator = new(Width, Height, LevelConfiguration.GetAreasParams());
        Level level = levelGenerator.GenerateLevel();

        LevelSpawner.SpawnLevel(level, LevelConfiguration);

        //LevelGenerator2D levelGenerator = new(Width, Height);
        //Level = levelGenerator.GenerateLevel();

        // Ставим на сцену
        //LabirintsSpawner2D.SpawnLabirints(Level);

        Width /= 2;
        Height /= 2;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 playerPosition = Player.transform.position;


        int signX = (int)Mathf.Sign(playerPosition.x);
        int ceilPositiveX = (int)playerPosition.x * signX;

        if (flagX)
        {
            if (ceilPositiveX != Width - 2)
                flagX = false;
        }
        else if (ceilPositiveX > Width + 1)
        {
            playerPosition.x = -1 * signX * (Width - ceilPositiveX % Width - playerPosition.x * signX + ceilPositiveX);
            flagX = true;
        }


        int signY = (int)Mathf.Sign(playerPosition.y);
        int ceilPositiveY = (int)playerPosition.y * signY;

        if (flagY)
        {
            if (ceilPositiveY != Height - 2)
                flagY = false;
        }
        else if (ceilPositiveY > Height + 1)
        {
            playerPosition.y = -1 * signY * (Height - ceilPositiveY % Height - playerPosition.y * signY + ceilPositiveY);
            flagY = true;
        }

        //if (flagY && randomResidueY != -1 &&  ceilPositiveY % Height > 6 || ceilPositiveY % Height < 4)
        //{
        //    flagY = false;
        //}
        //else if (ceilPositiveX > Height)
        //{
        //    int residueY = Random.Range(0, 3);
        //    if (ceilPositiveY != residueY && ceilPositiveY % Height == residueY)
        //    {
        //        flagY = true;
        //        playerPosition.y *= -1;
        //        randomResidueY = residueY;
        //    }
        //}


        Player.transform.position = playerPosition;
    }
}
