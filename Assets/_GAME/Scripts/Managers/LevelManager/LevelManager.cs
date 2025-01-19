using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Range(.05f, 10)]
    [SerializeField] float StartLength = .05f;

    void Start()
    {
        StartCoroutine(TimerInvoker());
    }

    IEnumerator TimerInvoker()
    {
        yield return new WaitForSeconds(StartLength);
        StartTimerInvoker();
    }

    void StartTimerInvoker()
    {
        ItemManager.Instance.InitGird();
        ItemManager.Instance.InitFloorBlock();
        ItemManager.Instance.InitBlock();
        ItemManager.Instance.InitAvailableBlock();
    }
}
