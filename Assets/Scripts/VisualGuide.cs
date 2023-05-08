using UnityEngine;

public class VisualGuide : MonoBehaviour {
    void Awake() { }

    void Update() { }

    public void SetPosition(Vector3 position) { transform.position = position; }

    public void SetRotation(Vector3 rotation) { transform.localEulerAngles = rotation; }

    public void SetScale(Vector3 scale) { transform.localScale = scale; }

    public void SetActive(bool val) { gameObject.SetActive(val); }
}