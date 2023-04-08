using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class WedgeFactory : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    public float depth = 1f;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        GenerateWedge(); // remove this when you tie it to some menu button in the canvas to generate a 30, 60, 90 triangle.
    }

    private void GenerateWedge()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[6];
        int[] triangles = new int[24];

        Vector3 topVertex = new Vector3(0, Mathf.Sqrt(3) * depth, 0);

        // Front right triangle points of wedge
        vertices[0] = new Vector3(0, 0, 0); // Origin
        vertices[1] = new Vector3(depth, 0, 0); // Right
        vertices[2] = topVertex; // Top

        // Back right triangle points of wedge
        vertices[3] = new Vector3(0, 0, 1);
        vertices[4] = new Vector3(depth, 0, 1);
        vertices[5] = topVertex + new Vector3(0, 0, 1);

        // Triangle array

        // Side Triangle 1
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;

        // Side Triangle 2
        triangles[3] = 3;
        triangles[4] = 4;
        triangles[5] = 5;

        // Square Face part 1
        triangles[6] = 0;
        triangles[7] = 4;
        triangles[8] = 3;
        // Square Face part 2
        triangles[9] = 0;
        triangles[10] = 1;
        triangles[11] = 4;

        // Rectangle Face part 1
        triangles[12] = 3;
        triangles[13] = 5;
        triangles[14] = 0;
        // Rectangle Face part 2
        triangles[15] = 0;
        triangles[16] = 5;
        triangles[17] = 2;

        // Hypotenuse Face part 1
        triangles[18] = 4;
        triangles[19] = 2;
        triangles[20] = 5;
        // Hypotenuse Face part 2
        triangles[21] = 4;
        triangles[22] = 1;
        triangles[23] = 2;

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate mesh normals
        mesh.RecalculateNormals();

        // Assign mesh to MeshFilter
        meshFilter.mesh = mesh;
    }
}