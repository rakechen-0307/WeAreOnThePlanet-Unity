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
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        placementSystem.ClickBlock(transform.position, Vector3.up);
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
