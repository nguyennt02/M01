using Unity.Mathematics;
using UnityEngine;

public partial class GridWord : MonoBehaviour
{
    public int2 gridSize { get; private set; }
    public float2 scale { get; private set; }
    public float2 offset { get => new float2((gridSize.x - 1) * 0.5f * scale.x, (gridSize.y - 1) * 0.5f * scale.y); }

    public void InitGridWord(int2 gridSize, float2 scale, float3 centerPos)
    {
        this.gridSize = gridSize;
        this.scale = scale;
        transform.position = centerPos;
    }

    public int ConvertGridPosToIndex(in int2 girdPos)
    {
        int index = girdPos.x + girdPos.y * gridSize.x;
        return index;
    }

    public int2 ConvertIndexToGridPos(in int index)
    {
        int x = index % gridSize.x;
        int y = index / gridSize.x;
        return new int2(x, y);
    }

    public float3 ConvertGridPosToWorldPos(in int2 girdPos)
    {
        var originalPosition = new float3(
            girdPos.x * scale.x - offset.x,
            girdPos.y * scale.y - offset.y,
            0);
        var wordPos = originalPosition + (float3)(transform.position);
        return wordPos;
    }

    public int2 ConvertWorldPosToGridPos(in float3 wordPos)
    {
        var originalPosition = wordPos - (float3)transform.position;
        var girdPos = new int2(
            (int)math.round((originalPosition.x + offset.x) / scale.x),
            (int)math.round((originalPosition.y + offset.y) / scale.y)
        );
        return girdPos;
    }

    public int ConvertWorldPosToIndex(in float3 worldPos)
    {
        var grid = ConvertWorldPosToGridPos(worldPos);
        var index = ConvertGridPosToIndex(grid);
        return index;
    }

    public float3 ConvertIndexToWorldPos(in int index)
    {
        var grid = ConvertIndexToGridPos(index);
        var worldPos = ConvertGridPosToWorldPos(grid);
        return worldPos;
    }

    public bool IsPosOutsideAt(float3 worldPos)
    {
        int2 gridPos = ConvertWorldPosToGridPos(worldPos);
        return IsGridPosOutsideAt(gridPos);
    }

    public bool IsGridPosOutsideAt(int2 gridPos)
    {
        var index = ConvertGridPosToIndex(gridPos);
        if (gridPos.x > gridSize.x - 1
        || gridPos.x < 0
        || gridPos.y > gridSize.y - 1
        || gridPos.y < 0) return true;
        return false;
    }

    public float3[] FindNeighborAt(int index)
    {
        var wordPos = ConvertIndexToWorldPos(index);
        return FindNeighborAt(wordPos);
    }

    public float3[] FindNeighborAt(int2 gridPos)
    {
        var wordPos = ConvertGridPosToWorldPos(gridPos);
        return FindNeighborAt(wordPos);
    }

    public float3[] FindNeighborAt(float3 worldPos)
    {
        int2[] directions = new int2[] { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
        float3[] neighbors = new float3[directions.Length];
        var gridPos = ConvertWorldPosToGridPos(worldPos);
        for (int i = 0; i < directions.Length; ++i)
        {
            var dir = directions[i];
            var neighbor = gridPos + dir;
            float3 wPos = ConvertGridPosToWorldPos(neighbor);
            neighbors[i] = wPos;
        }
        return neighbors;
    }
}
