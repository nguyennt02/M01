using System.Collections.Generic;
using UnityEngine;

public partial class ItemManager
{
    // [Header("Merge Block")]

    public void CheckSubBlockMerge(out HashSet<SubBlockCtrl> subBlockMerges)
    {
        subBlockMerges = new();
        foreach (Transform block in _blocksParent)
        {
            if (block.gameObject.TryGetComponent(out BlockCtrl blockCtrl))
            {
                var amountSubBlock = 4;
                for (int i = 0; i < amountSubBlock; i++)
                {
                    var subBlockPos = blockCtrl.girdWord.ConvertIndexToWorldPos(i);
                    var positions = blockCtrl.girdWord.FindPosNeighborAt(subBlockPos);
                    foreach (var pos in positions)
                    {
                        if (girdWord.IsPosOccupiedAt(pos) && !girdWord.IsPosOutsideAt(pos))
                        {
                            var neighborBlockIndex = girdWord.ConvertWorldPosToIndex(pos);
                            var neighborBlock = blocks[neighborBlockIndex];
                            if (neighborBlock.Equals(blockCtrl)) continue;
                            var subBlockIndex = neighborBlock.girdWord.ConvertWorldPosToIndex(pos);
                            if (neighborBlock.subBlockCtrls[subBlockIndex].ColorIndex == blockCtrl.subBlockCtrls[i].ColorIndex)
                            {
                                subBlockMerges.Add(blockCtrl.subBlockCtrls[i]);
                                subBlockMerges.Add(neighborBlock.subBlockCtrls[subBlockIndex]);
                            }
                        }
                    }
                }
            }
        }
    }

    public void RemoveSubBlockMerge(HashSet<SubBlockCtrl> subBlockMerges)
    {
        foreach (var subBlock in subBlockMerges)
        {
            subBlock.gameObject.SetActive(false);
        }
    }
}