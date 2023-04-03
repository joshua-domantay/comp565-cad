using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody rbody;
    private InputMaster inputs;
    private Vector3 movementDirection = Vector3.zero;

    [SerializeField] private RightController rightController;
    [SerializeField] private LeftController leftController;
    [SerializeField] private float moveSpeed;   // Overridden by GameController

    void Awake() {
        rbody = GetComponent<Rigidbody>();
        SetInputs();
        rightController.Parent = gameObject;
        leftController.Parent = gameObject;
    }

    void FixedUpdate() { rbody.velocity = (movementDirection * moveSpeed); }

    private void SetInputs() {
        inputs = new InputMaster();
        inputs.InGame.Movement.performed += ctx => SetMovementDirection(ctx.ReadValue<Vector2>());

        // Right controller inputs
        inputs.InGame.RightTrigger.started += ctx => rightController.TriggerPress(true);
        inputs.InGame.RightTrigger.canceled += ctx => rightController.TriggerPress(false);
        inputs.InGame.RightGrip.started += ctx => rightController.GripPress(true);
        inputs.InGame.RightGrip.canceled += ctx => rightController.GripPress(false);
        inputs.InGame.RightPrimary.started += ctx => rightController.PrimaryPress(true);
        inputs.InGame.RightPrimary.canceled += ctx => rightController.PrimaryPress(false);
        inputs.InGame.RightSecondary.started += ctx => rightController.SecondaryPress(true);
        inputs.InGame.RightSecondary.canceled += ctx => rightController.SecondaryPress(false);
        inputs.InGame.RightMenu.started += ctx => rightController.MenuPress(true);
        inputs.InGame.RightMenu.canceled += ctx => rightController.MenuPress(false);

        // Left controller inputs
        inputs.InGame.LeftTrigger.started += ctx => leftController.TriggerPress(true);
        inputs.InGame.LeftTrigger.canceled += ctx => leftController.TriggerPress(false);
        inputs.InGame.LeftGrip.started += ctx => leftController.GripPress(true);
        inputs.InGame.LeftGrip.canceled += ctx => leftController.GripPress(false);
        inputs.InGame.LeftPrimary.started += ctx => leftController.PrimaryPress(true);
        inputs.InGame.LeftPrimary.canceled += ctx => leftController.PrimaryPress(false);
        inputs.InGame.LeftSecondary.started += ctx => leftController.SecondaryPress(true);
        inputs.InGame.LeftSecondary.canceled += ctx => leftController.SecondaryPress(false);
        inputs.InGame.LeftMenu.started += ctx => leftController.MenuPress(true);
        inputs.InGame.LeftMenu.canceled += ctx => leftController.MenuPress(false);
    }

    private void SetMovementDirection(Vector2 m) {
        movementDirection.x = m.x;
        movementDirection.z = m.y;

        // Rotate movementDirection based on Camera's angle
        movementDirection = Quaternion.AngleAxis(Camera.main.transform.localEulerAngles.y, Vector3.up) * movementDirection;
    }

    public float MoveSpeed { set { moveSpeed = value; } }

    void OnEnable() { inputs.Enable(); }
    void OnDisable() { inputs.Disable(); }
}
