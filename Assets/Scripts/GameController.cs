using UnityEngine;

public class GameController : MonoBehaviour {
    private static GameController instance;
    private static GameObject player;
    private PlayerMovement playerMovement;
    
    // Override PlayerMovement speed
    [SerializeField] private int moveSpeed;
    [SerializeField] private int moveSpeedMin;
    [SerializeField] private int moveSpeedMax;
    [SerializeField] private float moveSpeedMultiplier;

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

    public static GameController Instance { get { return instance; } }

    public static GameObject Player { get { return player; } }
}