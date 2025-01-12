using Unity.Mathematics;
using UnityEngine;

public class FloorBlockCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;

    public void Setup(Vector2 size)
    {
        renderer.drawMode = SpriteDrawMode.Tiled;
        renderer.size = size;
    }
}
