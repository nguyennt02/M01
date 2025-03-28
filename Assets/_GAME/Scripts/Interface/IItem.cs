using Unity.Mathematics;

public interface IItem
{
    public void Initialize(float2 size, float3 position, LevelDesignObject data);
    public void Drop(float3 wordPos, int index);
    public float3 CurrentPosition {get;}
    public int ColorValue();
}