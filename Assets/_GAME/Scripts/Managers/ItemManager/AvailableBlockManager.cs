using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
public partial class ItemManager
{
    [Header("AvailableBlock")]
    [SerializeField] AvailableBlockCtrl availableBlockPref;
    [SerializeField] Transform _availableSpawnPos;
    public Transform AvailableSpawnPos { get => _availableSpawnPos; }
    [SerializeField] Transform _availableBlockParent;
    public Transform AvailableBlockParent { get => _availableBlockParent; }

    public void InitAvailableBlock(LevelDesignObject data)
    {
        int amountBlock = 2;
        foreach (Transform pos in _availableSpawnPos)
        {
            if (pos.name.Equals($"{amountBlock}BlockPos"))
            {
                SpawnAvailableBlock(pos.position, data);
            }
        }
    }

    public AvailableBlockCtrl SpawnAvailableBlock(float3 pos, LevelDesignObject data)
    {
        var block = Instantiate(availableBlockPref, _availableBlockParent);
        var gridSize = RandomGridSize(data);
        var size = gridWord.scale * gridSize;
        block.InitAvailableBlock(size, pos, gridSize, data);
        return block;
    }

    int2 RandomGridSize(LevelDesignObject data)
    {
        int randomNumber = UnityEngine.Random.Range(0, 101);

        int threshold = data.ratioDoubleAvailableBlock;
        if (randomNumber < threshold)
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 1) return new int2(1, 2);
            else return new int2(2, 1);
        }
        return new int2(1, 1);
    }
}