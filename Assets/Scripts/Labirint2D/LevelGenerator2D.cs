
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
        Level level2D = new Level(CreateMaze(MazeStructureType.Main));
        //level2D.SecondaryMazes.Append(GenerateMaze(MazeType.Field));

        return level2D;

    }

    public Maze CreateMaze(MazeStructureType type)
    {

        int width;
        int height;

        if (type == MazeStructureType.Field)
        {
            width = MainMazeWidth / 5;
            height = MainMazeHeight / 5;
        }
        else {
            width = MainMazeWidth;
            height = MainMazeHeight;
        }

        MazeGenerator2D mazeGenerator = new MazeGenerator2D(type, width, height);
        return mazeGenerator.Generate();

    }

}
