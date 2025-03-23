// using System.Collections.Generic;
// using Unity.Mathematics;
// using UnityEngine;

// public partial class ItemManager
// {
//     // [Header("Merge Block")]

//     public void CheckSubBlockMerge(out HashSet<SubBlockCtrl> subBlockMerges, out HashSet<BlockCtrl> blockMerges)
//     {
//         subBlockMerges = new();
//         blockMerges = new();
//         foreach (Transform block in _blocksParent)
//         {
//             if (block.gameObject.TryGetComponent(out BlockCtrl blockCtrl))
//             {
//                 var amountSubBlock = 4;
//                 for (int i = 0; i < amountSubBlock; i++)
//                 {
//                     var subBlockPos = blockCtrl.girdWord.ConvertIndexToWorldPos(i);
//                     var positions = blockCtrl.girdWord.FindPosNeighborAt(subBlockPos);
//                     foreach (var pos in positions)
//                     {
//                         if (gridWord.IsPosOccupiedAt(pos) && !gridWord.IsPosOutsideAt(pos))
//                         {
//                             var neighborBlockIndex = gridWord.ConvertWorldPosToIndex(pos);
//                             var neighborBlock = blocks[neighborBlockIndex];
//                             if (neighborBlock.Equals(blockCtrl)) continue;
//                             var subBlockIndex = neighborBlock.girdWord.ConvertWorldPosToIndex(pos);
//                             if (neighborBlock.subBlockCtrls[subBlockIndex].ColorIndex == blockCtrl.subBlockCtrls[i].ColorIndex)
//                             {
//                                 subBlockMerges.Add(blockCtrl.subBlockCtrls[i]);
//                                 subBlockMerges.Add(neighborBlock.subBlockCtrls[subBlockIndex]);
//                                 blockMerges.Add(blockCtrl);
//                                 blockMerges.Add(neighborBlock);
//                             }
//                         }
//                     }
//                 }
//             }
//         }
//     }

//     public void RemoveSubBlockMerge(HashSet<SubBlockCtrl> subBlockMerges)
//     {
//         foreach (var subBlock in subBlockMerges)
//         {
//             foreach (var index in subBlock.Indexs)
//             {
//                 var value = subBlock.Block.girdWord.GetEmptyValue();
//                 subBlock.Block.girdWord.SetValueAt(index, value);
//                 subBlock.Block.subBlockCtrls[index] = null;
//                 subBlock.gameObject.SetActive(false);
//                 subBlock.transform.SetParent(subBlock.Block.SubBlockRemoveParent);
//             }
//         }
//     }

//     public void RemoveBlockMerge(HashSet<BlockCtrl> blockMerges)
//     {
//         var amountSubBlock = 4;
//         foreach (var block in blockMerges)
//         {
//             List<int2> subBlocData = new();
//             for (int i = 0; i < amountSubBlock; i++)
//             {
//                 var colorValue = block.girdWord.GetValueAt(i);
//                 var emptyValue = block.girdWord.GetEmptyValue();
//                 if (colorValue == emptyValue)
//                 {
//                     var pos = block.girdWord.ConvertIndexToWorldPos(i);
//                     var neighbors = block.girdWord.FindNeighborAt(pos);
//                     SubBlockCtrl subBlockNeed = null;
//                     foreach (var newPos in neighbors)
//                     {
//                         if (newPos.Equals(new float3(-1, -1, -1))) continue;
//                         var index = block.girdWord.ConvertWorldPosToIndex(newPos);
//                         var subBlock = block.subBlockCtrls[index];
//                         subBlockNeed = subBlock;
//                         if (subBlock.Indexs.Length == 1)
//                         {
//                             subBlockNeed = subBlock;
//                             break;
//                         }
//                     }
//                     if (subBlockNeed)
//                     {
//                         var value = subBlockNeed.ColorIndex;
//                         subBlocData.Add(new int2(i, value));
//                         block.subBlockCtrls[i] = subBlockNeed;
//                     }
//                 }
//             }
//             foreach (var chid in subBlocData)
//             {
//                 block.subColorIndexs[chid.x] = chid.y;
//                 block.girdWord.SetValueAt(chid.x, chid.y);
//             }
//             block.UpdateBlock();
//         }
//     }
// }
