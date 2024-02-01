using UnityEngine;

public class GardenGrower : MonoBehaviour
{
    [SerializeField]
    private GameObject[] plantPrefabs = new GameObject[4];

    public int testLevel = 0;

    private void Start()
    {
        growPlants(testLevel);
    }

    public void growPlants(int level)
    {
        // clamp input
        level = Mathf.Clamp(level, 0, 3);

        // clean up
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // generate

        Mesh surfaceMesh = GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = surfaceMesh.vertices;

        // start
        int[] triangles = surfaceMesh.triangles;
        Vector3[] normals = surfaceMesh.normals;
        Vector4[] tangents = surfaceMesh.tangents;

        for (int i = 0; i < triangles.Length - 2; i++)
        {
            // Get the world positions of the triangle vertices
            Vector3 v0 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 v1 = transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 2]]);

            // Get the normals of the vertices
            Vector3 normal0 = normals[triangles[i]];
            Vector3 normal1 = normals[triangles[i + 1]];
            Vector3 normal2 = normals[triangles[i + 2]];

            // Get the tangents of the vertices
            Vector4 tangent0 = tangents[triangles[i]];
            Vector4 tangent1 = tangents[triangles[i + 1]];
            Vector4 tangent2 = tangents[triangles[i + 2]];

            // Calculate random barycentric coordinates
            float rand1 = Random.value;
            float rand2 = Random.value;
            if (rand1 + rand2 > 1f)
            {
                rand1 = 1f - rand1;
                rand2 = 1f - rand2;
            }

            // Interpolate the vertex position within the triangle
            Vector3 Position = v0 + rand1 * (v1 - v0) + rand2 * (v2 - v0);

            Vector3 Normal = normal0 + rand1 * (normal1 - normal0) + rand2 * (normal2 - normal0);

            Vector4 Tangent = tangent0 + rand1 * (tangent1 - tangent0) + rand2 * (tangent2 - tangent0);

            Quaternion rotation_norm = Quaternion.LookRotation(Normal, Tangent);
            Quaternion rotation_rand = Quaternion.AngleAxis(Random.Range(0f, 360f), plantPrefabs[level].transform.up);

            // Instantiate the plants
            Instantiate(plantPrefabs[level], Position, plantPrefabs[level].transform.rotation * rotation_norm * rotation_rand, transform);
        }
    }
}
