using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Texture2D cursorTexture; // Assign your cursor image in the Unity Editor

    [SerializeField] [Range(0.3f, 1f)]
    private float speed = 0.5f;

    [SerializeField] [Range(200f, 600f)]
    private float rotateSensitivity = 400f;

    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private Transform eyes;
    private float verticalRoatation = 0f;

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
    }

    void Update()
    {
        //calculate movement
        moveDirection.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        // rotate character
        transform.Rotate(Input.GetAxis("Mouse X") * Vector3.up * Time.deltaTime * rotateSensitivity);
        verticalRoatation += Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSensitivity;
        verticalRoatation = Mathf.Clamp(verticalRoatation, -60, 60);
        eyes.localEulerAngles = Vector3.left * verticalRoatation;
    }

    void FixedUpdate()
    {
        if (moveDirection != Vector3.zero)
        {
            rb.AddForce(transform.right * moveDirection.x * speed + transform.forward * moveDirection.z * speed, ForceMode.Impulse);
        }
    }

    void OnGUI()
    {
        // Get the center of the screen
        Vector2 centerScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        // Draw the cursor at the center of the screen
        GUI.DrawTexture(new Rect(centerScreen.x - (cursorTexture.width / 2), centerScreen.y - (cursorTexture.height / 2), cursorTexture.width, cursorTexture.height), cursorTexture);
    }
}