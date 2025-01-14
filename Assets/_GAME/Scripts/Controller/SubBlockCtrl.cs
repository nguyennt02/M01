using Unity.Mathematics;
using UnityEngine;

public class SubBlockCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;
    public float3 Position { get; private set; }
    public float2 Size { get; private set; }
    public BlockCtrl Block {get ; private set; }
    public int ColorIndex {get; private set;}
    public void Setup(float3 pos, float2 size, int colorIndex, BlockCtrl block)
    {
        if(size.Equals(float2.zero) || pos.Equals(float2.zero))
            Debug.LogError("du lieu bi loi");
            
        Position = pos;
        transform.position = pos;

        Size = size;
        SetSize(size);

        ColorIndex = colorIndex; 
        renderer.color = RendererManager.Instance.GetColorAt(colorIndex);

        Block = block;
    }

    public void SetSize(Vector2 size)
    {
        renderer.drawMode = SpriteDrawMode.Tiled;
        renderer.size = size;
    }
}
