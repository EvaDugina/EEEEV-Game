
using UnityEngine;

public enum WallType
{
    Top, Right, Bottom, Left
}

public class Wall
{
    public WallType Type;
    public Material Material;

    public Wall(WallType type, Material material)
    {
        Type = type;
        Material = material;
    }
}
