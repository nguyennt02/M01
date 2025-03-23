using Unity.Mathematics;
using UnityEngine;

public class FloorBlockCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRdr;

    public void InitFloor(Vector2 size)
    {
        spriteRdr.drawMode = SpriteDrawMode.Sliced;
        spriteRdr.size = size;
    }
}
