using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteUIHandle : MonoBehaviour
{
    public static DynamiteUIHandle Instance;

    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private GameObject bar;
    [SerializeField] private RectTransform rectBar;
    [SerializeField] private RectTransform baseBar;
    [SerializeField] private RectTransform perfectFieldBar;
    [SerializeField] private RectTransform tickBar;

    private Camera main;

    private float minPerfect;
    private float maxPerfect;
    private float maxTime;
    private float lengthBar;

    private void Start()
    {
        Instance = this;
        main = Camera.main;
    }

    private void OnEnable()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
    }

    public void Init(float minPerfect, float maxPerfect, float maxTime)
    {
        this.minPerfect = minPerfect;
        this.maxPerfect = maxPerfect;
        this.maxTime = maxTime;

        lengthBar = rectBar.rect.width;

        float minFieldPerfect = lengthBar * (this.minPerfect/ this.maxTime);
        float maxFieldPerfect = lengthBar - (lengthBar * (this.maxPerfect / this.maxTime));
        perfectFieldBar.offsetMin = new Vector2(minFieldPerfect, perfectFieldBar.offsetMin.y);
        perfectFieldBar.offsetMax = new Vector2(-maxFieldPerfect, perfectFieldBar.offsetMax.y);
    }

    public void ShowBar(bool flag)
    {
        bar.SetActive(flag);
        SetTickField(0);
    }

    public void UpdateTickTimeBar(float time, Vector2 worldPos)
    {
        SetTickField(time);
        SetPositionBar(worldPos);
    }

    private void SetPositionBar(Vector2 worldPos)
    {
        Vector2 canvasPos;
        Vector2 screenPos = main.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, main, out canvasPos);
        rectBar.anchoredPosition = canvasPos;
    }

    private void SetTickField(float time)
    {
        float convertToTickField = (time / maxTime) * 10f;

        float minFieldTime = lengthBar / 10f * (Mathf.Clamp(convertToTickField, 0, 9f));
        float maxFieldTime = lengthBar - (lengthBar / 10f * (Mathf.Clamp(convertToTickField + 1, 0, 10f)));

        tickBar.offsetMin = new Vector2(minFieldTime, tickBar.offsetMin.y);
        tickBar.offsetMax = new Vector2(-maxFieldTime, tickBar.offsetMax.y);
    }
}
