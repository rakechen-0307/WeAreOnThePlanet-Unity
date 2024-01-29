using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Climb : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 0.5f)]
    private float climbThreshold = 0.3f;

    [SerializeField] [Range(1f, 5f)]
    private float climbSpeed = 2f;

    private Collider playerCollider;
    private Rigidbody rb;

    private void Start()
    {
        playerCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            ClimbStep(collision);
        }        
    }
    // private void OnCollisionStay(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Climbable"))
    //     {
    //         ClimbStair(collision);
    //     }
    // }
    private void ClimbStep(Collision collision)
    {
        // calculate player lowest position
        float colliderHeight = playerCollider.bounds.size.y;
        Vector3 colliderCenter = playerCollider.bounds.center;
        Vector3 playerFeetPosition = colliderCenter - 0.5f * colliderHeight * transform.up;

        // calculate contact point
        Vector3 averageContactPoint = Vector3.zero;
        int totalContacts = 0;
        foreach (ContactPoint contact in collision.contacts)
        {
            averageContactPoint += contact.point;
            totalContacts++;
        }
        averageContactPoint /= totalContacts;

        // calculate height difference
        Vector3 difference = averageContactPoint - playerFeetPosition;
        float heightDifference = Vector3.Dot(difference, transform.up); // (a dot b) / (b^2)

        // Check if the height difference is below the threshold
        if (heightDifference < climbThreshold * colliderHeight)
        {
            Vector3 newPosition = transform.position + difference;
            Vector3 lerpPosition = Vector3.Lerp(transform.position, newPosition, climbSpeed);
            rb.MovePosition(lerpPosition);
        }
    }
    private void ClimbStair(Collision collision)
    {
        // calculate player lowest position
        float colliderHeight = playerCollider.bounds.size.y;
        Vector3 colliderCenter = playerCollider.bounds.center;
        Vector3 playerFeetPosition = colliderCenter - 0.5f * colliderHeight * transform.up;

        // calculate contact point
        Vector3 averageContactPoint = Vector3.zero;
        int totalContacts = 0;
        foreach (ContactPoint contact in collision.contacts)
        {
            averageContactPoint += contact.point;
            totalContacts++;
            Vector3 vector3 = contact.point - playerFeetPosition;
            Debug.Log(Vector3.Dot(vector3, transform.up));
        }
        averageContactPoint /= totalContacts;

        // calculate height difference
        Vector3 difference = averageContactPoint - playerFeetPosition;
        float heightDifference = Vector3.Dot(difference, transform.up); // (a dot b) / (b^2)

        // Check if the height difference is below the threshold
        if (heightDifference < climbThreshold * colliderHeight && heightDifference > 0.2f * colliderHeight)
        {
            Vector3 newPosition = transform.position + difference;
            Vector3 lerpPosition = Vector3.Lerp(transform.position, newPosition, climbSpeed);
            rb.MovePosition(lerpPosition);
        }
    }
}
