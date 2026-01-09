using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CircularPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private float radius = 10f;
    [SerializeField] private int segments = 64;
    [SerializeField] private float height = 0.5f;

    [Header("Edge Settings")]
    [SerializeField] private bool addEdgeRail = true;
    [SerializeField] private float railHeight = 1f;
    [SerializeField] private float railThickness = 0.2f;

    [Header("Materials")]
    [SerializeField] private Material platformMaterial;
    [SerializeField] private Material railMaterial;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private GameObject edgeRail;

    void Start()
    {
        GeneratePlatform();
        if (addEdgeRail)
        {
            GenerateEdgeRail();
        }
    }

    void GeneratePlatform()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        Mesh mesh = CreateCircularMesh();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = false;

        if (platformMaterial != null)
        {
            meshRenderer.material = platformMaterial;
        }
    }

    Mesh CreateCircularMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Circular Platform";

        // Calculate vertex count: center + ring vertices + top and bottom faces
        int vertexCount = (segments + 1) * 2 + 2; // Top and bottom rings + center points
        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];

        // Center vertices (top and bottom)
        int centerTop = 0;
        int centerBottom = 1;
        vertices[centerTop] = new Vector3(0, height / 2, 0);
        vertices[centerBottom] = new Vector3(0, -height / 2, 0);
        normals[centerTop] = Vector3.up;
        normals[centerBottom] = Vector3.down;
        uvs[centerTop] = new Vector2(0.5f, 0.5f);
        uvs[centerBottom] = new Vector2(0.5f, 0.5f);

        // Generate ring vertices
        float angleStep = 360f / segments;
        int topRingStart = 2;
        int bottomRingStart = topRingStart + segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            // Top ring
            vertices[topRingStart + i] = new Vector3(x, height / 2, z);
            normals[topRingStart + i] = Vector3.up;
            uvs[topRingStart + i] = new Vector2(
                0.5f + Mathf.Cos(angle) * 0.5f,
                0.5f + Mathf.Sin(angle) * 0.5f
            );

            // Bottom ring
            vertices[bottomRingStart + i] = new Vector3(x, -height / 2, z);
            normals[bottomRingStart + i] = Vector3.down;
            uvs[bottomRingStart + i] = new Vector2(
                0.5f + Mathf.Cos(angle) * 0.5f,
                0.5f + Mathf.Sin(angle) * 0.5f
            );
        }

        // Generate triangles
        int[] triangles = new int[segments * 12]; // Top face + bottom face + sides
        int triIndex = 0;

        // Top face triangles
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            triangles[triIndex++] = centerTop;
            triangles[triIndex++] = topRingStart + next;
            triangles[triIndex++] = topRingStart + i;
        }

        // Bottom face triangles
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            triangles[triIndex++] = centerBottom;
            triangles[triIndex++] = bottomRingStart + i;
            triangles[triIndex++] = bottomRingStart + next;
        }

        // Side face triangles
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;

            // First triangle
            triangles[triIndex++] = topRingStart + i;
            triangles[triIndex++] = topRingStart + next;
            triangles[triIndex++] = bottomRingStart + i;

            // Second triangle
            triangles[triIndex++] = topRingStart + next;
            triangles[triIndex++] = bottomRingStart + next;
            triangles[triIndex++] = bottomRingStart + i;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return mesh;
    }

    void GenerateEdgeRail()
    {
        edgeRail = new GameObject("EdgeRail");
        edgeRail.transform.SetParent(transform);
        edgeRail.transform.localPosition = Vector3.zero;

        MeshFilter railMeshFilter = edgeRail.AddComponent<MeshFilter>();
        MeshRenderer railMeshRenderer = edgeRail.AddComponent<MeshRenderer>();
        MeshCollider railMeshCollider = edgeRail.AddComponent<MeshCollider>();

        Mesh railMesh = CreateRailMesh();
        railMeshFilter.mesh = railMesh;
        railMeshCollider.sharedMesh = railMesh;
        railMeshCollider.convex = false;

        if (railMaterial != null)
        {
            railMeshRenderer.material = railMaterial;
        }
    }

    Mesh CreateRailMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Edge Rail";

        float innerRadius = radius - railThickness / 2;
        float outerRadius = radius + railThickness / 2;
        float railBottom = height / 2;
        float railTop = railBottom + railHeight;

        int vertexCount = segments * 8; // Inner and outer rings, top and bottom
        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];

        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            int baseIndex = i * 8;

            // Outer ring vertices
            vertices[baseIndex + 0] = new Vector3(cos * outerRadius, railBottom, sin * outerRadius);
            vertices[baseIndex + 1] = new Vector3(cos * outerRadius, railTop, sin * outerRadius);

            // Inner ring vertices
            vertices[baseIndex + 2] = new Vector3(cos * innerRadius, railBottom, sin * innerRadius);
            vertices[baseIndex + 3] = new Vector3(cos * innerRadius, railTop, sin * innerRadius);

            // Duplicate vertices for different faces (to get proper normals)
            vertices[baseIndex + 4] = vertices[baseIndex + 0]; // Outer bottom
            vertices[baseIndex + 5] = vertices[baseIndex + 1]; // Outer top
            vertices[baseIndex + 6] = vertices[baseIndex + 2]; // Inner bottom
            vertices[baseIndex + 7] = vertices[baseIndex + 3]; // Inner top

            Vector3 outwardNormal = new Vector3(cos, 0, sin);
            normals[baseIndex + 0] = outwardNormal;
            normals[baseIndex + 1] = outwardNormal;
            normals[baseIndex + 2] = -outwardNormal;
            normals[baseIndex + 3] = -outwardNormal;
            normals[baseIndex + 4] = Vector3.down;
            normals[baseIndex + 5] = Vector3.up;
            normals[baseIndex + 6] = Vector3.down;
            normals[baseIndex + 7] = Vector3.up;
        }

        // Generate triangles
        int[] triangles = new int[segments * 24]; // 4 faces per segment, 2 triangles per face
        int triIndex = 0;

        for (int i = 0; i < segments; i++)
        {
            int baseIndex = i * 8;
            int nextBase = ((i + 1) % segments) * 8;

            // Outer face
            triangles[triIndex++] = baseIndex + 0;
            triangles[triIndex++] = baseIndex + 1;
            triangles[triIndex++] = nextBase + 0;

            triangles[triIndex++] = baseIndex + 1;
            triangles[triIndex++] = nextBase + 1;
            triangles[triIndex++] = nextBase + 0;

            // Inner face
            triangles[triIndex++] = baseIndex + 2;
            triangles[triIndex++] = nextBase + 2;
            triangles[triIndex++] = baseIndex + 3;

            triangles[triIndex++] = baseIndex + 3;
            triangles[triIndex++] = nextBase + 2;
            triangles[triIndex++] = nextBase + 3;

            // Top face
            triangles[triIndex++] = baseIndex + 5;
            triangles[triIndex++] = baseIndex + 7;
            triangles[triIndex++] = nextBase + 5;

            triangles[triIndex++] = baseIndex + 7;
            triangles[triIndex++] = nextBase + 7;
            triangles[triIndex++] = nextBase + 5;

            // Bottom face (if needed for visual completeness)
            triangles[triIndex++] = baseIndex + 4;
            triangles[triIndex++] = nextBase + 4;
            triangles[triIndex++] = baseIndex + 6;

            triangles[triIndex++] = baseIndex + 6;
            triangles[triIndex++] = nextBase + 4;
            triangles[triIndex++] = nextBase + 6;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    void OnValidate()
    {
        if (Application.isPlaying && meshFilter != null)
        {
            GeneratePlatform();
            if (addEdgeRail && edgeRail != null)
            {
                DestroyImmediate(edgeRail);
                GenerateEdgeRail();
            }
            else if (!addEdgeRail && edgeRail != null)
            {
                DestroyImmediate(edgeRail);
            }
        }
    }
}
