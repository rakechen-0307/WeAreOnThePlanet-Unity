using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GravitySource : MonoBehaviour
{
    public Vector3 center;
    public float g = 9.8f;
    // Start is called before the first frame update
    void Awake()
    {
        center = transform.position;
    }
}