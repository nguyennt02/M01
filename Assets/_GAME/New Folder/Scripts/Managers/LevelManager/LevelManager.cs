using System.Collections;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{
    [Range(.05f, 10)]
    [SerializeField] float StartLength = .05f;

    void Start()
    {
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

    void StartTimerInvoker()
    {
        ItemManager.Instance.InitGird();
        // ItemManager.Instance.InitFloorBlock();
        ItemManager.Instance.InitBlock();
        ItemManager.Instance.InitAvailableBlock();
    }
}
