using Unity.Mathematics;
using UnityEngine;

public class InitializeRainbowCtrl : MonoBehaviour, IInitialize
{
    public void Initialize(float2 size, float3 position, LevelDesignObject data)
    {
        if (TryGetComponent(out BlockCtrl block))
        {
            var subColorIndexs = new int[4] {8,8,8,8};
            block.InitBlock(size, position, subColorIndexs);
        }
    }
}
