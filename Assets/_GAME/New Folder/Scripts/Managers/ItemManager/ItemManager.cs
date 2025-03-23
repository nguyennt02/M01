using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager : MonoBehaviour
{
    [Header("Base Item")]
    public static ItemManager Instance { get; private set; }
    [SerializeField] GridWord girdWordPref;
    [SerializeField] Transform _gridWordParent;
    public Transform GridWordParent { get => _gridWordParent; }
    [SerializeField] float3 centerPos;
    public GridWord gridWord {get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void InitGird()
    {
        var gridSize = new int2(5, 5);
        var scale = new float2(2, 2);
        gridWord = Instantiate(girdWordPref, _gridWordParent);
        gridWord.InitGridWord(gridSize, scale, centerPos);
    }
}
