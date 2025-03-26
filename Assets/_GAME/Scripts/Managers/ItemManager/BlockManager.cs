using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager
{
    [Header("Block")]
    [SerializeField] BlockCtrl blockPref;
    [SerializeField] Transform _blocksParent;
    public Transform BlocksParent { get => _blocksParent; }
    [SerializeField] Transform _blockRemovesParent;
    public Transform BlockRemovesParent { get => _blockRemovesParent; }
    public BlockCtrl[] blocks;
    public void InitBlock(LevelDesignObject data)
    {
        blocks = new BlockCtrl[data.grids.Length];

        for (int i = 0; i < blocks.Length; i++)
        {
            if (data.grids[i].GRIDSTATE != GRIDSTATE.BLOCK) continue;

            var pos = gridWord.ConvertIndexToWorldPos(i);
            blocks[i] = SpawnBlock(pos, data.grids[i].ColorIndexs, _blocksParent);

            var value = gridWord.EmptyValue + (int)GRIDSTATE.BLOCK;
            gridWord.SetValueAt(pos, value);
        }
    }

    public BlockCtrl SpawnBlock(float3 pos, int[] subBlockIndexs, Transform parent)
    {
        var block = Instantiate(blockPref, parent);
        block.InitBlock(gridWord.scale, pos, subBlockIndexs);
        return block;
    }
}