using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager : MonoBehaviour
{
    [Header("Base Item")]
    public static ItemManager Instance { get; private set; }


    private void Start()
    {
        if (Instance == null) Instance = this;
    }

    public void InitGird()
    {
        var gridSize = new int2(5,5);
        floorBlocks = new FloorBlockCtrl[gridSize.x * gridSize.y];
        blocks = new BlockCtrl[gridSize.x * gridSize.y];
        
        GirdWordManager.Instance.GridSize = gridSize;
        GirdWordManager.Instance.BakingGridWorld();
    }
}
