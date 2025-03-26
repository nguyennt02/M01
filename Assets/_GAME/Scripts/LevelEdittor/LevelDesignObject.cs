using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignObject", menuName = "Level Data", order = 1)]
public class LevelDesignObject : ScriptableObject
{
    public int2 gridSize;
    public float2 scale;
    public float3 centerPos;
    public Grid[] grids;
}
