using Unity.Mathematics;
using UnityEngine;

public class AvailableBlockCtrl : MonoBehaviour
{
    [SerializeField] GridWord gridWordPref;
    [SerializeField] Transform _gridWordParent;
    public Transform GridWordParent { get => _gridWordParent; }
    public float2 Size { get; private set; }
    public float3 Position { get; private set; }
    [SerializeField] SubBlockCtrl subBlockPref;
    public int[] SubColorIndexs { get; private set; }
    [SerializeField] Transform _subBlockParents;
    public Transform SubBlockParents { get => _subBlockParents; }
    [SerializeField] Transform _subBlockRemoveParent;
    public Transform SubBlockRemoveParent { get => _subBlockRemoveParent; }
    public SubBlockCtrl[] subBlockCtrls;
    public GridWord gridWord;
}
