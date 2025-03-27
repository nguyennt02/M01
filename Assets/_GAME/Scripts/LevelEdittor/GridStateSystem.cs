using Unity.Mathematics;
using UnityEngine;

public class GridStateSystem : MonoBehaviour
{
    [SerializeField] GridWord gridWord;
    [SerializeField] GridStateControl gridStatePrefab;
    public GridStateControl[] gridStateControls;
    public void CreateGrid(int2 gridSize, float2 scale, float3 centerPos)
    {
        RemoveGrid();
        gridWord.InitGridWord(gridSize, scale, centerPos);
        gridStateControls = new GridStateControl[gridSize.x * gridSize.y];
        for (int i = 0; i < gridStateControls.Length; i++)
        {
            gridStateControls[i] = Instantiate(gridStatePrefab, transform);
            gridStateControls[i].SetSize(scale);
            gridStateControls[i].transform.position = gridWord.ConvertIndexToWorldPos(i);
        }
    }

    public void InitData(LevelDesignObject data)
    {
        for (int i = 0; i < gridStateControls.Length; i++)
        {
            if (gridStateControls[i])
            {
                gridStateControls[i].SetGridState((GRIDSTATE)data.grids[i].GRIDSTATE);
                gridStateControls[i].ColorIndexs = data.grids[i].ColorIndexs;
            }
        }
    }

    void RemoveGrid()
    {
        for (int i = 0; i < gridStateControls.Length; i++)
        {
            if (gridStateControls[i])
                DestroyImmediate(gridStateControls[i].gameObject);
        }
    }
}
