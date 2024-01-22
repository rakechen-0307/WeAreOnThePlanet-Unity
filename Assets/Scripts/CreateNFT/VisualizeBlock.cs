using UnityEngine;
using UnityEngine.EventSystems;

public class VisualizeBlock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public PlacementSystem placementSystem;
    private Material material;
    private Color color;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        color = Color.blue;
        material.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        placementSystem.PlaceBlock(transform.position + Vector3.up);
        // Debug.Log("Click");
        // color = color == Color.blue ? Color.red : Color.blue;
        // material.color = color + new Color(0.5f, 0.5f, 0.5f);
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
