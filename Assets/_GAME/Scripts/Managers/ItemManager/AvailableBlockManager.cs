using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
public partial class ItemManager
{
    [Header("AvailableBlock")]
    [SerializeField] Transform _availableSpawnPos;
    [SerializeField] Transform _availableBlockParent;
    public Transform AvailableSpawnPos { get => _availableSpawnPos; }
    public Transform AvailableBlockParent { get => _availableBlockParent; }

    int2[] pairs = new int2[4] { new int2(0, 1), new int2(0, 2), new int2(3, 2), new int2(3, 2) };
    public BlockCtrl CurrentAvailableBlock { get; private set; }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public struct Ratio
    {
        public int ratio1SubBlock;
        public int ratio2SubBlock;
        public int ratio3SubBlock;
    }

    public void InitAvailableBlock()
    {
        int amountBlock = 2;
        foreach (Transform pos in _availableSpawnPos)
        {
            if (pos.name.Equals($"{amountBlock}BlockPos"))
            {
                SpawnAvailableBlock(pos.position);
            }
        }
    }

    public BlockCtrl SpawnAvailableBlock(float3 pos)
    {
        var amountSubBlock = RandomAmountSubBlock();
        var colorValues = RandomColorValue(amountSubBlock);
        var subBlockIndexs = CreateDataBlock(colorValues);

        var block = Instantiate(blockPref, _availableBlockParent);
        block.transform.position = pos;
        block.Setup(girdWord.Scale, pos, subBlockIndexs);
        return block;
    }

    int RandomAmountSubBlock()
    {
        int randomNumber = UnityEngine.Random.Range(0, 101);
        var ratio = GetRatioAt(Difficulty.Easy);

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

    int[] RandomColorValue(int amountSubBlock)
    {
        int[] colors = new int[6] { 0, 0, 4, 1, 2, 3 };

        if (amountSubBlock > colors.Distinct().Count())
        {
            throw new ArgumentException("amountSubBlock không được lớn hơn số lượng phần tử duy nhất trong colors.");
        }

        HashSet<int> selectedColors = new HashSet<int>();

        for (int i = 0; i < amountSubBlock; i++)
        {
            while (true)
            {
                int randomIndex = UnityEngine.Random.Range(0, colors.Length);
                int randomColor = colors[randomIndex];

                // Kiểm tra xem màu đã được chọn chưa
                if (!selectedColors.Contains(randomColor))
                {
                    selectedColors.Add(randomColor);
                    break;
                }
            }
        }

        return selectedColors.ToArray();
    }


    int[] CreateDataBlock(int[] colorValues)
    {
        var amountSubBlock = colorValues.Length;
        if (amountSubBlock == 1)
        {
            return Spawn1SubBlock(colorValues);
        }
        if (amountSubBlock == 2)
        {
            return Spawn2SubBlock(colorValues);
        }
        if (amountSubBlock == 3)
        {
            return Spawn3SubBlock(colorValues);
        }

        else return colorValues;
    }

    int[] Spawn1SubBlock(int[] colorvalues)
    {
        int[] data = new int[4];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = colorvalues[0];
        }
        return data;
    }

    int[] Spawn2SubBlock(int[] colorValues)
    {
        int[] data = new int[4];
        int randomNumber = UnityEngine.Random.Range(0, pairs.Length);
        int2 pair = pairs[randomNumber];
        for (int i = 0; i < data.Length; i++)
        {
            if (i == pair.x) data[i] = colorValues[0];
            else if (i == pair.y) data[i] = colorValues[0];
            else data[i] = colorValues[1];
        }
        return data;
    }

    int[] Spawn3SubBlock(int[] colorValues)
    {
        int[] data = new int[4];
        List<int> colors = new List<int>(colorValues);

        int randomNumber = UnityEngine.Random.Range(0, pairs.Length);
        int2 pair = pairs[randomNumber];

        for (int i = 0; i < data.Length; i++)
        {
            if (i == pair.x || i == pair.y)
            {
                data[i] = colors[0];
            }
            else
            {
                data[i] = colors[1];
                colors.RemoveAt(1);
            }
        }
        return data;
    }

    void OnTouchBegan(float2 position, Collider2D[] colliders)
    {
        GrabAvailableBlockFrom(colliders);
    }

    void OnTouchMove(float2 position, float2 direction)
    {
        if (CurrentAvailableBlock == null) return;
        MoveAvailableBlockAt(position);
    }

    void OnTouchEnd(float2 position, float2 direction)
    {
        if (CurrentAvailableBlock == null) return;
        var blockPos = CurrentAvailableBlock.transform.position;
        if (girdWord.IsPosOccupiedAt(blockPos))
        {
            ReturnAvailableBlock();
        }
        else
        {
            SpawnAvailableBlock(CurrentAvailableBlock.Position);
            DropAvailableBlock();
        }
    }

    void GrabAvailableBlockFrom(Collider2D[] colliders)
    {
        foreach (var block in colliders)
        {
            if (block.transform.parent.Equals(_availableBlockParent)
            && block.TryGetComponent(out BlockCtrl blockCtrl))
            {
                CurrentAvailableBlock = blockCtrl;
            }
        }
    }

    void MoveAvailableBlockAt(float2 pos)
    {
        var targetPos = new float3(pos.x, pos.y, 0);
        targetPos.y += 1;

        CurrentAvailableBlock.transform.position = targetPos;
    }

    void DropAvailableBlock()
    {
        var blockPos = CurrentAvailableBlock.transform.position;
        var index = girdWord.ConvertWorldPosToIndex(blockPos);
        var wordPos = girdWord.ConvertIndexToWorldPos(index);

        CurrentAvailableBlock.transform.position = wordPos;
        CurrentAvailableBlock.transform.SetParent(_blocksParent);
        blocks[index] = CurrentAvailableBlock;

        var value = girdWord.GetFullValue();
        girdWord.SetValueAt(wordPos, value);

        CurrentAvailableBlock = null;
    }

    void ReturnAvailableBlock()
    {
        CurrentAvailableBlock.transform.position = CurrentAvailableBlock.Position;
        CurrentAvailableBlock = null;
    }
}