using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class CustomUI : MonoBehaviour
{
    // Properties
    [SerializeField]
    private Vector2 size = new Vector2(0.5f, 0.5f);

    [SerializeField]
    private Vector2 anchor = new Vector2(0.5f, 0.5f);

    [SerializeField]
    private Alignment alignment = Alignment.MIDDLE_CENTER;

    [SerializeField]
    private SizeMode sizeMode = SizeMode.RelativeToParent;

    // References
    public RectTransform rectTransform;
    public RectTransform parentRectTransform;

    // Editor
    private SizeMode sizeMode_prev = SizeMode.RelativeToParent;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        Resize();
    }
    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        Resize();
    }
    #if UNITY_EDITOR
    private void OnValidate()
    {   
        if (Application.isPlaying || rectTransform == null)
        {
            return;
        }
        if (parentRectTransform == null)
        {
            parentRectTransform = transform.parent.GetComponent<RectTransform>();
        }
        if (parentRectTransform != null)
        {
            if (sizeMode != sizeMode_prev)
            {
                switchSizeMode(sizeMode_prev, sizeMode);
                sizeMode_prev = sizeMode;
            }
            UnityEditor.EditorApplication.delayCall += Resize;
        }
    }
    #endif
    public void Resize()
    {
        switch (sizeMode)
        {
            case SizeMode.Absolute:
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.zero;
                rectTransform.pivot = Vector2.zero;
                rectTransform.anchoredPosition = getBottomLeftCorner(alignment, anchor, size);
                rectTransform.sizeDelta = size;
                break;
            case SizeMode.RelativeToParent:
            default:
                rectTransform.anchorMin = getBottomLeftCorner(alignment, anchor, size);
                rectTransform.anchorMax = rectTransform.anchorMin + size;
                rectTransform.pivot = anchor;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = Vector2.zero;
                break;
        }
    }
    private Vector2 getBottomLeftCorner(Alignment alignment, Vector2 anchor, Vector2 size)
    {
        Vector2 corner;
        switch (alignment)
        {
            case Alignment.LEFT_TOP:
                corner = anchor - new Vector2(0f, size.y);
                break;
            case Alignment.LEFT_CENTER:
                corner = anchor - new Vector2(0f, size.y / 2);
                break;
            case Alignment.LEFT_BOTTOM:
                corner = anchor;
                break;
            case Alignment.MIDDLE_TOP:
                corner = anchor - new Vector2(size.x / 2, size.y);
                break;
            case Alignment.MIDDLE_CENTER:
                corner = anchor - new Vector2(size.x / 2, size.y / 2);
                break;
            case Alignment.MIDDLE_BOTTOM:
                corner = anchor - new Vector2(size.x / 2, 0f);
                break;
            case Alignment.RIGHT_TOP:
                corner = anchor - new Vector2(size.x, size.y);
                break;
            case Alignment.RIGHT_CENTER:
                corner = anchor - new Vector2(size.x, size.y / 2);
                break;
            case Alignment.RIGHT_BOTTOM:
                corner = anchor - new Vector2(size.x, 0f);
                break;
            default:
                corner = anchor - new Vector2(size.x / 2, size.y / 2);
                break;
        }
        return corner;
    }
    private void switchSizeMode(SizeMode from, SizeMode to)
    {
        if (from == to)
        {
            return;
        }

        Rect parentRect = parentRectTransform.rect;
        switch (from)
        {
            case SizeMode.Absolute:
                size.x /= parentRect.width;
                size.y /= parentRect.height;
                anchor.x /= parentRect.width;
                anchor.y /= parentRect.height;
                break;
            case SizeMode.RelativeToParent:
            default:
                size.x *= parentRect.width;
                size.y *= parentRect.height;
                anchor.x = anchor.x * parentRect.width;
                anchor.y = anchor.y * parentRect.height;
                break;
        }
    }
    public void setSize(Vector2 Size)
    {
        size = Size;
    }
    public void setAnchor(Vector2 Anchor)
    {
        anchor = Anchor;
    }
    public void setAlignment(Alignment newAlignment)
    {
        alignment = newAlignment;
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
    RelativeToParent
}