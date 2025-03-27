using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public partial class LevelManager
{
    public BlockCtrl CurrentAvailableBlock { get; private set; }
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
        if (ItemManager.Instance.gridWord.IsPosOutsideAt(blockPos))
        {
            ReturnAvailableBlock();
        }
        else
        {
            // ItemManager.Instance.SpawnAvailableBlock(CurrentAvailableBlock.Position);
            DropAvailableBlock();
        }
    }

    void GrabAvailableBlockFrom(Collider2D[] colliders)
    {
        var availableBlockParent = ItemManager.Instance.AvailableBlockParent;
        foreach (var block in colliders)
        {
            if (block.transform.parent.Equals(availableBlockParent)
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
        var index = ItemManager.Instance.gridWord.ConvertWorldPosToIndex(blockPos);
        var wordPos = ItemManager.Instance.gridWord.ConvertIndexToWorldPos(index);
        var blocksParent = ItemManager.Instance.BlocksParent;

        CurrentAvailableBlock.transform.position = wordPos;
        CurrentAvailableBlock.transform.SetParent(blocksParent);
        CurrentAvailableBlock.SetPosition(wordPos);
        ItemManager.Instance.blocks[index] = CurrentAvailableBlock;

        CurrentAvailableBlock = null;
    }

    void ReturnAvailableBlock()
    {
        CurrentAvailableBlock.transform.position = CurrentAvailableBlock.Position;
        CurrentAvailableBlock = null;
    }
}