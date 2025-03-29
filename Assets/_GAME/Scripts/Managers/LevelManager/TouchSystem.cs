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
        UpdatePosGridWordOfBlock();
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
            CheckBlocks(out Dictionary<int, HashSet<SubBlockCtrl>> needSubBlocks);
            if (needSubBlocks.Count == 0) return;
            RemoveSubBlock(needSubBlocks);
            RemoveBlock();
            UpdateValueBlock();
            UpdateSizeBlock();
        }
    }

    void UpdatePosGridWordOfBlock()
    {
        var gridWord = ItemManager.Instance.gridWord;
        var items = CurrentAvailableBlock.items;
        foreach (var item in items)
        {
            var blockPos = item.transform.position;
            if (item.TryGetComponent(out BlockCtrl block))
            {
                if (!gridWord.IsPosOccupiedAt(blockPos))
                {
                    var index = gridWord.ConvertWorldPosToIndex(blockPos);
                    var wordPos = gridWord.ConvertIndexToWorldPos(index);
                    block.ChangePosGridWord(wordPos);
                    FindBlock(out Dictionary<int, HashSet<SubBlockCtrl>> needSubBlocks);
                    ScaleSubBlock(needSubBlocks);
                }
                else
                {
                    block.ReturnGridWord();
                }
            }
        }
    }



    void FindBlock(out Dictionary<int, HashSet<SubBlockCtrl>> needSubBlocks)
    {
        needSubBlocks = new();
        var blocks = ItemManager.Instance.blocks;
        var gridWord = ItemManager.Instance.gridWord;
        var items = CurrentAvailableBlock.items;
        foreach (var item in items)
        {
            if (item.TryGetComponent(out BlockCtrl block))
            {
                CheckBlock(out needSubBlocks, block, blocks, gridWord);
            }
        }
    }
}