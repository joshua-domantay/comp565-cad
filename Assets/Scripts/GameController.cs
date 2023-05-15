using UnityEngine;

public class GameController : MonoBehaviour {
    private static GameController instance;
    private static GameObject player;
    private static PlayerMovement playerMovement;    
    private bool gizmoUIOnObject = true;
    
    [SerializeField] private VisualGuide visualGuide;
    [SerializeField] private float scaleFactor = 1;
    [SerializeField] private float scaleFactorGizmoUI = 0.00126f;
    [SerializeField] private float gizmoRotateSpeed = 0.00126f;
    [SerializeField] private float cuboidSpawnDistance = 1.75f;
    [SerializeField] private float cuboidSnapRange = 0.25f;
    [Header("Player Settings")]
    [SerializeField] private int moveSpeed;                 // Override Player movement speed
    [SerializeField] private int moveSpeedMin;
    [SerializeField] private int moveSpeedMax;
    [SerializeField] private float moveSpeedMultiplier;
    [Header("Gizmo UI Settings")]
    [SerializeField] private Vector3 gizmoUIPositionUser;
    [SerializeField] private float gizmoUIPositionObjectDistance = 0.25f;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        ChangeMoveSpeed(0);
    }

    public int ChangeMoveSpeed(int x) {
        if(x > 0) {
            moveSpeed++;
            if(moveSpeed > moveSpeedMax) { moveSpeed = moveSpeedMax; }
        } else if(x < 0) {
            moveSpeed--;
            if(moveSpeed < moveSpeedMin) { moveSpeed = moveSpeedMin; }
        }
        playerMovement.MoveSpeed = (moveSpeed * moveSpeedMultiplier);
        return moveSpeed;
    }

    public string ChangeGizmoUIPosition(bool x) {
        gizmoUIOnObject = x;
        string val = (gizmoUIOnObject ? "Object" : "User");
        GizmoUI.Instance.SetPosition(val);
        return val;
    }

    public static GameController Instance { get { return instance; } }

    public static GameObject Player { get { return player; } }

    public static PlayerMovement PlayerMovement { get { return playerMovement; } }

    public bool GizmoUIOnObject { get { return gizmoUIOnObject; } }

    public VisualGuide VisualGuide { get { return visualGuide; } }

    public float ScaleFactor { get { return scaleFactor; } }

    public float ScaleFactorGizmoUI { get { return scaleFactorGizmoUI; } }

    public float GizmoRotateSpeed { get { return gizmoRotateSpeed; } }

    public float CuboidSpawnDistance { get { return cuboidSpawnDistance; } }

    public float CuboidSnapRange { get { return cuboidSnapRange; } }

    public Vector3 GizmoUIPositionUser { get { return gizmoUIPositionUser; } }

    public float GizmoUIPositionObjectDistance { get { return gizmoUIPositionObjectDistance; } }
}