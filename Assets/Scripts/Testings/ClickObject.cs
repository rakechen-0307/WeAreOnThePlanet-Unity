// Requirements: a Physics Raycaster in camera and a EventSystem
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Material material;
    private Color color ;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        color = Color.blue;
        material.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        color = color == Color.blue ? Color.red : Color.blue;
        material.color = color + new Color(0.5f, 0.5f, 0.5f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Enter");
        material.color += new Color(0.5f, 0.5f, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Exit");
        material.color = color;
    }
}
