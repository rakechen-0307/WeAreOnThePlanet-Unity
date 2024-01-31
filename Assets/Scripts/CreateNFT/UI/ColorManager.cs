using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [SerializeField]
    private Color brushColor;

    [SerializeField]
    private RectTransform rectTransform;
    // [SerializeField]
    // private List<Button> buttons;

    [SerializeField]
    private List<Color> colors = new List<Color>() {Color.white, Color.black, Color.red, Color.green, Color.blue, Color.magenta, Color.yellow, Color.cyan};
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        brushColor = colors[0];
        int childNumber = transform.childCount;
        for (int i = 0; i < childNumber; i++)
        {
            Transform child = transform.GetChild(i);
            InitializeButton(child, i, childNumber);
        }
    }
    private void Update()
    {
        ControlAspectRatioMode();
    }

    private void InitializeButton(Transform buttonTransform, int buttonIndex, int childNumber)
    {
        Button childButton = buttonTransform.GetComponent<Button>();
        Image childImage = buttonTransform.GetComponent<Image>();
        CustomUI childUI = buttonTransform.GetComponent<CustomUI>();
        Color assignedColor = colors[buttonIndex];
        
        childButton.onClick.AddListener(() => SetColor(assignedColor));
        childImage.color = assignedColor;

        Vector2 buttonSize = new Vector2(0.8f, 1 / (float)childNumber);
        childUI.setAlignment(Alignment.MIDDLE_BOTTOM);
        childUI.setSize(buttonSize);
        childUI.setAnchor(new Vector2(0.5f, (float)buttonIndex / (float)(childNumber)));
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += childUI.Resize;
#endif
    }
    private void ControlAspectRatioMode()
    {
        int childNumber = transform.childCount;
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height / (float)childNumber;

        for (int i = 0; i < childNumber; i++)
        {
            Transform child = transform.GetChild(i);
            AspectRatioFitter aspectRatioFitter = child.GetComponent<AspectRatioFitter>();
            aspectRatioFitter.aspectMode = (width<height) ? AspectRatioFitter.AspectMode.WidthControlsHeight : AspectRatioFitter.AspectMode.HeightControlsWidth;
        }
    }
    private void SetColor(Color color)
    {
        brushColor = color;
    }
    public Color getBrushColor()
    {
        return brushColor;
    }
}
