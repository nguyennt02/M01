using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class BlockCtrl : MonoBehaviour, IItem
{
    [SerializeField] GridWord gridWordPref;
    [SerializeField] Transform _gridWordParent;
    public Transform GridWordParent { get => _gridWordParent; }
    public int Index { get; private set; }
    public float2 Size { get; private set; }
    public float3 Position => transform.position;
    [SerializeField] SubBlockCtrl subBlockPref;
    [SerializeField] Transform _subBlockParents;
    public Transform SubBlockParents { get => _subBlockParents; }
    [SerializeField] Transform _subBlockRemoveParent;
    public Transform SubBlockRemoveParent { get => _subBlockRemoveParent; }

    public SubBlockCtrl[] subBlockCtrls;
    public GridWord gridWord;

    public void Initialize(float2 size, float3 position, LevelDesignObject data)
    {
        Size = size;
        SetPosition(position);
        InitGrid();
        var amountColor = RandomAmountColor((Difficulty)data.difficulty);
        var subColorIndexs = CreateColorValues(amountColor, data.colorValues);
        InitSubBlock(subColorIndexs);
        SetSizeSubBlocks();
    }

    public void InitBlock(float2 size, float3 position, int[] subColorIndexs)
    {
        Size = size;
        SetPosition(position);

        InitGrid();
        InitSubBlock(subColorIndexs);
        SetSizeSubBlocks();
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    public void SetPosition(float3 pos)
    {
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

    int RandomAmountColor(Difficulty difficulty)
    {
        int randomNumber = UnityEngine.Random.Range(0, 101);
        var ratio = GetRatioAt(difficulty);

        int threshold = ratio.ratio1SubBlock;
        if (randomNumber < threshold) return 1;

        threshold += ratio.ratio2SubBlock;
        if (randomNumber < threshold) return 2;

        threshold += ratio.ratio3SubBlock;
        if (randomNumber < threshold) return 3;

        return 4;
    }

    Ratio GetRatioAt(Difficulty difficulty)
    {
        Ratio ratio = new();
        switch (difficulty)
        {
            case Difficulty.Easy:
                ratio.ratio1SubBlock = 40;
                ratio.ratio2SubBlock = 20;
                ratio.ratio3SubBlock = 15;
                break;
            case Difficulty.Medium:
                ratio.ratio1SubBlock = 30;
                ratio.ratio2SubBlock = 20;
                ratio.ratio3SubBlock = 20;
                break;
            case Difficulty.Hard:
                ratio.ratio1SubBlock = 20;
                ratio.ratio2SubBlock = 20;
                ratio.ratio3SubBlock = 20;
                break;
        }
        return ratio;
    }

    public int[] CreateColorValues(int amount, int[] colorValues)
    {
        int[] needColors = NeedColor(colorValues, amount);
        List<int> needColorsClone = new(needColors);
        int[] colors = new int[4];
        for (int i = 0; i < colors.Length; i++)
        {
            if (needColorsClone.Count == 0) needColorsClone = new(needColors);
            var randomIndex = UnityEngine.Random.Range(0, needColorsClone.Count);
            colors[i] = needColorsClone[randomIndex];
            needColorsClone.RemoveAt(randomIndex);
        }
        return colors;
    }

    int[] NeedColor(int[] colorValues, int amount)
    {
        int[] needColors = new int[amount];
        for (int i = 0; i < needColors.Length; i++)
        {
            while (true)
            {
                var randomIndex = UnityEngine.Random.Range(0, colorValues.Length);
                var colorValue = colorValues[randomIndex];
                if (needColors.Contains(colorValue)) continue;
                needColors[i] = colorValue;
                break;
            }
        }
        return needColors;
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
