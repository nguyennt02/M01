using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager
{
    [Header("FloorBlock")]
    [SerializeField] FloorBlockCtrl floorBlockPref;
    [SerializeField] Transform _floorBlocksParent;
    public Transform FloorBlocksParent { get => _floorBlocksParent; }
    FloorBlockCtrl[] floorBlocks;

    public void InitFloorBlock()
    {
        var posIndexs = new int[25];
        for (int i = 0; i < 25; i++)
        {
            posIndexs[i] = i;
        }
        var length = gridWord.gridSize.x * gridWord.gridSize.y;
        floorBlocks = new FloorBlockCtrl[length];

        for (int i = 0; i < posIndexs.Length; i++)
        {
            var index = posIndexs[i];
            if (index == -1) continue;

            var pos = gridWord.ConvertIndexToWorldPos(posIndexs[i]);
            floorBlocks[i] = SpawnFloorBlockAt(pos);
            floorBlocks[i].InitFloor(gridWord.scale);

            var value = gridWord.EmptyValue;
            gridWord.SetValueAt(pos, value);
        }
    }

    FloorBlockCtrl SpawnFloorBlockAt(float3 pos)
    {
        var obj = Instantiate(floorBlockPref,_floorBlocksParent);
        obj.transform.position = pos;
        return obj;
    }
}