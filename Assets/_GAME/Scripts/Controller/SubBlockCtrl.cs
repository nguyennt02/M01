using Unity.Mathematics;
using UnityEngine;

public class SubBlockCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;
    public float3 Position { get; private set; }
    public float2 Size { get; private set; }
    public void Setup(float3 pos, float2 size)
    {
        if(size.Equals(float2.zero) || pos.Equals(float2.zero))
            Debug.LogError("du lieu bi loi");
            
        Position = pos;
        Size = size;
        transform.position = pos;
        SetSize(size);
    }

    public void SetSize(Vector2 size)
    {
        renderer.drawMode = SpriteDrawMode.Tiled;
        renderer.size = size;
    }
}
