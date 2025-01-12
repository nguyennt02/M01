using Unity.Mathematics;
using UnityEngine;

public class BlockCtrl : MonoBehaviour
{
    [SerializeField] SubBlockCtrl subBlockPref;
    public float2 Size { get; private set; }
    public float3 Position { get; private set; }

    public int[] subColorIndexs { get; private set; }
    [SerializeField] Transform _subBlockParents;
    public Transform SubBlockParents { get; private set; }
    SubBlockCtrl[] subBlockCtrls;

    public void InjecValue(float2 size, float3 position)
    {
        Size = size;
        Position = position;
        SetSize(size);
    }

    public void SetSize(float2 size)
    {
        if (TryGetComponent(out BoxCollider2D boxCol))
        {
            boxCol.size = size;
        }
    }

    public void CreateBlock(int[] subColorIndexs)
    {
        this.subColorIndexs = subColorIndexs;
        subBlockCtrls = new SubBlockCtrl[subColorIndexs.Length];
        for (int i = 0; i < subBlockCtrls.Length; i++)
        {
            var pos = ConvertIndexToWordPos(i);
            var subBlock = Instantiate(subBlockPref, _subBlockParents);
            subBlock.transform.localPosition = pos;
            subBlockCtrls[i] = subBlock;
        }
    }

    public int2 ConvertIndexToGridPos(int index)
    {
        int row = index / 2; // Chỉ số hàng (row)
        int col = index % 2; // Chỉ số cột (column)
        return new int2(row, col);
    }

    public float3 ConvertGridPosToWordPos(int2 girdPos)
    {
        float3 pos = new float3(girdPos.x * Size.x / 2, girdPos.y * Size.y / 2, 0);
        float3 offset = new float3(-Size.x, -Size.y, 0) / 4;
        pos += offset;
        return pos;
    }

    public float3 ConvertIndexToWordPos(int index)
    {
        int2 girdPos = ConvertIndexToGridPos(index);
        float3 pos = ConvertGridPosToWordPos(girdPos);
        return pos;
    }

    public int2 ConverWordPosToGirdPos(float3 pos)
    {
        float3 offset = new float3(-Size.x, -Size.y, 0) / 4;
        pos -= offset;
        int x = (int)math.round(pos.x / Size.x * 2);
        int y = (int)math.round(pos.y / Size.y * 2);
        int2 girdPos = new int2(x, y);
        return girdPos;
    }

    public int ConvertGirdPosToIndex(int2 girdPos)
    {
        int index = girdPos.x * 2 + girdPos.y;
        return index;
    }

    public int ConvertWordPosToIndex(float3 pos)
    {
        int2 girdPos = ConverWordPosToGirdPos(pos);
        int index = ConvertGirdPosToIndex(girdPos);
        return index;
    }

}
