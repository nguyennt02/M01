using System.Collections.Generic;
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

    public void UpdateBlock()
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
                for (int k = 0; k < neighbors.Length; k++)
                {
                    if(block.gridWord.IsPosOutsideAt(neighbors[k])) continue;
                    var neighborIndex = block.gridWord.ConvertWorldPosToIndex(neighbors[k]);
                    var neighborSubBlock = subBlocks[neighborIndex];
                    if (neighborSubBlock == null) continue;
                    if (neighborSubBlock.Lst_Index.Count != 1) continue;
                    neighborSubBlock.AddIndex(j);
                    block.subBlockCtrls[j] = neighborSubBlock;
                    block.gridWord.SetValueAt(j,neighborSubBlock.ColorIndex);
                }
            }
        }
    }
}
