using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager
{
    [Header("FloorBlock")]
    [SerializeField] FloorBlockCtrl floorBlockPref;
    [SerializeField] Transform _floorBlocksParent;
    public Transform FloorBlocksParent { get; private set; }
    FloorBlockCtrl[] floorBlocks;

    public void InitFloorBlock()
    {
        var posIndexs = new int[25];
        for (int i = 0; i < 25; i++)
        {
            posIndexs[i] = i;
        }

        for (int i = 0; i < posIndexs.Length; i++)
        {
            var index = posIndexs[i];
            if (index == -1) continue;

            var pos = GirdWordManager.Instance.ConvertIndexToWorldPos(posIndexs[i]);
            floorBlocks[i] = SpawnFloorBlockAt(pos);

            var value = GirdWordManager.Instance.GetEmptyValue();
            GirdWordManager.Instance.SetValueAt(pos, value);
        }
    }

    FloorBlockCtrl SpawnFloorBlockAt(float3 pos)
    {
        var obj = Instantiate(floorBlockPref,_floorBlocksParent);
        obj.transform.position = pos;
        return obj;
    }
}