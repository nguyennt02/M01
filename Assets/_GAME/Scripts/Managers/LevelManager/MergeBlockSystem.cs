using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public partial class LevelManager
{
    public void CheckBlock(out Dictionary<int, HashSet<SubBlockCtrl>> needSubBlocks)
    {
        needSubBlocks = new();
        var gridWord = ItemManager.Instance.gridWord;
        var blocks = ItemManager.Instance.blocks;
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == null) continue;
            var subBlockCtrls = blocks[i].subBlockCtrls;
            for (int j = 0; j < subBlockCtrls.Length; j++)
            {
                var currentSubBlock = subBlockCtrls[j];
                var currentSubBlockColorIndex = currentSubBlock.ColorIndex;
                var neighbors = blocks[i].gridWord.FindNeighborAt(j);
                for (int k = 0; k < neighbors.Length; k++)
                {
                    if (gridWord.IsPosOutsideAt(neighbors[k])) continue;
                    var blockIndex = gridWord.ConvertWorldPosToIndex(neighbors[k]);
                    var block = blocks[blockIndex];
                    if (block == null) continue;
                    var subBlockIndex = block.gridWord.ConvertWorldPosToIndex(neighbors[k]);
                    var neighborSubBlock = block.subBlockCtrls[subBlockIndex];
                    if (currentSubBlock == neighborSubBlock) continue;
                    var neighborSubBlockColorIndex = neighborSubBlock.ColorIndex;
                    if (neighborSubBlockColorIndex != currentSubBlockColorIndex) continue;
                    if (!needSubBlocks.ContainsKey(currentSubBlockColorIndex))
                    {
                        HashSet<SubBlockCtrl> subBlocks = new();
                        needSubBlocks.Add(currentSubBlockColorIndex, subBlocks);
                    }
                    needSubBlocks[currentSubBlockColorIndex].Add(currentSubBlock);
                    needSubBlocks[neighborSubBlockColorIndex].Add(neighborSubBlock);
                }
            }
        }
    }

    public void RemoveSubBlock(Dictionary<int, HashSet<SubBlockCtrl>> needSubBlocks)
    {
        foreach (var subBlocks in needSubBlocks.Values)
        {
            foreach (var subBlock in subBlocks)
            {
                subBlock.Remove();
            }
        }
    }

    public void RemoveBlock()
    {
        var blocks = ItemManager.Instance.blocks;
        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block == null) continue;
            block.Remove();
        }
    }

    public void UpdateValueBlock()
    {
        var blocks = ItemManager.Instance.blocks;
        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block == null) continue;
            var subBlocks = block.subBlockCtrls;
            for (int j = 0; j < subBlocks.Length; j++)
            {
                var subBlock = subBlocks[j];
                if (subBlock != null) continue;
                var neighbors = block.gridWord.FindNeighborAt(j);
                var needSubBlock = FindNeedSubBlock(neighbors, block);
                needSubBlock.AddIndex(j);
                block.subBlockCtrls[j] = needSubBlock;
                block.gridWord.SetValueAt(j, needSubBlock.ColorIndex);
            }
        }
    }

    SubBlockCtrl FindNeedSubBlock(float3[] neighbors, BlockCtrl block)
    {
        var gridWord = block.gridWord;
        var subBlockCtrls = block.subBlockCtrls;
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (gridWord.IsPosOutsideAt(neighbors[i])) continue;
            var neighborIndex = gridWord.ConvertWorldPosToIndex(neighbors[i]);
            var neighborSubBlock = subBlockCtrls[neighborIndex];
            if (neighborSubBlock == null) continue;
            if (neighborSubBlock.Lst_Index.Count != 1) continue;
            return neighborSubBlock;
        }
        for (int j = 0; j < block.subBlockCtrls.Length; j++)
        {
            if (subBlockCtrls[j] == null) continue;
            return subBlockCtrls[j];
        }
        return null;
    }

    public void UpdateSizeBlock()
    {
        var blocks = ItemManager.Instance.blocks;
        for (int i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block == null) continue;
            block.SetSizeSubBlocks();
        }
    }
}
