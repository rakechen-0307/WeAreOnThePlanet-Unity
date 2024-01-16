// Requirements: a Physics Raycaster in camera, an EventSystem and a Grid attached to the object
using UnityEngine;
using UnityEngine.EventSystems;

public class GridObject : MonoBehaviour, IPointerMoveHandler
{
    [SerializeField]
    private GameObject indicator;
    private Grid grid;

    private void Start()
    {
        grid = GetComponent<Grid>();
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = eventData.pointerCurrentRaycast.worldPosition;
        Vector3Int gridPos = grid.WorldToCell(mouseWorldPos);
        indicator.transform.position = grid.CellToWorld(gridPos);
    }
}
