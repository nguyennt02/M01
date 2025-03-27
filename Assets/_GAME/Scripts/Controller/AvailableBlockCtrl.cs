using Unity.Mathematics;
using UnityEngine;

public class AvailableBlockCtrl : MonoBehaviour
{
    [SerializeField] GridWord gridWordPref;
    [SerializeField] Transform _gridWordParent;
    public Transform GridWordParent { get => _gridWordParent; }
    [SerializeField] Transform _availableBlockParent;
    public Transform AvailableBlockParent { get => _availableBlockParent; }
    [SerializeField] GameObject[] itemPrefs;
    public float2 Size { get; private set; }
    public float3 Position { get; private set; }
    IItem[] items;
    GridWord gridWord;

    public void InitAvailableBlock(float2 size, float3 position, int2 gridSize, LevelDesignObject data)
    {
        SetSize(size);
        SetPosition(position);
        InitGrid(gridSize);
        InitBlock(data);
    }

    public void SetSize(float2 size)
    {
        if (TryGetComponent(out BoxCollider2D collider2D))
        {
            Size = size;
            collider2D.size = size;
        }
    }

    public void SetPosition(float3 position)
    {
        Position = position;
        transform.position = position;
    }

    void InitGrid(int2 gridSize)
    {
        var size = new float2(Size.x / gridSize.x, Size.y / gridSize.y);
        gridWord = Instantiate(gridWordPref, _gridWordParent);
        gridWord.InitGridWord(gridSize, size, Position);
    }

    void InitBlock(LevelDesignObject data)
    {
        var length = gridWord.gridSize.x * gridWord.gridSize.y;
        items = new IItem[length];
        for (int i = 0; i < items.Length; i++)
        {
            var randomItem = RandomItem(data);
            var item = Instantiate(randomItem, _availableBlockParent);
            if (item.TryGetComponent(out IItem itemControl))
            {
                var pos = gridWord.ConvertIndexToWorldPos(i);
                itemControl.Initialize(gridWord.scale, pos, data);
                items[i] = itemControl;
            }
        }
    }

    GameObject RandomItem(LevelDesignObject data)
    {
        int randomNumber = UnityEngine.Random.Range(0, 101);
        int threshold = 0;
        for (int i = 0; i < data.availableBlocks.Length; i++)
        {
            threshold += data.availableBlocks[i].ratio;
            if (randomNumber < threshold) 
                return itemPrefs[data.availableBlocks[i].GRIDSTATE];
        }
        return itemPrefs[data.availableBlocks[0].GRIDSTATE];
    }
}
