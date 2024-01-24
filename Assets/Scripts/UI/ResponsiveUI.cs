using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode] [RequireComponent(typeof(RectTransform))]
public class ResponsiveUI : MonoBehaviour
{
    // Properties
    [SerializeField]
    private Vector2 size = new Vector2(100, 100);

    [SerializeField]
    private Vector2 anchorPoint = new Vector2(0.5f, 0.5f);

    [SerializeField]
    private Alignment alignment = Alignment.MIDDLE_CENTER;

    [SerializeField]
    private SizeMode sizeMode = SizeMode.RelativeToParent;

    // References
    private RectTransform rectTransform;
    
    [SerializeField]
    private RectTransform parent_rectTransform;
    // Editor

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parent_rectTransform = transform.parent.GetComponent<RectTransform>();
        if (parent_rectTransform == null)
        {
            sizeMode = SizeMode.RelativeToScreen;
        }
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
            Debug.Log("validate!");
        }
    }
    #endif

    protected virtual void Resize()
    {
        // Initial Settings
        rectTransform.pivot = Vector2.zero;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;

        Vector2 origin, sizeUnit;
        (origin, sizeUnit) = getOrigin_Unit(sizeMode);
        Debug.Log((origin, sizeUnit));
        // Set RectTransform
        rectTransform.anchoredPosition = origin + getAnchoredPosition(alignment);
        rectTransform.sizeDelta = Vector2.Scale(size, sizeUnit);
    }
    private Vector2 getAnchoredPosition(Alignment alignment)
    {
        Vector2 anchoredPosition;
        switch (alignment)
        {
            case Alignment.LEFT_TOP:
                anchoredPosition = anchorPoint - new Vector2(0f, size.y);
                break;
            case Alignment.LEFT_CENTER:
                anchoredPosition = anchorPoint - new Vector2(0f, size.y / 2);
                break;
            case Alignment.LEFT_BOTTOM:
                anchoredPosition = anchorPoint;
                break;
            case Alignment.MIDDLE_TOP:
                anchoredPosition = anchorPoint - new Vector2(size.x / 2, size.y);
                break;
            case Alignment.MIDDLE_CENTER:
                anchoredPosition = anchorPoint - new Vector2(size.x / 2, size.y / 2);
                break;
            case Alignment.MIDDLE_BOTTOM:
                anchoredPosition = anchorPoint - new Vector2(size.x / 2, 0f);
                break;
            case Alignment.RIGHT_TOP:
                anchoredPosition = anchorPoint - new Vector2(size.x, size.y);
                break;
            case Alignment.RIGHT_CENTER:
                anchoredPosition = anchorPoint - new Vector2(size.x, size.y / 2);
                break;
            case Alignment.RIGHT_BOTTOM:
                anchoredPosition = anchorPoint - new Vector2(size.x, 0f);
                break;
            default:
                anchoredPosition = anchorPoint - new Vector2(size.x / 2, size.y / 2);
                break;
        }
        return anchoredPosition;
    }
    private (Vector2, Vector2) getOrigin_Unit(SizeMode sizeMode)
    {
        Vector2 origin;
        Vector2 sizeUnit;
        switch (sizeMode)
        {
            case SizeMode.Absolute:
                origin = Vector2.zero;
                sizeUnit = Vector2.one;
                break;
            case SizeMode.RelativeToParent:
                if (parent_rectTransform != null)
                {
                    origin = parent_rectTransform.anchoredPosition;
                    sizeUnit = parent_rectTransform.sizeDelta;
                }
                else
                {
                    origin = Vector2.zero;
                    sizeUnit = parent_rectTransform.sizeDelta;
                }
                break;
            case SizeMode.RelativeToScreen:
                origin = Vector2.zero;
                sizeUnit = new Vector2(Screen.width, Screen.height);
                break;
            default:
                origin = Vector2.zero;
                sizeUnit = Vector2.one;
                break;
        }
        return (origin, sizeUnit);
    }
}

public enum Alignment
{
    LEFT_TOP,
    LEFT_CENTER,
    LEFT_BOTTOM,
    MIDDLE_TOP,
    MIDDLE_CENTER,
    MIDDLE_BOTTOM,
    RIGHT_TOP,
    RIGHT_CENTER,
    RIGHT_BOTTOM
}
public enum SizeMode
{
    Absolute,
    RelativeToParent,
    RelativeToScreen
}