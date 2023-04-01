using UnityEngine;
using UnityEngine.UI;

public class RightController : MonoBehaviour, IControllerInputs {
    private GameObject parent;
    private LineRenderer renderLine;
    private bool moveObject;
    private GameObject objectToMove;

    void Awake() { renderLine = GetComponent<LineRenderer>(); }

    void Update() { DrawRaycast(); }

    private void DrawRaycast() {
        renderLine.SetPosition(0, transform.position);
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, Mathf.Infinity)) {
            renderLine.SetPosition(1, hitInfo.point);       // End LineRenderer at hit point
        } else {
            renderLine.SetPosition(1, (transform.forward * 1000f));     // End LineRenderer at max distance 1000
        }
    }

    private void SetMoveObject(GameObject toMove) {
        moveObject = true;
        objectToMove = toMove;
        objectToMove.GetComponent<Collider>().isTrigger = true;
        objectToMove.transform.SetParent(transform);
    }

    // IControllerInputs methods
    public void TriggerPress(bool pressed) {
        if(pressed) {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, Mathf.Infinity)) {
                if(hitInfo.collider.CompareTag("UI")) {
                    hitInfo.collider.GetComponent<Button>().onClick.Invoke();
                } else if(hitInfo.collider.CompareTag("Object")) {
                    SetMoveObject(hitInfo.collider.gameObject);
                }
            }
        } else {
            if(moveObject) {        // Moving an object
                moveObject = false;
                objectToMove.GetComponent<Collider>().isTrigger = false;
                objectToMove.transform.SetParent(null);
            }
        }
    }

    public void GripPress(bool pressed) { }

    public void PrimaryPress(bool pressed) { }

    public void SecondaryPress(bool pressed) { }

    public void MenuPress(bool pressed) { }

    public GameObject Parent { set { parent = value; } }
}