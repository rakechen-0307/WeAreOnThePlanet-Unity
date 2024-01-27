using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Texture2D cursorTexture; // Assign your cursor image in the Unity Editor

    [SerializeField] [Range(0.1f, 1f)]
    private float speed = 0.3f;

    [SerializeField] [Range(0.5f, 2f)]
    private float rotateSensitivity = 1f;

    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private Transform eyes;

    void Start()
    {
        // rigidbody settings
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.drag = 3f;
        // eyes
        eyes = transform.GetChild(0);
        // cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveDirection.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection.Normalize();
    }

    void FixedUpdate()
    {
        if (moveDirection != Vector3.zero)
        {
            rb.AddForce(transform.right * moveDirection.x * speed + transform.forward * moveDirection.z * speed, ForceMode.Impulse);
        }
        // rotate character
        transform.Rotate(Input.GetAxis("Mouse X") * Vector3.up * rotateSensitivity);
        eyes.Rotate(Input.GetAxis("Mouse Y") * Vector3.forward * rotateSensitivity);
    }

    void OnGUI()
    {
        // Get the center of the screen
        Vector2 centerScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        // Draw the cursor at the center of the screen
        GUI.DrawTexture(new Rect(centerScreen.x - (cursorTexture.width / 2), centerScreen.y - (cursorTexture.height / 2), cursorTexture.width, cursorTexture.height), cursorTexture);
    }
}