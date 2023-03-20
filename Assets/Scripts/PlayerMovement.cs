using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody rbody;
    private LineRenderer leftLineRenderer;
    private LineRenderer rightLineRenderer;
    private InputMaster inputs;
    private Vector3 movementDirection = Vector3.zero;
    private bool teleport;

    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;
    [SerializeField] private float moveSpeed;

    void Awake() {
        rbody = GetComponent<Rigidbody>();
        leftLineRenderer = leftController.GetComponent<LineRenderer>();
        rightLineRenderer = rightController.GetComponent<LineRenderer>();

        inputs = new InputMaster();
        inputs.InGame.Movement.performed += ctx => SetMovementDirection(ctx.ReadValue<Vector2>());
        inputs.InGame.RightTrigger.started += ctx => RightTriggerPressed();
        inputs.InGame.LeftTrigger.started += ctx => Teleport(true);
        inputs.InGame.LeftTrigger.canceled += ctx => Teleport(false);
    }

    void FixedUpdate() { rbody.velocity = (movementDirection * moveSpeed); }

    void Update() { DrawRightRaycast(); }

    private void DrawRightRaycast() {
        rightLineRenderer.SetPosition(0, rightController.transform.position);
        if(Physics.Raycast(rightController.transform.position, rightController.transform.forward, out RaycastHit hitInfo, Mathf.Infinity)) {
            rightLineRenderer.SetPosition(1, hitInfo.point);    // End rightLineRenderer at hit point
        } else {
            rightLineRenderer.SetPosition(1, (rightController.transform.forward * 1000f));      // End rightLineRenderer at max distance 1000
        }
    }

    private void RightTriggerPressed() {
        if(Physics.Raycast(rightController.transform.position, rightController.transform.forward, out RaycastHit hitInfo, Mathf.Infinity)) {
            if(hitInfo.collider.CompareTag("UI")) {
                hitInfo.collider.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    private void SetMovementDirection(Vector2 m) {
        movementDirection.x = m.x;
        movementDirection.z = m.y;

        // Rotate movementDirection based on Camera's angle
        movementDirection = Quaternion.AngleAxis(Camera.main.transform.localEulerAngles.y, Vector3.up) * movementDirection;
    }

    private void Teleport(bool pressed) {
        Debug.DrawRay(leftController.transform.position, leftController.transform.forward, Color.red, Time.deltaTime);
        if(pressed) {
            teleport = true;
            StartCoroutine(TeleportRenderLine());
        } else if(teleport) {
            teleport = false;
            if(Physics.Raycast(leftController.transform.position, leftController.transform.forward, out RaycastHit hitInfo, Mathf.Infinity, (1 << 6))) {
                transform.position = hitInfo.point;
            }
        }
    }

    // To keep rendering line while teleporting
    private IEnumerator TeleportRenderLine() {
        leftLineRenderer.enabled = true;
        while(teleport) {
            leftLineRenderer.SetPosition(0, leftController.transform.position);
            if(Physics.Raycast(leftController.transform.position, leftController.transform.forward, out RaycastHit hitInfo, Mathf.Infinity, (1 << 6))) {
                leftLineRenderer.SetPosition(1, hitInfo.point);     // End leftLineRenderer at hit point
            } else {
                leftLineRenderer.SetPosition(1, (leftController.transform.forward * 1000f));    // End leftLineRenderer at max distance 1000
            }
            yield return null;
        }
        leftLineRenderer.enabled = false;
    }

    void OnEnable() { inputs.Enable(); }
    void OnDisable() { inputs.Disable(); }
}
