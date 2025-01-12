using Unity.Mathematics;
using UnityEngine;

public class BlockCtrl : MonoBehaviour
{
    [SerializeField] GirdWord girdWordPref;
    [SerializeField] Transform _girdWordParent;
    public Transform GirdWordParent { get => _girdWordParent; }
    [SerializeField] SubBlockCtrl subBlockPref;
    public float2 Size { get; private set; }
    public float3 Position { get; private set; }
    public int[] subColorIndexs { get; private set; }
    [SerializeField] Transform _subBlockParents;
    public Transform SubBlockParents { get => _subBlockParents; }
    SubBlockCtrl[] subBlockCtrls;
    GirdWord girdWord;

    public void Setup(float2 size, float3 position, int[] subColorIndexs)
    {
        this.subColorIndexs = subColorIndexs;
        Size = size;
        Position = position;
        SetSize(size);
        InitGird();
        CreateBlock(subColorIndexs);
    }

    void InitGird()
    {
        var gridSize = new int2(2, 2);
        var size = new float2(Size.x / gridSize.x, Size.y / gridSize.y);
        girdWord = Instantiate(girdWordPref, _girdWordParent);
        girdWord.Setup(gridSize, size, Position);
        girdWord.BakingGridWorld();
    }

    void SetSize(float2 size)
    {
        if (TryGetComponent(out BoxCollider2D boxCol))
        {
            boxCol.size = size;
        }
    }

    void CreateBlock(int[] subColorIndexs)
    {
        subBlockCtrls = new SubBlockCtrl[subColorIndexs.Length];
        for (int i = 0; i < subBlockCtrls.Length; i++)
        {
            var pos = girdWord.ConvertIndexToWorldPos(i);
            var subBlock = Instantiate(subBlockPref, _subBlockParents);
            subBlock.transform.position = pos;
            subBlockCtrls[i] = subBlock;
        }
    }
}
