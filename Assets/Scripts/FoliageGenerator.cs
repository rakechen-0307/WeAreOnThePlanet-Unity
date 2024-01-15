using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class FoliageGenerator : MonoBehaviour
{

    public GameObject prefab;
    private Matrix4x4[] TransformData;
    public bool preview = true;

    // Preview in editor
    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (!EditorApplication.isPlaying&&preview)
        {
            Generate();
        }
    }
    #endif

    // Start is called before the first frame update
    private void Start()
    {
        Generate();
    }

    // Update is called once per frame
    private void Update()
    {
        #if UNITY_EDITOR
        if (!EditorApplication.isPlaying&&preview)
        {
            DrawMesh();
            return;
        }
        else
        {
            return;
        }
        #else
            DrawMesh();
        #endif
    }
    // Generate object data
    private void Generate()
    {
        Mesh surfaceMesh = GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = surfaceMesh.vertices;
        
        // start
        int[] triangles = surfaceMesh.triangles;
        TransformData = new Matrix4x4[triangles.Length-2];

        for (int i = 0; i < triangles.Length-2; i++)
        {
            // Get the world positions of the triangle vertices
            Vector3 v0 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 v1 = transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 2]]);

            // Calculate random barycentric coordinates
            float rand1 = Random.value;
            float rand2 = Random.value;
            if (rand1 + rand2 > 1f)
            {
                rand1 = 1f - rand1;
                rand2 = 1f - rand2;
            }

            // Interpolate the vertex position within the triangle
            Vector3 vertexWorldPosition = v0 + rand1 * (v1 - v0) + rand2 * (v2 - v0);
            
            Quaternion rotation = Quaternion.Euler(-90, Random.Range(0f, 360f), 0);
            // Instantiate the grass
            TransformData[i] = Matrix4x4.TRS(vertexWorldPosition, rotation, new Vector3(1f, 1f, 1f));
        }
    }
    // DrawMesh
    private void DrawMesh()
    {
        // Generate();
        Graphics.DrawMeshInstanced(prefab.GetComponent<MeshFilter>().sharedMesh, 0, prefab.GetComponent<MeshRenderer>().sharedMaterial, TransformData);
    }
}
