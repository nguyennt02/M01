using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager : MonoBehaviour
{
    [Header("Base Item")]
    public static ItemManager Instance { get; private set; }
    [SerializeField] GridWord girdWordPref;
    [SerializeField] Transform _gridWordParent;
    public Transform GridWordParent { get => _gridWordParent; }
    public GridWord gridWord {get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void InitGird(LevelDesignObject data)
    {
        gridWord = Instantiate(girdWordPref, _gridWordParent);
        gridWord.InitGridWord(data.gridSize, data.scale, data.centerPos);
        gridWord.InitGridStatus();
    }
}
