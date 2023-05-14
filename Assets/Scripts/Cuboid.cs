using UnityEngine;

public class Cuboid : MonoBehaviour {
    private bool moving;

    void Awake() { }

    void Update() {
        if(moving) {
            DebugSnapping();
            SetVisualGuide();
        }
    }

    private void CheckSnapping() {
        // TODO: Try to snap between to two objects
        GameController.Instance.VisualGuide.SetActive(false);
        for(int i = 0; i < 6; i++) {    // 6 directions
            if(Physics.Raycast(GetRaycastOrigin(i), GetRaycastDirection(i), out RaycastHit hitInfo, GameController.Instance.CuboidSnapRange, LayerMasks.Object)) {
                transform.position = GetSnapPosition(hitInfo, i);
                // Debug.DrawRay(transform.position, transform.up * 3f, Color.blue, 10f);
                RotateHelper.Instance.Rotate(gameObject, GetRaycastDirection(i), -hitInfo.normal);
                break;
            }
        }
    }

    private void DebugSnapping() {
        // Top, Bottom
        // Debug.DrawRay(transform.position + (transform.up * transform.localScale.y / 2), transform.up * GameController.Instance.CuboidSnapRange, Color.red);
        // Debug.DrawRay(transform.position - (transform.up * transform.localScale.y / 2), -transform.up * GameController.Instance.CuboidSnapRange, Color.red);

        // // Right, Left
        // Debug.DrawRay(transform.position + (transform.right * transform.localScale.x / 2), transform.right * GameController.Instance.CuboidSnapRange, Color.red);
        // Debug.DrawRay(transform.position - (transform.right * transform.localScale.x / 2), -transform.right * GameController.Instance.CuboidSnapRange, Color.red);

        // // Forward, Backward
        // Debug.DrawRay(transform.position + (transform.forward * transform.localScale.z / 2), transform.forward * GameController.Instance.CuboidSnapRange, Color.red);
        // Debug.DrawRay(transform.position - (transform.forward * transform.localScale.z / 2), -transform.forward * GameController.Instance.CuboidSnapRange, Color.red);

        for(int i = 0; i < 6; i++) {    // 6 directions
            Debug.DrawRay(GetRaycastOrigin(i), GetRaycastDirection(i) * GameController.Instance.CuboidSnapRange, Color.blue);
        }
    }

    public Vector3 GetSnapPosition(RaycastHit hitInfo, int index) {
        return hitInfo.point + (hitInfo.normal * GetDistance(index));
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

    private Vector3 GetDistanceVector(int index) {
        int sign = ((index % 2) == 0) ? 1 : -1;
        Vector3 distance = Vector3.zero;
        switch(index / 2) {
            case 0:     // For top, bottom
                distance.y = GetDistance(index);
                break;
            case 1:     // For right, left
                distance.x = GetDistance(index);
                break;
            default:    // For forward, backward
                distance.z = GetDistance(index);
                break;
        }
        return (sign * distance);
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

    private void SetVisualGuide() {
        GameController.Instance.VisualGuide.SetActive(false);
        for(int i = 0; i < 6; i++) {
            if(Physics.Raycast(GetRaycastOrigin(i), GetRaycastDirection(i), out RaycastHit hitInfo, GameController.Instance.CuboidSnapRange, LayerMasks.Object)) {
                GameController.Instance.VisualGuide.SetActive(true);
                GameController.Instance.VisualGuide.SetPosition(GetSnapPosition(hitInfo, i));
                GameController.Instance.VisualGuide.SetRotation(transform.eulerAngles);
                // SetRotation(GameController.Instance.VisualGuide.gameObject, hitInfo, i);
                RotateHelper.Instance.Rotate(GameController.Instance.VisualGuide.gameObject, GetRaycastDirection(i), -hitInfo.normal);
                GameController.Instance.VisualGuide.SetScale(transform.localScale);
                break;
            }
        }
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