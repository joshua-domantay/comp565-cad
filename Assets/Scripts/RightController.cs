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

    public void SetMoveObject(GameObject toMove) {
        moveObject = true;
        objectToMove = toMove;
        Collider colliderComp = objectToMove.GetComponent<Collider>();
        if(colliderComp) { colliderComp.isTrigger = true; }
        objectToMove.transform.SetParent(transform);
        Cuboid cuboidComp = objectToMove.GetComponent<Cuboid>();
        if(cuboidComp) { cuboidComp.SetMoving(true); }
    }

    // IControllerInputs methods
    public void TriggerPress(bool pressed) {
        if(pressed) {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, Mathf.Infinity)) {
                if(hitInfo.collider.CompareTag("UI")) {
                    hitInfo.collider.GetComponent<Button>().onClick.Invoke();
                } else if(hitInfo.collider.CompareTag("Object")) {
                    GizmoUI.Instance.SetUI(hitInfo.collider.gameObject, hitInfo.point);
                } else if(hitInfo.collider.CompareTag("GizmoRotate")) {
                    GizmoUI.Instance.RotateObject(hitInfo.collider.gameObject, hitInfo.point);
                } else {
                    GizmoUI.Instance.CloseUI();
                }
            }
        } else {
            if(moveObject) {        // Moving an object
                moveObject = false;
                Collider colliderComp = objectToMove.GetComponent<Collider>();
                if(colliderComp) { colliderComp.isTrigger = false; }
                objectToMove.transform.SetParent(null);
                Cuboid cuboidComp = objectToMove.GetComponent<Cuboid>();
                if(cuboidComp) { cuboidComp.SetMoving(false); }

                if(objectToMove.CompareTag("GizmoLookAt")) { GizmoUI.Instance.SetUI(); }
            }
        }
    }

    public void GripPress(bool pressed) { }

    public void PrimaryPress(bool pressed) { }

    public void SecondaryPress(bool pressed) { }

    public void MenuPress(bool pressed) { }

    public GameObject Parent { set { parent = value; } }
}