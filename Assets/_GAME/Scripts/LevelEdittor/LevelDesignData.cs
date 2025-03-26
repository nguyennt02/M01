using System;
using Unity.Mathematics;
public enum TYPEBLOCK
{
    BLUE,
    BROWN,
    GREEN,
    ORANGE,
    PINK,
    PURPLE,
    RED,
    YELLOW,
    DESTRUCTIBLEBLOCK = 11,
    SPECIALSUBBLOCK = 12,
}

public enum GRIDSTATE
{
    NONE,
    BLOCK,
    REMOVE
}

[Serializable]
public struct LevelDesignDatas
{
    public LevelDesignData[] data;
}
[Serializable]
public struct LevelDesignData
{
    public int2 gridSize;
    public float2 scale;
    public float3 centerPos;
    public Grid[] grids;
}
[Serializable]
public struct Grid
{
    public GRIDSTATE GRIDSTATE;
    public int[] ColorIndexs;
}
