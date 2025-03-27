using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager
{
    [Header("FloorBlock")]
    [SerializeField] FloorBlockCtrl floorBlockPref;
    [SerializeField] Transform _floorBlocksParent;
    public Transform FloorBlocksParent { get => _floorBlocksParent; }
    FloorBlockCtrl[] floorBlocks;

    public void InitFloorBlock(LevelDesignObject data)
    {
        floorBlocks = new FloorBlockCtrl[data.grids.Length];
        for (int i = 0; i < floorBlocks.Length; i++)
        {
            if (data.grids[i].GRIDSTATE == (int)GRIDSTATE.REMOVE) continue;
            var pos = gridWord.ConvertIndexToWorldPos(i);
            floorBlocks[i] = SpawnFloorBlockAt(pos);
            floorBlocks[i].InitFloor(gridWord.scale);
        }
    }

    FloorBlockCtrl SpawnFloorBlockAt(float3 pos)
    {
        var obj = Instantiate(floorBlockPref, _floorBlocksParent);
        obj.transform.position = pos;
        return obj;
    }
}