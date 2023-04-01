using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboidFactory4x4 : MonoBehaviour
{
    public GameObject cuboidPrefab;
    public float length;

    void Start()
    {
        // Vector3 position = new Vector3(1.5f, length / 2, 1.5f);
        // GameObject cuboid = Instantiate(cuboidPrefab, position, Quaternion.identity);
        // cuboid.transform.localScale = new Vector3(4, length, 4);
    }

    public void CreateCuboid(float cLength) {
        Vector3 position = new Vector3(1.5f, cLength / 2, 1.5f);
        GameObject cuboid = Instantiate(cuboidPrefab, position, Quaternion.identity);
        cuboid.tag = "Object";
        cuboid.transform.localScale = new Vector3(4, cLength, 4);
    }
}
