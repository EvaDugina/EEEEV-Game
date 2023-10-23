
public class LevelGenerator2D
{


    private int MainMazeWidth = 32;
    private int MainMazeHeight = 32;


    public LevelGenerator2D(int width, int height)
    {
        MainMazeWidth = width;
        MainMazeHeight = height;
    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
|   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */

    /// <summary>
    // Генерируем уровень
    /// </summary>
    public Level GenerateLevel()
    {
        Level level2D = new Level(CreateMaze(MazeAreaType.Main));
        level2D.SecondaryMazes.Add(CreateMaze(MazeAreaType.Field));
        level2D.SecondaryMazes.Add(CreateMaze(MazeAreaType.Room));
        level2D.SecondaryMazes.Add(CreateMaze(MazeAreaType.Corridor));

        return level2D;

    }

    public Maze CreateMaze(MazeAreaType type)
    {

        int width;
        int height;

        if (type == MazeAreaType.Field)
        {
            width = MainMazeWidth / 4;
            height = MainMazeHeight / 4;
        }
        else if (type == MazeAreaType.Room)
        {
            width = MainMazeWidth / 5;
            height = MainMazeHeight / 5;
        }
        else if (type == MazeAreaType.Corridor)
        {
            width = MainMazeWidth/2;
            height = 2;
        }
        else {
            width = MainMazeWidth;
            height = MainMazeHeight;
        }

        MazeGenerator2D mazeGenerator = new MazeGenerator2D(type, width, height);
        return mazeGenerator.Generate();

    }

}
