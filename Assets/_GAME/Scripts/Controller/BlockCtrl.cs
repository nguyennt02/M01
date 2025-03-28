using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class BlockCtrl : MonoBehaviour, IDrop
{
    [SerializeField] GridWord gridWordPref;
    [SerializeField] Transform _gridWordParent;
    public Transform GridWordParent { get => _gridWordParent; }
    public int Index { get; private set; }
    public float2 Size { get; private set; }
    public float3 Position { get; private set; }
    [SerializeField] SubBlockCtrl subBlockPref;
    [SerializeField] Transform _subBlockParents;
    public Transform SubBlockParents { get => _subBlockParents; }
    [SerializeField] Transform _subBlockRemoveParent;
    public Transform SubBlockRemoveParent { get => _subBlockRemoveParent; }

    public SubBlockCtrl[] subBlockCtrls;
    public GridWord gridWord;

    public void InitBlock(float2 size, float3 position, int[] subColorIndexs)
    {
        SetSize(size);
        SetPosition(position);

        InitGrid();
        InitSubBlock(subColorIndexs);
        SetSizeSubBlocks();
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    public void SetSize(float2 size)
    {
        Size = size;
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
        gridWord.InitGridStatus();
    }

    void InitSubBlock(int[] subColorIndexs)
    {
        subBlockCtrls = new SubBlockCtrl[subColorIndexs.Length];
        for (int i = 0; i < subBlockCtrls.Length; i++)
        {
            if (subBlockCtrls[i] == null)
            {
                var subBlockValue = subColorIndexs[i];
                gridWord.SetValueAt(i, subBlockValue);
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

    public void SetSizeSubBlocks()
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
        var subBlockPos = subBlock.transform.position;
        var distance = new float2(math.abs(pos.x - subBlockPos.x), math.abs(pos.y - subBlockPos.y));
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

    public void Drop(float3 wordPos, int index)
    {
        SetIndex(index);
        var gridWord = ItemManager.Instance.gridWord;
        var blocksParent = ItemManager.Instance.BlocksParent;
        ItemManager.Instance.blocks[index] = this;
        transform.SetParent(blocksParent);
        SetPosition(wordPos);

        var value = gridWord.EmptyValue + (int)GRIDSTATE.BLOCK;
        gridWord.SetValueAt(wordPos, value);
    }

    public void Remove()
    {
        if (_subBlockParents.childCount == 0)
        {
            ItemManager.Instance.blocks[Index] = null;
            var parent = ItemManager.Instance.BlockRemovesParent;
            transform.SetParent(parent);
            gameObject.SetActive(false);

            var value = ItemManager.Instance.gridWord.EmptyValue;
            ItemManager.Instance.gridWord.SetValueAt(Index, value);
        }
    }
}
