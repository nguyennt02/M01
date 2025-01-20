using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager
{
    [Header("Block")]
    [SerializeField] BlockCtrl blockPref;
    [SerializeField] Transform _blocksParent;
    public Transform BlocksParent { get => _blocksParent; }
    BlockCtrl[] blocks;
    public void InitBlock()
    {
        var blockDatas = new Block[25];
        // for (int i = 0; i < 25; i++)
        // {
        //     var block = new Block() { index = i, subBlockIndex = new int[4] { 0, 1, 3, 3 } };
        //     blockDatas[i] = block;
        // }

        var length = girdWord.GridSize.x * girdWord.GridSize.y;
        this.blocks = new BlockCtrl[length];

        for (int i = 0; i < blockDatas.Length; i++)
        {
            var blockData = blockDatas[i];
            if (blockData.Equals(default(Block))) continue;

            var pos = girdWord.ConvertIndexToWorldPos(blockDatas[i].index);
            this.blocks[i] = SpawnBlock(pos, blockDatas[i].subBlockIndex);

            var value = girdWord.GetFullValue();
            girdWord.SetValueAt(pos, value);
        }
    }

    public BlockCtrl SpawnBlock(float3 pos, int[] subBlockIndexs)
    {
        var block = Instantiate(blockPref, _blocksParent);
        block.transform.position = pos;
        block.Setup(girdWord.Scale, pos, subBlockIndexs);
        return block;
    }


}