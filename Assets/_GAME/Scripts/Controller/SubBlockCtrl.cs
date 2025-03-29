using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SubBlockCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRdr;
    public float3 Position => transform.position;
    public float2 Size { get; private set; }
    public BlockCtrl BlockParent { get; private set; }
    public int ColorIndex { get; private set; }
    public List<int> Lst_Index { get; private set; }
    public void InitSubBlock(float3 pos, float2 size, int colorIndex, BlockCtrl block)
    {
        BlockParent = block;
        Lst_Index = new();

        SetPosition(pos);
        SetSize(size);
        SetRenderer(colorIndex);
    }

    public void SetSize(Vector2 size)
    {
        Size = size;
        spriteRdr.drawMode = SpriteDrawMode.Tiled;
        spriteRdr.size = size;
    }

    public void SetPosition(float3 pos)
    {
        transform.position = pos;
    }

    public void SetRenderer(int colorIndex)
    {
        ColorIndex = colorIndex;
        spriteRdr.color = RendererManager.Instance.GetColorAt(colorIndex);
    }

    public void AddIndex(int index)
    {
        Lst_Index.Add(index);
    }

    public void Remove()
    {
        transform.SetParent(BlockParent.SubBlockRemoveParent);
        gameObject.SetActive(false);

        var value = BlockParent.gridWord.EmptyValue;
        for (int i = 0; i < Lst_Index.Count; i++)
        {
            BlockParent.subBlockCtrls[Lst_Index[i]] = null;
            BlockParent.gridWord.SetValueAt(Lst_Index[i], value);
        }
    }
}
