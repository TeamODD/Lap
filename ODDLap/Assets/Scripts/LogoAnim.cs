using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class LogoAnim : MonoBehaviour
{
    public float amplitude = 30f; // 이동 높이 (위아래 총 60)
    public float speed = 1f;      // 속도 (진동 주기)

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * speed) * amplitude;
        rectTransform.anchoredPosition = startPos + new Vector2(0, offsetY);
    }
}
