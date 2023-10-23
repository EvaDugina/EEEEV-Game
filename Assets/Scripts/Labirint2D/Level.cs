
using System.Collections.Generic;

public class Level
{
    public Maze MainMaze;
    public List<Maze> SecondaryMazes;

    public Level(Maze mainMaze)
    {
        MainMaze = mainMaze;
        SecondaryMazes = new List<Maze>(); ;
    }

}
