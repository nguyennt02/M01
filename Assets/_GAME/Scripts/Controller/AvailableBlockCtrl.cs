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
    public float3 StartPosition { get; private set; }
    GameObject[] items;
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
        StartPosition = position;
        transform.position = position;
    }

    void InitGrid(int2 gridSize)
    {
        var size = new float2(Size.x / gridSize.x, Size.y / gridSize.y);
        gridWord = Instantiate(gridWordPref, _gridWordParent);
        gridWord.InitGridWord(gridSize, size, StartPosition);
    }

    void InitBlock(LevelDesignObject data)
    {
        var length = gridWord.gridSize.x * gridWord.gridSize.y;
        items = new GameObject[length];
        for (int i = 0; i < items.Length; i++)
        {
            var randomItem = RandomItem(data);
            items[i] = Instantiate(randomItem, _availableBlockParent);
            if (items[i].TryGetComponent(out IInitialize itemControl))
            {
                var pos = gridWord.ConvertIndexToWorldPos(i);
                itemControl.Initialize(gridWord.scale, pos, data);
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
                return itemPrefs[data.availableBlocks[i].BLOCKTYPE];
        }
        return itemPrefs[data.availableBlocks[0].BLOCKTYPE];
    }

    public bool isDrop()
    {
        var gridWord = ItemManager.Instance.gridWord;
        for (int i = 0; i < items.Length; i++)
        {
            var blockPos = items[i].transform.position;
            if (gridWord.IsPosOccupiedAt(blockPos))
                return false;
        }
        return true;
    }

    public void Drop()
    {
        var gridWord = ItemManager.Instance.gridWord;
        for (int i = 0; i < items.Length; i++)
        {
            var blockPos = items[i].transform.position;
            if (!gridWord.IsPosOccupiedAt(blockPos)
                && items[i].TryGetComponent(out IDrop drop))
            {
                var index = gridWord.ConvertWorldPosToIndex(blockPos);
                var wordPos = gridWord.ConvertIndexToWorldPos(index);
                drop.Drop(wordPos, index);
            }
        }
    }
}
