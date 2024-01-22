using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Sensitivity Parameters
    [SerializeField] [Range(0.1f, 0.3f)]
    private float rotate_sensitivity = 0.25f;
    
    [SerializeField] [Range(8f, 20f)]
    private float zoom_sensitivity = 12f;

    [SerializeField] [Range(0.05f, 0.15f)]
    private float pan_sensitivity = 0.1f;
    
    // Mouse Anchors
    private Vector3 anchorPoint_rotaion = Vector3.zero;
    private Quaternion anchorRotation = Quaternion.identity;

    private Vector3 anchorPoint_pan = Vector3.zero;
    private Vector3 anchorPosition = Vector3.zero;

    private void Update()
    {        
        // anchoring
        if (Input.GetMouseButtonDown(1))
        {
            anchorPoint_rotaion.Set(Input.mousePosition.y, -Input.mousePosition.x, 0f);
            anchorRotation = transform.rotation;
        }
        if (Input.GetMouseButtonDown(2))
        {
            anchorPoint_pan = Input.mousePosition;
            anchorPosition = transform.position;
        }
        // actions
        if (Input.GetMouseButton(1))
        {   
            RotateCamera();
        }
        
        if (Input.GetMouseButton(2))
        {
            panCamera();
        }
        ZoomCamera();
    }

    private void RotateCamera()
    {
        Vector3 mouse_difference = anchorPoint_rotaion - new Vector3(Input.mousePosition.y, -Input.mousePosition.x, 0f);
        Vector3 newAngles = anchorRotation.eulerAngles + mouse_difference * rotate_sensitivity;
        transform.rotation = Quaternion.Euler(newAngles);
    }

    private void ZoomCamera()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        Vector3 newPosition = transform.position + transform.forward * scrollValue * zoom_sensitivity;
        transform.position = newPosition;
    }
    private void panCamera()
    {
        Vector3 mouse_difference = anchorPoint_pan - Input.mousePosition;
        Vector3 displacement = mouse_difference * pan_sensitivity;
        Vector3 newPosition = anchorPosition + displacement.x * transform.right + displacement.y * transform.up;
        transform.position = newPosition;
    }
}
