using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboidFactory4x4 : MonoBehaviour
{
    public GameObject cuboidPrefab;
    public float length;

    public void CreateCuboid(float cLength)
    {
        if(cLength != 0) {
            Vector3 position = new Vector3(0f, (cLength * GameController.Instance.ScaleFactor) / 2, 0f);
            GameObject cuboid = Instantiate(cuboidPrefab, position, Quaternion.identity);
            cuboid.tag = "Object";
            cuboid.transform.localScale = new Vector3(4 * GameController.Instance.ScaleFactor, cLength * GameController.Instance.ScaleFactor, 4 * GameController.Instance.ScaleFactor);
            cuboid.layer = LayerMask.NameToLayer("Object");
            cuboid.SetActive(true);

            // Could be better
            Vector3 cameraForwardModified = Camera.main.transform.forward;
            cameraForwardModified.y = 0f;
            cameraForwardModified = Camera.main.transform.position + (cameraForwardModified.normalized * GameController.Instance.CuboidSpawnDistance);
            cameraForwardModified.y = 0f;
            cuboid.transform.position += cameraForwardModified;

            // Better y position so user can see cuboid spawn
            cameraForwardModified = cuboid.transform.position;
            cameraForwardModified.y = Camera.main.transform.position.y;
            cuboid.transform.position = cameraForwardModified;
        }
    }
}
