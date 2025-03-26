using System;
using System.Collections.Generic;
using Lean.Touch;
using Unity.Mathematics;
using UnityEngine;

public class LeanTouchManager : MonoBehaviour
{
    public static LeanTouchManager Instance { get; private set; }
    [Header("Events")]
    public Action<float2, Collider2D[]> onTouchBegan;
    public Action<float2, float2> onTouchMoved;
    public Action<float2, float2> onTouchEnd;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        LeanTouch.OnFingerOld += LeanTouch_OnFingerOld;
        LeanTouch.OnFingerDown += LeanTouch_OnDetect;
        LeanTouch.OnGesture += LeanTouch_OnGesture;
        LeanTouch.OnFingerUp += LeanTouch_OnFingerUp;
        LeanTouch.OnFingerExpired += LeanTouch_OnFingerExpired;
    }

    void OnDestroy()
    {
        LeanTouch.OnFingerOld -= LeanTouch_OnFingerOld;
        LeanTouch.OnFingerDown -= LeanTouch_OnDetect;
        LeanTouch.OnGesture -= LeanTouch_OnGesture;
        LeanTouch.OnFingerUp -= LeanTouch_OnFingerUp;
        LeanTouch.OnFingerExpired -= LeanTouch_OnFingerExpired;
    }

    private void LeanTouch_OnFingerOld(LeanFinger finger) { }

    private void LeanTouch_OnDetect(LeanFinger finger)
    {
        if (LeanTouch.Fingers.Count == 1)
        {
            Vector2 startTouchPos = Camera.main.ScreenToWorldPoint(finger.ScreenPosition);
            Collider2D[] colliders = Physics2D.OverlapPointAll(startTouchPos);
            onTouchBegan?.Invoke(startTouchPos, colliders);
        }
    }

    private void LeanTouch_OnGesture(List<LeanFinger> leanFingers)
    {
        if (leanFingers.Count == 0) return;
        var selectedFinger = leanFingers[0];
        Vector2 touchingDirection = selectedFinger.ScreenPosition - selectedFinger.LastScreenPosition;
        Vector2 touchedPosition = Camera.main.ScreenToWorldPoint(selectedFinger.ScreenPosition);

        onTouchMoved?.Invoke(touchedPosition, touchingDirection);
    }

    private void LeanTouch_OnFingerUp(LeanFinger finger)
    {
        Vector2 touchingDirection = finger.ScreenPosition - finger.LastScreenPosition;
        Vector2 touchedPosition = Camera.main.ScreenToWorldPoint(finger.ScreenPosition);

        onTouchEnd?.Invoke(touchedPosition, touchingDirection);
    }

    private void LeanTouch_OnFingerExpired(LeanFinger finger)
    {
        Vector2 touchingDirection = finger.ScreenPosition - finger.LastScreenPosition;
        Vector2 touchedPosition = Camera.main.ScreenToWorldPoint(finger.ScreenPosition);

        onTouchEnd?.Invoke(touchedPosition, touchingDirection);
    }
}
