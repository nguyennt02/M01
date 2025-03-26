using Nenn.InspectorEnhancements.Runtime.Attributes.Conditional;
using UnityEngine;

public class GridStateControl : MonoBehaviour
{
    [SerializeField] Color[] gridColors;
    [SerializeField] SpriteRenderer spriteRdr;
    public void SetSize(Vector2 size)
    {
        spriteRdr.drawMode = SpriteDrawMode.Sliced;
        spriteRdr.size = size;
    }

    public GRIDSTATE GRIDSTATE;
    void OnValidate()
    {
        spriteRdr.color = gridColors[(int)GRIDSTATE];
    }

    public int[] ColorIndexs = new int[4];
    public void SetGridState(GRIDSTATE GRIDSTATE)
    {
        this.GRIDSTATE = GRIDSTATE;
        spriteRdr.color = gridColors[(int)GRIDSTATE];
    }
}
