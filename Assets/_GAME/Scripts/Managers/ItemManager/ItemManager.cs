using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager : MonoBehaviour
{
    [Header("Base Item")]
    public static ItemManager Instance { get; private set; }
    [SerializeField] GirdWord girdWordPref;
    [SerializeField] Transform _girdWordParent;
    public Transform GirdWordParent { get => _girdWordParent; }
    [SerializeField] float3 centerPos;
    GirdWord girdWord;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        LeanTouchManager.Instance.onTouchBegan += OnTouchBegan;
        LeanTouchManager.Instance.onTouchMoved += OnTouchMove;
        LeanTouchManager.Instance.onTouchEnd += OnTouchEnd;
    }

    private void OnDestroy()
    {
        LeanTouchManager.Instance.onTouchBegan -= OnTouchBegan;
        LeanTouchManager.Instance.onTouchMoved -= OnTouchMove;
        LeanTouchManager.Instance.onTouchEnd -= OnTouchEnd;
    }

    public void InitGird()
    {
        var gridSize = new int2(5, 5);
        var scale = new float2(2, 2);
        girdWord = Instantiate(girdWordPref, _girdWordParent);
        girdWord.Setup(gridSize, scale, centerPos);
        girdWord.BakingGridWorld();
    }
}
