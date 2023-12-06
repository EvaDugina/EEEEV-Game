using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum StaticPositionParameter
{
    None, Random, Order,
    Left, Center, Right, Top, Bottom
}

public enum DynamicPositionParameter
{
    None, ByDistance, ByMovingSide
}

public class AreaStructure
{
    public StaticPositionParameter StartParameters;
    public DynamicPositionParameter FinishParameter;

    public List<StaticPositionParameter> PortalOutParameters;
    public DynamicPositionParameter PortalBackParameter;

}

public class AreaStructureHandler
{

    public static Vector2Int GetPositionByStaticParameter(StaticPositionParameter parameter, int width, int height)
    {

        switch (parameter)
        {
            case StaticPositionParameter.Random:

                // Чтобы при динамической подстановке точки возвращения из портала
                // не выйти за границы лабиринта не ставим порталы на края
                int x = Random.Range(1, width - 1);
                int y = Random.Range(1, height - 1);

                return new Vector2Int(x, y);

            case StaticPositionParameter.Left:
                return new Vector2Int(0, height / 2);

            case StaticPositionParameter.Center:
                return new Vector2Int(width / 2, height / 2);

            case StaticPositionParameter.Right:
                return new Vector2Int(width - 1, height / 2);

            case StaticPositionParameter.Top:
                return new Vector2Int(width / 2, height - 1);

            case StaticPositionParameter.Bottom:
                return new Vector2Int(width / 2, 0);

            default:
                return new Vector2Int();
        }
    }

    public static Vector2Int GeneratePortalOrderedPosition(Area area, int distanceBetweenPortals, int countSecondaryAreas)
    {
        int order = area.Portals.Count;
        if (order == 0)
        {
            return GoAheadReverseThroughTheMazeByDistance(area.MainMaze, area.MainMaze.FinishPosition, distanceBetweenPortals);
        }
        else
        {
            Vector2Int lastPortalPosition = area.Portals[order - 1].Position;
            return GoAheadReverseThroughTheMazeByDistance(area.MainMaze, lastPortalPosition, distanceBetweenPortals);
        }
    }

    public static Vector2Int GoAheadReverseThroughTheMazeByDistance(Maze maze, Vector2Int endPosition, int distance)
    {
        MazeCell currentMazeCell = maze.Cells[endPosition.x][endPosition.y];
        int currentDistanceFromStart = currentMazeCell.DistanceFromStart;
        for (int i = 0; i < distance; i++)
        {
            if (currentMazeCell.X < maze.Width - 1 && maze.Cells[currentMazeCell.X + 1][currentMazeCell.Y].DistanceFromStart == currentDistanceFromStart - 1) {
                currentMazeCell = maze.Cells[currentMazeCell.X + 1][currentMazeCell.Y];
                currentDistanceFromStart = currentMazeCell.DistanceFromStart;
            } else if (currentMazeCell.X > 0 && maze.Cells[currentMazeCell.X - 1][currentMazeCell.Y].DistanceFromStart == currentDistanceFromStart - 1)
            {
                currentMazeCell = maze.Cells[currentMazeCell.X - 1][currentMazeCell.Y];
                currentDistanceFromStart = currentMazeCell.DistanceFromStart;
            } else if (currentMazeCell.Y < maze.Height - 1 && maze.Cells[currentMazeCell.X][currentMazeCell.Y + 1].DistanceFromStart == currentDistanceFromStart - 1)
            {
                currentMazeCell = maze.Cells[currentMazeCell.X][currentMazeCell.Y + 1];
                currentDistanceFromStart = currentMazeCell.DistanceFromStart;
            } else if (currentMazeCell.Y > 0 && maze.Cells[currentMazeCell.X][currentMazeCell.Y - 1].DistanceFromStart == currentDistanceFromStart - 1)
            {
                currentMazeCell = maze.Cells[currentMazeCell.X][currentMazeCell.Y - 1];
                currentDistanceFromStart = currentMazeCell.DistanceFromStart;
            }
        }

        return new Vector2Int(currentMazeCell.X, currentMazeCell.Y);
    }

    public static int GetCountPortalsOutByAreaStructure(AreaStructure areaStructure)
    {
        return areaStructure.PortalOutParameters.Count;
    }

    public static List<Vector2Int> GetPositionByParameters(List<StaticPositionParameter> parameters, int width, int height)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        foreach (StaticPositionParameter parameter in parameters)
        {
            positions.Add(GetPositionByStaticParameter(parameter, width, height));
        }
        return positions;
    }

    public static AreaStructure GetAreaStructureByAreaType(AreaType type)
    {
        if (type == AreaType.Main)
            return GetMainAreaStructure();
        else if (type == AreaType.Room)
            return GetRoomAreaStructure();
        else if (type == AreaType.Field)
            return GetFieldAreaStructure();
        else
            return GetCorridorAreaStructure();
    }

    public static AreaStructure GetMainAreaStructure()
    {
        return new AreaStructure()
        {
            StartParameters = StaticPositionParameter.Center,
            FinishParameter = DynamicPositionParameter.ByDistance,
            PortalBackParameter = DynamicPositionParameter.ByMovingSide,
            PortalOutParameters = new List<StaticPositionParameter>() {
                StaticPositionParameter.Order
            },
        };
    }

    public static AreaStructure GetRoomAreaStructure()
    {
        return new AreaStructure()
        {
            StartParameters = StaticPositionParameter.Left,
            FinishParameter = DynamicPositionParameter.None,
            PortalBackParameter = DynamicPositionParameter.None,
            PortalOutParameters = new List<StaticPositionParameter>() {
                StaticPositionParameter.Top,
                StaticPositionParameter.Bottom,
                StaticPositionParameter.Right,
            },
        };
    }

    public static AreaStructure GetFieldAreaStructure()
    {
        return new AreaStructure()
        {
            StartParameters = StaticPositionParameter.Center,
            FinishParameter = DynamicPositionParameter.None,
            PortalBackParameter = DynamicPositionParameter.None,
            PortalOutParameters = new List<StaticPositionParameter>() {
                StaticPositionParameter.Top,
                StaticPositionParameter.Bottom,
                StaticPositionParameter.Left,
                StaticPositionParameter.Right,
            },
        };
    }

    public static AreaStructure GetCorridorAreaStructure()
    {
        return new AreaStructure()
        {
            StartParameters = StaticPositionParameter.Left,
            FinishParameter = DynamicPositionParameter.None,
            PortalBackParameter = DynamicPositionParameter.None,
            PortalOutParameters = new List<StaticPositionParameter>() {
                StaticPositionParameter.Right
            },
        };
    }

}
