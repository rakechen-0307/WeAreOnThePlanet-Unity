using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityObject : MonoBehaviour
{
    public GravitySource source;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Vector3 g_direction = (source.center - transform.position).normalized;
        Quaternion g_rotation = Quaternion.FromToRotation(transform.up, -g_direction);
        // gravity
        rb.AddForce(rb.mass * source.g * g_direction, ForceMode.Force); // Fg = mg*g_hat 
        // adjust pose
        transform.rotation = g_rotation * transform.rotation; // rotate by g_rotation
    }
}