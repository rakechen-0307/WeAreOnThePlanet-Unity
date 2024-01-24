using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DrawUIManager : MonoBehaviour
{
    private int width = Screen.width;
    private int height = Screen.height;

    [SerializeField]
    private RectTransform ColorPlate;
    
    [SerializeField]
    private List<RectTransform> ColorButton_Transforms = new List<RectTransform>();
    private List<Button> ColorButtons = new List<Button>();

    public Vector2 test;

    private void Start()
    {
        ResizeUI();
    }
    private void OnRectTransformDimensionsChange()
    {
        if (ColorPlate==null)
        {
            return;
        }
        ResizeUI();
    }
    private void ResizeUI()
    {
        // Update current screen size
        width = Screen.width;
        height = Screen.height;
        // color plate
        ColorPlate.sizeDelta = new Vector2(width * 0.1f, height * 0.9f);
        ColorPlate.anchoredPosition = new Vector2(width * 0.05f, height * 0.05f);
        // color buttons
        int ButtomNum = ColorButton_Transforms.Count;
        float sizeY = ColorPlate.sizeDelta.y / ButtomNum;
        float cellSize = Mathf.Min(ColorPlate.sizeDelta.x, sizeY);
        for (int i = 0; i < ColorButton_Transforms.Count; i++)
        {
            ColorButton_Transforms[i].anchoredPosition = test;
            ColorButton_Transforms[i].sizeDelta = new Vector2(cellSize, cellSize) * 0.9f;
        }
    }
}
