// Requirements: a Physics Raycaster in camera, an EventSystem and a Grid attached to the object
using UnityEngine;
using UnityEngine.EventSystems;

public class GridObject : MonoBehaviour, IPointerMoveHandler, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject indicator;

    [SerializeField]
    private PlacementSystem placementSystem;

    private Grid grid;

    private void Start()
    {
        grid = GetComponent<Grid>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerMove(PointerEventData eventData)
    {
        indicator.transform.position = getCubePosition(eventData);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        indicator.transform.position = getCubePosition(eventData);
        placementSystem.PlaceBlock(indicator.transform.position);
    }
    private Vector3 getCubePosition(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = eventData.pointerCurrentRaycast.worldPosition;
        Vector3Int gridPos = grid.WorldToCell(mouseWorldPos);
        Vector3 cubePosition = grid.CellToWorld(gridPos) + new Vector3(grid.cellSize.x/2, 0, grid.cellSize.z/2);
        cubePosition.y = grid.cellSize.y/2;
        return cubePosition;
    }
}
