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
    List<AvailableBlockCtrl> _availableBlocks = new();

    public void InitAvailableBlock(LevelDesignObject data)
    {
        if (_availableBlocks.Count > 0) return;
        foreach (Transform pos in _availableSpawnPos)
        {
            if (pos.name.Equals($"{data.amountBlock}BlockPos"))
            {
                var block = SpawnAvailableBlock(pos.position, data);
                _availableBlocks.Add(block);
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

    public void RemoveAvailableBlockOfList(AvailableBlockCtrl availableBlock)
    {
        _availableBlocks.Remove(availableBlock);
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