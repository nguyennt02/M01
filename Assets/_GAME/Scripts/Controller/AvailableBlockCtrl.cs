using Unity.Mathematics;
using UnityEngine;

public class AvailableBlockCtrl : MonoBehaviour, IItem
{
    [SerializeField] GridWord gridWordPref;
    [SerializeField] Transform _gridWordParent;
    public Transform GridWordParent { get => _gridWordParent; }
    public float2 Size { get; private set; }
    public float3 Position { get; private set; }
    [SerializeField] Transform _blockParent;
    public Transform BlockParent { get; private set;}

    public GridWord gridWord;

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public int ColorValue()
    {
        throw new System.NotImplementedException();
    }
}
