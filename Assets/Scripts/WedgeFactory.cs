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
        Vector3[] vertices = new Vector3[8];
        int[] triangles = new int[30];

        // Vertices
        Vector3 topVertex = new Vector3(0, Mathf.Sqrt(3) * depth, 0);

        vertices[0] = new Vector3(0, 0, 0); // Origin
        vertices[1] = new Vector3(depth, 0, 0); // Right
        vertices[2] = topVertex; // Top

        vertices[3] = new Vector3(0, 0, 1);
        vertices[4] = new Vector3(depth, 0, 1);
        vertices[5] = topVertex + new Vector3(0, 0, 1);

        vertices[6] = new Vector3(0, 0, 1); // Back left
        vertices[7] = topVertex + new Vector3(0, 0, 1); // Back right

        // Triangles
        // Front
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        // Back
        triangles[3] = 3;
        triangles[4] = 5;
        triangles[5] = 4;

        // Bottom
        triangles[6] = 0;
        triangles[7] = 3;
        triangles[8] = 1;

        triangles[9] = 1;
        triangles[10] = 3;
        triangles[11] = 4;

        // Top
        triangles[12] = 2;
        triangles[13] = 5;
        triangles[14] = 7;

        triangles[15] = 5;
        triangles[16] = 2;
        triangles[17] = 1;

        // Right
        triangles[18] = 1;
        triangles[19] = 4;
        triangles[20] = 5;

        // Left
        triangles[21] = 0;
        triangles[22] = 6;
        triangles[23] = 3;

        // Ramp
        triangles[24] = 2;
        triangles[25] = 7;
        triangles[26] = 0;

        triangles[27] = 0;
        triangles[28] = 7;
        triangles[29] = 6;

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate mesh normals
        mesh.RecalculateNormals();

        // Assign mesh to MeshFilter
        meshFilter.mesh = mesh;
    }
}