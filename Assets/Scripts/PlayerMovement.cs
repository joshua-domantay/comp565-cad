using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody rbody;
    private InputMaster inputs;
    private Vector3 movementDirection = Vector3.zero;

    [SerializeField] private float moveSpeed;

    void Awake() {
        rbody = GetComponent<Rigidbody>();
        inputs = new InputMaster();
        inputs.InGame.Movement.performed += ctx => SetMovementDirection(ctx.ReadValue<Vector2>());
    }

    void FixedUpdate() { rbody.velocity = (movementDirection * moveSpeed); }

    void Update() { }

    private void SetMovementDirection(Vector2 m) {
        movementDirection.x = m.x;
        movementDirection.z = m.y;
    }

    void OnEnable() { inputs.Enable(); }
    void OnDisable() { inputs.Disable(); }
}
