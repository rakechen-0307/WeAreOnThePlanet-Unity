using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Texture2D cursorTexture; // Assign your cursor image in the Unity Editor
    private float speed = 0.7f;

    [SerializeField] [Range(0.4f, 1.2f)]
    private float walkSpeed = 0.7f;

    [SerializeField] [Range(0.5f, 2f)]
    private float runSpeed = 1.4f;

    [SerializeField] [Range(100f, 600f)]
    private float rotateSensitivity = 400f;

    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private Transform eyes;
    private float verticalRoatation = 0f;

    public bool moveable = true;

    void Start()
    {
        // rigidbody settings
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
        // eyes
        eyes = transform.GetChild(0);
        // cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        moveable = true;
    }

    void Update()
    {
        if (!moveable)
        {
            return;
        }
        //calculate movement
        moveDirection.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        // run
        speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        // rotate character
        transform.Rotate(Input.GetAxis("Mouse X") * Vector3.up * Time.deltaTime * rotateSensitivity);
        verticalRoatation += Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSensitivity;
        verticalRoatation = Mathf.Clamp(verticalRoatation, -60, 60);
        eyes.localEulerAngles = Vector3.left * verticalRoatation;
    }

    void FixedUpdate()
    {
        if (!moveable)
        {
            return;
        }
        if (moveDirection != Vector3.zero)
        {
            rb.AddForce(transform.right * moveDirection.x * speed + transform.forward * moveDirection.z * speed, ForceMode.Impulse);
        }
    }
}