using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public partial class LevelManager
{
    public AvailableBlockCtrl CurrentAvailableBlock { get; private set; }
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
        if (CurrentAvailableBlock.isDrop())
        {
            CurrentAvailableBlock.Drop();
            SpawnAvailableBlock();
            CheckMergeBlock();
        }
        else
        {
            CurrentAvailableBlock.transform.position = CurrentAvailableBlock.StartPosition;
        }
        CurrentAvailableBlock = null;
    }

    void GrabAvailableBlockFrom(Collider2D[] colliders)
    {
        foreach (var block in colliders)
        {
            if (block.TryGetComponent(out AvailableBlockCtrl blockCtrl))
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

    void SpawnAvailableBlock()
    {
        ItemManager.Instance.RemoveAvailableBlockOfList(CurrentAvailableBlock);
        var data = GetCurrentLevelDesignObject();
        ItemManager.Instance.InitAvailableBlock(data);
    }

    void CheckMergeBlock()
    {
        while (true)
        {
            CheckBlock(out Dictionary<int, HashSet<SubBlockCtrl>> needSubBlocks);
            if(needSubBlocks.Count == 0) return;
            RemoveSubBlock(needSubBlocks);
            RemoveBlock();
            UpdateValueBlock();
            UpdateSizeBlock();
        }
    }
}