using System.Collections;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set;}
    [Range(.05f, 10)]
    [SerializeField] float StartLength = .05f;
    [SerializeField] LevelEdittor levelEdittor;

    void Start()
    {
        if(Instance == null) Instance = this;
        AddEvent();
        StartCoroutine(TimerInvoker());
    }

    void AddEvent()
    {
        LeanTouchManager.Instance.onTouchBegan += OnTouchBegan;
        LeanTouchManager.Instance.onTouchMoved += OnTouchMove;
        LeanTouchManager.Instance.onTouchEnd += OnTouchEnd;
    }

    void OnDestroy()
    {
        LeanTouchManager.Instance.onTouchBegan -= OnTouchBegan;
        LeanTouchManager.Instance.onTouchMoved -= OnTouchMove;
        LeanTouchManager.Instance.onTouchEnd -= OnTouchEnd;
    }

    IEnumerator TimerInvoker()
    {
        yield return new WaitForSeconds(StartLength);
        StartTimerInvoker();
    }

    public LevelDesignObject GetCurrentLevelDesignObject()
    {
        int levelIndex = 0;
        return levelEdittor.levelDesignObjects[levelIndex];
    }

    void StartTimerInvoker()
    {
        var CurrentLevelDesignObject = GetCurrentLevelDesignObject();
        ItemManager.Instance.InitGird(CurrentLevelDesignObject);
        ItemManager.Instance.InitFloorBlock(CurrentLevelDesignObject);
        ItemManager.Instance.InitBlock(CurrentLevelDesignObject);
        ItemManager.Instance.InitAvailableBlock(CurrentLevelDesignObject);
    }
}
