using UnityEngine;

public class Cuboid : MonoBehaviour {
    private bool moving;

    void Awake() { }

    void Update() {
        if(moving) {
            // DebugSnapping();
            for(int i = 0; i < 6; i++) {
                Debug.DrawRay(GetRaycastOrigin(i), GetRaycastDirection(i) * GameController.Instance.CuboidSnapRange, Color.red);
            }
        }
    }

    private void CheckSnapping() {
        // TODO: Try to snap between to two objects
        for(int i = 0; i < 6; i++) {    // 6 directions
            if(Physics.Raycast(GetRaycastOrigin(i), GetRaycastDirection(i), out RaycastHit hitInfo, GameController.Instance.CuboidSnapRange, LayerMasks.Object)) {
                Debug.DrawRay(hitInfo.point, hitInfo.normal * GameController.Instance.CuboidSnapRange, Color.blue, 10f);
                break;
            }
        }
    }

    private void DebugSnapping() {
        // Top, Bottom
        Debug.DrawRay(transform.position + (transform.up * transform.localScale.y / 2), transform.up * GameController.Instance.CuboidSnapRange, Color.red);
        Debug.DrawRay(transform.position - (transform.up * transform.localScale.y / 2), -transform.up * GameController.Instance.CuboidSnapRange, Color.red);

        // Right, Left
        Debug.DrawRay(transform.position + (transform.right * transform.localScale.x / 2), transform.right * GameController.Instance.CuboidSnapRange, Color.red);
        Debug.DrawRay(transform.position - (transform.right * transform.localScale.x / 2), -transform.right * GameController.Instance.CuboidSnapRange, Color.red);

        // Forward, Backward
        Debug.DrawRay(transform.position + (transform.forward * transform.localScale.z / 2), transform.forward * GameController.Instance.CuboidSnapRange, Color.red);
        Debug.DrawRay(transform.position - (transform.forward * transform.localScale.z / 2), -transform.forward * GameController.Instance.CuboidSnapRange, Color.red);
    }

    private Vector3 GetDirection(int index) {
        switch(index / 2) {
            case 0:     // For top, bottom
                return transform.up;
            case 1:     // For right, left
                return transform.right;
            default:    // For forward, backward
                return transform.forward;
        }
    }

    private float GetDistance(int index) {
        switch(index / 2) {
            case 0:     // For top, bottom
                return (transform.localScale.y / 2);
            case 1:     // For right, left
                return (transform.localScale.x / 2);
            default:    // For forward, backward
                return (transform.localScale.z / 2);
        }
    }

    private Vector3 GetRaycastOrigin(int index) {
        int sign = ((index % 2) == 0) ? 1 : -1;
        Vector3 direction = GetDirection(index) * GetDistance(index);
        return (transform.position + (sign * direction));
    }

    private Vector3 GetRaycastDirection(int index) {
        int sign = ((index % 2) == 0) ? 1 : -1;
        Vector3 direction = GetDirection(index);
        return (sign * direction);
    }

    public void SetLength(float length) {
        transform.localScale = new Vector3(4 * GameController.Instance.ScaleFactor, length * GameController.Instance.ScaleFactor, 4 * GameController.Instance.ScaleFactor);
    }

    public void SetMoving(bool isMoving) {
        moving = isMoving;
        if(!moving) {
            CheckSnapping();
        }
    }
}