using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public partial class GridWord
{
    NativeArray<int> _grid;
    public NativeArray<int> Grid { get => _grid; }
    [SerializeField] int _emptyValue;
    public int EmptyValue { get => _emptyValue; }
    public void InitGridStatus()
    {
        _grid = new NativeArray<int>(gridSize.x * gridSize.y, Allocator.Persistent);
        for(int i = 0; i< _grid.Length; i++) _grid[i] = EmptyValue;
    }

    private void OnDestroy()
    {
        _grid.Dispose();
    }
    public void Clear()
    {
        _grid.Dispose();
    }

    public int GetValueAt(int index)
    {
        if (index < 0 || index > _grid.Length - 1) return _emptyValue;
        return _grid[index];
    }

    public int GetValueAt(float3 worldPos)
    {
        var index = ConvertWorldPosToIndex(worldPos);
        return GetValueAt(index);
    }

    public void SetValueAt(float3 worldPos, int value = 1)
    {
        int index = ConvertWorldPosToIndex(worldPos);
        SetValueAt(index);
    }

    public void SetValueAt(int index, int value = 1)
    {
        if (index < 0 || index > _grid.Length - 1) return;
        _grid[index] = value;
    }

    public bool IsPosOccupiedAt(float3 worldPos)
    {
        int2 gridPos = ConvertWorldPosToGridPos(worldPos);
        return IsGridPosOccupiedAt(gridPos);
    }

    public bool IsGridPosOccupiedAt(int2 gridPos)
    {
        if (IsGridPosOutsideAt(gridPos)) return true;
        var index = ConvertGridPosToIndex(gridPos);
        if (_grid[index] > _emptyValue) return true;
        return false;
    }
}