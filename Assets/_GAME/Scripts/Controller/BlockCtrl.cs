using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BlockCtrl : MonoBehaviour
{
    [SerializeField] GirdWord girdWordPref;
    [SerializeField] Transform _girdWordParent;
    public Transform GirdWordParent { get => _girdWordParent; }
    [SerializeField] SubBlockCtrl subBlockPref;
    public float2 Size { get; private set; }
    public float3 Position { get; private set; }
    public int[] subColorIndexs { get; private set; }
    [SerializeField] Transform _subBlockParents;
    public Transform SubBlockParents { get => _subBlockParents; }
    SubBlockCtrl[] subBlockCtrls;
    GirdWord girdWord;

    public void Setup(float2 size, float3 position, int[] subColorIndexs)
    {
        this.subColorIndexs = subColorIndexs;
        Size = size;
        Position = position;
        SetSize(size);
        InitGird();
        InitSubBlock(subColorIndexs);
    }

    void InitGird()
    {
        var gridSize = new int2(2, 2);
        var size = new float2(Size.x / gridSize.x, Size.y / gridSize.y);
        girdWord = Instantiate(girdWordPref, _girdWordParent);
        girdWord.Setup(gridSize, size, Position);
        girdWord.BakingGridWorld();
    }

    void SetSize(float2 size)
    {
        if (TryGetComponent(out BoxCollider2D boxCol))
        {
            boxCol.size = size;
        }
    }

    void InitSubBlock(int[] subColorIndexs)
    {
        subBlockCtrls = new SubBlockCtrl[subColorIndexs.Length];
        var subBlockColors = ConverArrayToDirection(subColorIndexs);

        foreach (var subBlockData in subBlockColors)
        {
            GetSubBlockDataAt(subBlockData.Value, out float3 pos, out float2 size);
            var subBlock = SpawnSubBlock(pos, size, subBlockData.Key);
            SetValueAt(subBlockData, subBlock);
        }
    }

    Dictionary<int, List<int>> ConverArrayToDirection(int[] arr)
    {
        Dictionary<int, List<int>> subBlockColors = new();
        for (int i = 0; i < arr.Length; i++)
        {
            if (subBlockColors.ContainsKey(arr[i]))
            {
                subBlockColors[arr[i]].Add(i);
            }
            else
            {
                List<int> indexs = new();
                indexs.Add(i);

                subBlockColors.Add(arr[i], indexs);
            }
        }
        return subBlockColors;
    }

    void GetSubBlockDataAt(List<int> indexs, out float3 pos, out float2 size)
    {
        pos = float3.zero;
        size = float2.zero;
        if (indexs.Count == 1)
        {
            pos = girdWord.ConvertIndexToWorldPos(indexs[0]);
            size = girdWord.Scale;
        }
        else if (indexs.Count == 2)
        {
            var pos1 = girdWord.ConvertIndexToWorldPos(indexs[0]);
            var pos2 = girdWord.ConvertIndexToWorldPos(indexs[1]);
            pos = (pos1 + pos2) / 2;

            var dir = new Vector2(pos2.x - pos1.x, pos2.y - pos1.y).normalized;
            if (dir.x != 0 && dir.y == 0)
            {
                size = new float2(girdWord.Scale.x * 2, girdWord.Scale.y);
            }
            else if (dir.x == 0 && dir.y != 0)
            {
                size = new float2(girdWord.Scale.x, girdWord.Scale.y * 2);
            }
        }
        else if (indexs.Count == 4)
        {
            pos = Position;
            size = girdWord.Scale * 2;
        }
    }

    SubBlockCtrl SpawnSubBlock(float3 pos, float2 size, int colorIndex)
    {
        var subBlock = Instantiate(subBlockPref, _subBlockParents);
        subBlock.Setup(pos, size, colorIndex, this);
        return subBlock;
    }

    void SetValueAt(KeyValuePair<int, List<int>> subBlockData, SubBlockCtrl subBlock)
    {
        foreach (var index in subBlockData.Value)
        {
            var pos = girdWord.ConvertIndexToWorldPos(index);
            girdWord.SetValueAt(pos, subBlockData.Key);
            subBlockCtrls[index] = subBlock;
        }
    }
}
