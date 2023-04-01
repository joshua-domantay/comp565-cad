using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WedgeFactory : MonoBehaviour
{
    public GameObject wedgePrefab;
    public TMP_InputField angleInputField;

    public void GenerateWedge()
    {
        float angle = float.Parse(angleInputField.text);
        float scaleFactor = Mathf.Tan(angle * Mathf.Deg2Rad / 2f); // only interested in 1/2 the angle when computing scale factor for a wedge, hence divide by 2f

        Vector3 position = new Vector3(0f, 0f, 0f);
        Quaternion rotation = Quaternion.Euler(0f, -angle / 2f, 0f);
        GameObject wedge = Instantiate(wedgePrefab, position, rotation);

        Vector3 scale = wedge.transform.localScale;
        scale.x = scaleFactor * 2f;
        scale.z = scaleFactor * 2f;
        wedge.transform.localScale = scale;

        MeshRenderer meshRenderer = wedge.GetComponent<MeshRenderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.red;
        meshRenderer.material = material;
    }
}
