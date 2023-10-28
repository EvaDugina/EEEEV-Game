
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

    public List<Maze> GetAllMazes()
    {
        List<Maze> mazes = new List<Maze>
        {
            MainMaze
        };
        mazes.AddRange(SecondaryMazes);
        return mazes;
    }

    public Maze GetMazeByName(string name) { 
        if (name == MainMaze.Name) 
            return MainMaze;
        
        foreach (Maze maze in SecondaryMazes)
            if (name == maze.Name)
                return maze;

        return null;
    }

}
