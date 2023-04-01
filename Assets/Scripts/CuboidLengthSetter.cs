using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CuboidLengthSetter : MonoBehaviour
{
    public CuboidFactory4x4 cuboidFactory;
    public TMP_InputField inputField;

    public void SetCuboidLength()
    {
        float length = float.Parse(inputField.text);
        // cuboidFactory.length = length;
        cuboidFactory.CreateCuboid(length);
    }
}
