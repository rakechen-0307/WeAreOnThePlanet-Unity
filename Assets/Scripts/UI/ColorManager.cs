using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [SerializeField]
    private Color brushColor;

    [SerializeField]
    private List<Button> buttons;

    [SerializeField]
    private List<Color> colors = new List<Color>() {Color.white, Color.black, Color.red, Color.green, Color.blue};
    private void Start()
    {
        brushColor = colors[0];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Button childButton = child.GetComponent<Button>();
            Image childImage = child.GetComponent<Image>();
            Color assignedColor = colors[i];
            childButton.onClick.AddListener(() => SetColor(assignedColor));
            childButton.image.color = colors[i];
            buttons.Add(childButton);
        }
    }
    private void SetColor(Color color)
    {
        Debug.Log(color);
        brushColor = color;
    }
}
