using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] [RequireComponent(typeof(RectTransform))]
public class UITesting : MonoBehaviour
{
    [SerializeField]
    private Vector2 anchorMin = new Vector2(0, 0);

    [SerializeField]
    private Vector2 anchorMax = new Vector2(1, 1);

    [SerializeField]
    private Vector2 pivot = new Vector2(0.5f, 0.5f);

    [SerializeField]
    private Vector2 anchoredPosition = new Vector2(0, 0);

    [SerializeField]
    private Vector2 sizeDelta = new Vector2(0, 0);

    // References
    private RectTransform rectTransform;
    // Editor

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnRectTransformDimensionsChange()
    {
        if (rectTransform == null)
        {
            return;
        }
        Resize();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (rectTransform != null)
        {
            Resize();
        }
    }
    #endif

    protected virtual void Resize()
    {
        rectTransform.pivot = pivot;
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = sizeDelta;
    }
}
