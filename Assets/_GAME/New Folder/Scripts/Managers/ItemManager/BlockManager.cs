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
    public void InitBlock()
    {
        var blockDatas = new Block[25];

        var block = new Block() { subBlockIndex = new int[4] { 0, 1, 0, 1 } };
        blockDatas[0] = block;

        var length = gridWord.gridSize.x * gridWord.gridSize.y;
        this.blocks = new BlockCtrl[length];

        for (int i = 0; i < blockDatas.Length; i++)
        {
            var blockData = blockDatas[i];
            if (blockData.Equals(default(Block))) continue;

            var pos = gridWord.ConvertIndexToWorldPos(i);
            this.blocks[i] = SpawnBlock(pos, blockDatas[i].subBlockIndex, _blocksParent);
        }
    }

    public BlockCtrl SpawnBlock(float3 pos, int[] subBlockIndexs, Transform parent)
    {
        var block = Instantiate(blockPref, parent);
        block.InitBlock(gridWord.scale, pos, subBlockIndexs);
        return block;
    }
}