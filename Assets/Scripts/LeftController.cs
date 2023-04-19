using System.Collections;
using UnityEngine;

public class LeftController : MonoBehaviour, IControllerInputs {
    private GameObject parent;
    private LineRenderer renderLine;
    private bool teleport;

    void Awake() { renderLine = GetComponent<LineRenderer>(); }

    // To keep rendering line while teleporting
    private IEnumerator TeleportRenderLine() {
        renderLine.enabled = true;
        while(teleport) {
            renderLine.SetPosition(0, transform.position);
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, Mathf.Infinity, LayerMasks.Floor)) {
                renderLine.SetPosition(1, hitInfo.point);       // End renderLine at hit point
            } else {
                renderLine.SetPosition(1, (transform.forward * 1000f));     // End renderLine at max distance 1000
            }
            yield return null;
        }
        renderLine.enabled = false;
    }

    // IControllerInputs methods
    public void TriggerPress(bool pressed) {
        Debug.DrawRay(transform.position, transform.forward, Color.red, Time.deltaTime);
        if(pressed) {       // Start teleport process
            teleport = true;
            StartCoroutine(TeleportRenderLine());
        } else if(teleport) {       // Teleport
            teleport = false;
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, Mathf.Infinity, LayerMasks.Floor)) {
                parent.transform.position = hitInfo.point;
            }
        }
    }

    public void GripPress(bool pressed) { HandUI.Instance.ToggleHandUI(); }

    public void PrimaryPress(bool pressed) { }

    public void SecondaryPress(bool pressed) { }

    public void MenuPress(bool pressed) { }

    public GameObject Parent { set { parent = value; } }
}