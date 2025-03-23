using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class BlockCtrl : MonoBehaviour, IinitializeItem
{
    [SerializeField] GridWord gridWordPref;
    [SerializeField] Transform _gridWordParent;
    public Transform GridWordParent { get => _gridWordParent; }
    public float2 Size { get; private set; }
    public float3 Position { get; private set; }
    [SerializeField] SubBlockCtrl subBlockPref;
    public int[] SubColorIndexs { get; private set; }
    [SerializeField] Transform _subBlockParents;
    public Transform SubBlockParents { get => _subBlockParents; }
    [SerializeField] Transform _subBlockRemoveParent;
    public Transform SubBlockRemoveParent { get => _subBlockRemoveParent; }

    public int ColorValue => gridWord.EmptyValue;

    public SubBlockCtrl[] subBlockCtrls;
    public GridWord gridWord;

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public void InitBlock(float2 size, float3 position, int[] subColorIndexs)
    {
        SetSize(size);
        SetPosition(position);

        InitGrid();
        this.SubColorIndexs = subColorIndexs;
        InitSubBlock(subColorIndexs);
        SetSizeSubBlocks();
    }

    public void SetSize(float2 size)
    {
        if (TryGetComponent(out BoxCollider2D boxCol))
        {
            Size = size;
            boxCol.size = size;
        }
    }

    public void SetPosition(float3 pos)
    {
        Position = pos;
        transform.position = pos;
    }

    void InitGrid()
    {
        var gridSize = new int2(2, 2);
        var size = new float2(Size.x / gridSize.x, Size.y / gridSize.y);
        gridWord = Instantiate(gridWordPref, _gridWordParent);
        gridWord.InitGridWord(gridSize, size, Position);
    }

    void InitSubBlock(int[] subColorIndexs)
    {
        subBlockCtrls = new SubBlockCtrl[subColorIndexs.Length];
        for (int i = 0; i < subBlockCtrls.Length; i++)
        {
            if (subBlockCtrls[i] == null)
            {
                var subBlockValue = subColorIndexs[i];
                var neighbors = gridWord.FindNeighborAt(i);
                for (int j = 0; j < neighbors.Length; j++)
                {
                    if (gridWord.IsPosOutsideAt(neighbors[j])) continue;
                    var neighborIndex = gridWord.ConvertWorldPosToIndex(neighbors[j]);
                    if (subBlockCtrls[neighborIndex] == null) continue;
                    var neighborValue = subColorIndexs[neighborIndex];
                    if (neighborValue != subBlockValue) continue;
                    subBlockCtrls[i] = subBlockCtrls[neighborIndex];
                    subBlockCtrls[i].AddIndex(i);
                    break;
                }
                if (subBlockCtrls[i] != null) continue;
                var pos = gridWord.ConvertIndexToWorldPos(i);
                var colorIndex = subColorIndexs[i];
                subBlockCtrls[i] = SpawnSubBlock(pos, gridWord.scale, colorIndex);
                subBlockCtrls[i].AddIndex(i);
            }
        }
    }

    SubBlockCtrl SpawnSubBlock(float3 pos, float2 size, int colorIndex)
    {
        var subBlock = Instantiate(subBlockPref, _subBlockParents);
        subBlock.InitSubBlock(pos, size, colorIndex, this);
        return subBlock;
    }

    void SetSizeSubBlocks()
    {
        HashSet<SubBlockCtrl> subBlocks = ConvertArrayToHashSet(subBlockCtrls);
        foreach (var subBlock in subBlocks)
        {
            SetSizeSubBlock(subBlock);
        }
    }

    void SetSizeSubBlock(SubBlockCtrl subBlock)
    {
        var lst_Index = subBlock.Lst_Index;
        var pos = GetPosSubBlock(lst_Index);
        var distance = new float2(math.abs(pos.x - subBlock.Position.x), math.abs(pos.y - subBlock.Position.y));
        var size = subBlock.Size + distance * 2;
        subBlock.SetPosition(pos);
        subBlock.SetSize(size);
    }

    HashSet<SubBlockCtrl> ConvertArrayToHashSet(SubBlockCtrl[] arr_subBlocks)
    {
        HashSet<SubBlockCtrl> subBlocks = new();
        foreach (var subBlock in arr_subBlocks)
        {
            subBlocks.Add(subBlock);
        }
        return subBlocks;
    }

    float3 GetPosSubBlock(List<int> lst_index)
    {
        float3 sum = 0;
        foreach (var index in lst_index)
        {
            var pos = gridWord.ConvertIndexToWorldPos(index);
            sum += pos;
        }
        return sum / lst_index.Count;
    }

    // void InitSubBlock(int[] subColorIndexs)
    // {
    //     subBlockCtrls = new SubBlockCtrl[subColorIndexs.Length];
    //     var subBlockColors = ConverArrayToDirection(subColorIndexs);

    //     foreach (var subBlockData in subBlockColors)
    //     {
    //         GetSubBlockDataAt(subBlockData.Value, out float3 pos, out float2 size);
    //         var subBlock = SpawnSubBlock(pos, size, subBlockData);
    //         SetValueAt(subBlockData, subBlock);
    //     }
    // }

    // Dictionary<int, List<int>> ConverArrayToDirection(int[] arr)
    // {
    //     Dictionary<int, List<int>> subBlockColors = new();
    //     for (int i = 0; i < arr.Length; i++)
    //     {
    //         if (subBlockColors.ContainsKey(arr[i]))
    //         {
    //             subBlockColors[arr[i]].Add(i);
    //         }
    //         else
    //         {
    //             List<int> indexs = new();
    //             indexs.Add(i);

    //             subBlockColors.Add(arr[i], indexs);
    //         }
    //     }
    //     return subBlockColors;
    // }

    // void GetSubBlockDataAt(List<int> indexs, out float3 pos, out float2 size)
    // {
    //     pos = float3.zero;
    //     size = float2.zero;
    //     if (indexs.Count == 1)
    //     {
    //         pos = girdWord.ConvertIndexToWorldPos(indexs[0]);
    //         size = girdWord.Scale;
    //     }
    //     else if (indexs.Count == 2)
    //     {
    //         var pos1 = girdWord.ConvertIndexToWorldPos(indexs[0]);
    //         var pos2 = girdWord.ConvertIndexToWorldPos(indexs[1]);
    //         pos = (pos1 + pos2) / 2;

    //         var dir = new Vector2(pos2.x - pos1.x, pos2.y - pos1.y).normalized;
    //         if (dir.x != 0 && dir.y == 0)
    //         {
    //             size = new float2(girdWord.Scale.x * 2, girdWord.Scale.y);
    //         }
    //         else if (dir.x == 0 && dir.y != 0)
    //         {
    //             size = new float2(girdWord.Scale.x, girdWord.Scale.y * 2);
    //         }
    //     }
    //     else if (indexs.Count == 4)
    //     {
    //         pos = Position;
    //         size = girdWord.Scale * 2;
    //     }
    // }

    // void SetValueAt(KeyValuePair<int, List<int>> subBlockData, SubBlockCtrl subBlock)
    // {
    //     foreach (var index in subBlockData.Value)
    //     {
    //         var pos = girdWord.ConvertIndexToWorldPos(index);
    //         girdWord.SetValueAt(pos, subBlockData.Key);
    //         subBlockCtrls[index] = subBlock;
    //     }
    // }

    // public void UpdateBlock()
    // {
    //     if (_subBlockParents.childCount == 0)
    //     {
    //         var parent = ItemManager.Instance.BlockRemovesParent;
    //         transform.SetParent(parent);
    //         gameObject.SetActive(false);
    //         var girdWord = ItemManager.Instance.gridWord;
    //         var index = girdWord.ConvertWorldPosToIndex(Position);
    //         ItemManager.Instance.blocks[index] = null;
    //         var emptyValue = girdWord.GetEmptyValue();
    //         girdWord.SetValueAt(index, emptyValue);
    //         return;
    //     }
    //     var subBlockColors = ConverArrayToDirection(SubColorIndexs);
    //     foreach (var subBlockData in subBlockColors)
    //     {
    //         GetSubBlockDataAt(subBlockData.Value, out float3 pos, out float2 size);
    //         var subBlock = subBlockCtrls[subBlockData.Value[0]];
    //         subBlock.UpdateSubBlock(pos, size, subBlockData.Value.ToArray());
    //     }
    // }
}
