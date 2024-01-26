using UnityEngine;
using UnityEngine.EventSystems;

public class VisualizeBlock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public PlacementSystem placementSystem;
    private Material material;
    public Color color = Color.white;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        material.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        placementSystem.PlaceBlock(transform.position + Vector3.up);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        material.color += new Color(0.5f, 0.5f, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        material.color = color;
    }
}
