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

public enum BLOCKTYPE
{
    BLOCK,
    RAINBOWBLOCK,
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
    public int difficulty;
    public int[] colorValues;
    public AvailableBlock[] availableBlocks;
    public int ratioDoubleAvailableBlock;
    public int amountBlock;
    public Grid[] grids;
}
[Serializable]
public struct Grid
{
    public int GRIDSTATE;
    public int[] ColorIndexs;
}

[Serializable]
public struct AvailableBlock
{
    public int BLOCKTYPE;
    public int ratio;
}

[Serializable]
public struct AvailableBlockEditor
{
    public BLOCKTYPE BLOCKTYPE;
    public int ratio;
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public struct Ratio
{
    public int ratio1SubBlock;
    public int ratio2SubBlock;
    public int ratio3SubBlock;
}
