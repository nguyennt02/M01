using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager
{
    [Header("Block")]
    [SerializeField] BlockCtrl blockPref;
    [SerializeField] float2 scale;
    [SerializeField] Transform _blocksParent;
    public Transform BlocksParent { get; private set; }
    BlockCtrl[] blocks;
    public void InitBlock()
    {
        var blocks = new Block[25];
        for (int i = 0; i < 25; i++)
        {
            var block = new Block() { index = i, subBlockIndex = new int[4] { 0, 0, 0, 0 } };
            blocks[i] = block;
        }

        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block.Equals(default(Block))) continue;

            var pos = GirdWordManager.Instance.ConvertIndexToWorldPos(blocks[i].index);
            this.blocks[i] = SpawnBlock(pos, blocks[i].subBlockIndex);

            var value = GirdWordManager.Instance.GetFullValue();
            GirdWordManager.Instance.SetValueAt(pos, value);
        }
    }

    public BlockCtrl SpawnBlock(float3 pos, int[] subBlockIndexs)
    {
        var block = Instantiate(blockPref,_blocksParent);
        block.transform.position = pos;
        block.InjecValue(scale, pos);
        block.CreateBlock(subBlockIndexs);
        return block;
    }


}