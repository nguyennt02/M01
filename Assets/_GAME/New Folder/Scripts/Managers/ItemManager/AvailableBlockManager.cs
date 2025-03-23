using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
public partial class ItemManager
{
    [Header("AvailableBlock")]
    [SerializeField] Transform _availableSpawnPos;
    [SerializeField] Transform _availableBlockParent;
    public Transform AvailableSpawnPos { get => _availableSpawnPos; }
    public Transform AvailableBlockParent { get => _availableBlockParent; }

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
        var amountColor = RandomAmountColor();
        var colorValues = CreateColorValues(amountColor);
        var block = SpawnBlock(pos,colorValues, _availableBlockParent);
        return block;
    }

    int RandomAmountColor()
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

    public int[] CreateColorValues(int amount)
    {
        int[] colorValues = new int[6] { 1, 2, 3, 4, 5, 4 };
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

    // int[] CreateColorValueAt(int amountSubBlock)
    // {
    //     int[] colors = new int[6] { 0, 0, 4, 1, 2, 3 };
    //     int[] colorValues = new int[4];
    //     switch (amountSubBlock)
    //     {
    //         case 1:
    //             int randomIndex = UnityEngine.Random.Range(0, colors.Length);
    //             int randomColor = colors[randomIndex];
    //             for (int i = 0; i < colorValues.Length; i++)
    //             {
    //                 colorValues[i] = randomColor;
    //             }
    //             break;
    //         case 2:

    //             break;
    //         case 3:
    //             break;
    //         case 4:
    //             break;
    //     }
    // }

    // int[] RandomColorValue(int amountSubBlock)
    // {
    //     int[] colors = new int[6] { 0, 0, 4, 1, 2, 3 };

    //     if (amountSubBlock > colors.Distinct().Count())
    //     {
    //         throw new ArgumentException("amountSubBlock không được lớn hơn số lượng phần tử duy nhất trong colors.");
    //     }

    //     HashSet<int> selectedColors = new HashSet<int>();

    //     for (int i = 0; i < amountSubBlock; i++)
    //     {
    //         while (true)
    //         {
    //             int randomIndex = UnityEngine.Random.Range(0, colors.Length);
    //             int randomColor = colors[randomIndex];

    //             // Kiểm tra xem màu đã được chọn chưa
    //             if (!selectedColors.Contains(randomColor))
    //             {
    //                 selectedColors.Add(randomColor);
    //                 break;
    //             }
    //         }
    //     }

    //     return selectedColors.ToArray();
    // }


    // int[] CreateDataBlock(int[] colorValues)
    // {
    //     var amountSubBlock = colorValues.Length;
    //     if (amountSubBlock == 1)
    //     {
    //         return Spawn1SubBlock(colorValues);
    //     }
    //     if (amountSubBlock == 2)
    //     {
    //         return Spawn2SubBlock(colorValues);
    //     }
    //     if (amountSubBlock == 3)
    //     {
    //         return Spawn3SubBlock(colorValues);
    //     }

    //     else return colorValues;
    // }

    // int[] Spawn1SubBlock(int[] colorvalues)
    // {
    //     int[] data = new int[4];
    //     for (int i = 0; i < data.Length; i++)
    //     {
    //         data[i] = colorvalues[0];
    //     }
    //     return data;
    // }

    // int[] Spawn2SubBlock(int[] colorValues)
    // {
    //     int[] data = new int[4];
    //     int randomNumber = UnityEngine.Random.Range(0, pairs.Length);
    //     int2 pair = pairs[randomNumber];
    //     for (int i = 0; i < data.Length; i++)
    //     {
    //         if (i == pair.x) data[i] = colorValues[0];
    //         else if (i == pair.y) data[i] = colorValues[0];
    //         else data[i] = colorValues[1];
    //     }
    //     return data;
    // }

    // int[] Spawn3SubBlock(int[] colorValues)
    // {
    //     int[] data = new int[4];
    //     List<int> colors = new List<int>(colorValues);

    //     int randomNumber = UnityEngine.Random.Range(0, pairs.Length);
    //     int2 pair = pairs[randomNumber];

    //     for (int i = 0; i < data.Length; i++)
    //     {
    //         if (i == pair.x || i == pair.y)
    //         {
    //             data[i] = colors[0];
    //         }
    //         else
    //         {
    //             data[i] = colors[1];
    //             colors.RemoveAt(1);
    //         }
    //     }
    //     return data;
    // }

    // void OnTouchBegan(float2 position, Collider2D[] colliders)
    // {
    //     GrabAvailableBlockFrom(colliders);
    // }

    // void OnTouchMove(float2 position, float2 direction)
    // {
    //     if (CurrentAvailableBlock == null) return;
    //     MoveAvailableBlockAt(position);
    // }

    // void OnTouchEnd(float2 position, float2 direction)
    // {
    //     if (CurrentAvailableBlock == null) return;
    //     var blockPos = CurrentAvailableBlock.transform.position;
    //     if (gridWord.IsPosOccupiedAt(blockPos))
    //     {
    //         ReturnAvailableBlock();
    //     }
    //     else
    //     {
    //         SpawnAvailableBlock(CurrentAvailableBlock.Position);
    //         DropAvailableBlock();
    //         CheckSubBlockMerge(out HashSet<SubBlockCtrl> subBlockMerges, out HashSet<BlockCtrl> blockMerges);
    //         RemoveSubBlockMerge(subBlockMerges);
    //         RemoveBlockMerge(blockMerges);
    //     }
    // }

    // void GrabAvailableBlockFrom(Collider2D[] colliders)
    // {
    //     foreach (var block in colliders)
    //     {
    //         if (block.transform.parent.Equals(_availableBlockParent)
    //         && block.TryGetComponent(out BlockCtrl blockCtrl))
    //         {
    //             CurrentAvailableBlock = blockCtrl;
    //         }
    //     }
    // }

    // void MoveAvailableBlockAt(float2 pos)
    // {
    //     var targetPos = new float3(pos.x, pos.y, 0);
    //     targetPos.y += 1;

    //     CurrentAvailableBlock.transform.position = targetPos;
    // }

    // void DropAvailableBlock()
    // {
    //     var blockPos = CurrentAvailableBlock.transform.position;
    //     var index = gridWord.ConvertWorldPosToIndex(blockPos);
    //     var wordPos = gridWord.ConvertIndexToWorldPos(index);

    //     CurrentAvailableBlock.transform.position = wordPos;
    //     CurrentAvailableBlock.transform.SetParent(_blocksParent);
    //     CurrentAvailableBlock.SetPosition(wordPos);
    //     blocks[index] = CurrentAvailableBlock;

    //     var value = gridWord.GetFullValue();
    //     gridWord.SetValueAt(wordPos, value);

    //     CurrentAvailableBlock = null;
    // }

    // void ReturnAvailableBlock()
    // {
    //     CurrentAvailableBlock.transform.position = CurrentAvailableBlock.Position;
    //     CurrentAvailableBlock = null;
    // }
}