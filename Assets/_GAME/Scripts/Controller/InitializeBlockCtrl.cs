using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class InitializeBlockCtrl : MonoBehaviour, IInitialize
{

    public void Initialize(float2 size, float3 position, LevelDesignObject data)
    {
        if (TryGetComponent(out BlockCtrl block))
        {
            var amountColor = RandomAmountColor((Difficulty)data.difficulty);
            var subColorIndexs = CreateColorValues(amountColor, data.colorValues);
            block.InitBlock(size, position, subColorIndexs);
        }
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
}
