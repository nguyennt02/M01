using Unity.Mathematics;
using UnityEngine;

public class SubBlockCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;
    public int[] Indexs;
    public float3 Position { get; private set; }
    public float2 Size { get; private set; }
    public BlockCtrl Block {get ; private set; }
    public int ColorIndex {get; private set;}
    public void Setup(float3 pos, float2 size, int colorIndex, int[] indexs, BlockCtrl block)
    {
        if(size.Equals(float2.zero) || pos.Equals(float2.zero))
            Debug.Log("du lieu bi loi",gameObject);
            
        Position = pos;
        transform.position = pos;

        Size = size;
        SetSize(size);

        ColorIndex = colorIndex; 
        renderer.color = RendererManager.Instance.GetColorAt(colorIndex);

        Indexs = indexs;

        Block = block;
    }

    public void SetSize(Vector2 size)
    {
        renderer.drawMode = SpriteDrawMode.Tiled;
        renderer.size = size;
    }
}
