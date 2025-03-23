using UnityEngine;

public partial class GridWord
{
    [SerializeField] bool shouldDrawString;
    [SerializeField] Color color;
    void OnDrawGizmos()
    {
        DrawGrid();
        DrawString();
    }
    /// <summary>
    /// Only for debugging
    /// </summary>
    void DrawGrid()
    {
        if (Grid == null) return;
        for (int i = 0; i < _grid.Length; ++i)
        {
            var worldPos = ConvertIndexToWorldPos(i);
            DrawGizmos.DrawQuad(worldPos, scale, color);
            var val = _grid[i];
            if (val < _emptyValue) continue;
            DrawGizmos.DrawQuad(worldPos, scale * 0.9f, color);
        }
    }

    void DrawString()
    {
        if (!shouldDrawString) return;
        for (int i = 0; i < _grid.Length; ++i)
        {
            var worldPos = ConvertIndexToWorldPos(i);
            var val = _grid[i];
            if (val < _emptyValue) continue;
            DrawGizmos.DrawString(val.ToString(), worldPos, color);
        }
    }
}