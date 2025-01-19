using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
public partial class ItemManager
{
    [SerializeField] Transform _availableBlockParent;
    public Transform AvailableBlockParent { get => _availableBlockParent; }

    int2[] pairs = new int2[4] { new int2(0, 1), new int2(0, 2), new int2(3, 2), new int2(3, 2) };

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

    public BlockCtrl SpawnAvailableBlock(float3 pos)
    {
        var amountSubBlock = RandomAmountSubBlock();
        var colorValues = RandomColorValue(amountSubBlock);
        var blockData = CreateDataBlock(colorValues);
        return SpawnBlock(pos, blockData);
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
}