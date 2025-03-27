using Unity.Mathematics;

public interface IItem
{
    public int ColorValue();
    public void Initialize(float2 size, float3 position, LevelDesignObject data);
}