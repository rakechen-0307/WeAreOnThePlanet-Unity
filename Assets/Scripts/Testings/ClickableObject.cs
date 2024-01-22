// Requirements: a Physics Raycaster in camera and a EventSystem
// Notice: more IPointer event handler can be added to support more events
// Usage: Object scripts can inherit this and override the handler functions
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
    }
    public virtual void OnPointerMove(PointerEventData eventData)
    {
        Debug.Log("Move to "+$"{eventData.pointerCurrentRaycast.worldPosition}");
    }
}
