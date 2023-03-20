using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody rbody;
    private LineRenderer lineRenderer;
    private InputMaster inputs;
    private Vector3 movementDirection = Vector3.zero;
    private bool teleport;

    [SerializeField] private GameObject leftController;
    [SerializeField] private float moveSpeed;

    void Awake() {
        rbody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        inputs = new InputMaster();
        inputs.InGame.Movement.performed += ctx => SetMovementDirection(ctx.ReadValue<Vector2>());
        inputs.InGame.Teleport.started += ctx => Teleport(true);
        inputs.InGame.Teleport.canceled += ctx => Teleport(false);
    }

    void FixedUpdate() { rbody.velocity = (movementDirection * moveSpeed); }

    void Update() { }

    private void SetMovementDirection(Vector2 m) {
        movementDirection.x = m.x;
        movementDirection.z = m.y;
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
        lineRenderer.enabled = true;
        while(teleport) {
            lineRenderer.SetPosition(0, leftController.transform.position);
            if(Physics.Raycast(leftController.transform.position, leftController.transform.forward, out RaycastHit hitInfo, Mathf.Infinity, (1 << 6))) {
                lineRenderer.SetPosition(1, hitInfo.point);     // End lineRenderer at hit point
            } else {
                lineRenderer.SetPosition(1, (leftController.transform.forward * 1000f));    // End lineRenderer at max distance 1000
            }
            yield return null;
        }
        lineRenderer.enabled = false;
    }

    void OnEnable() { inputs.Enable(); }
    void OnDisable() { inputs.Disable(); }
}
