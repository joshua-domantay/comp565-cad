using UnityEngine;

public class Cuboid : MonoBehaviour {
    private bool moving;

    void Awake() { }

    void Update() {
        if(moving) {
            // DebugSnapping();
            SetVisualGuide();
        }
    }

    private void CheckSnapping() {
        // TODO: Try to snap between to two objects
        GameController.Instance.VisualGuide.SetActive(false);
        for(int i = 0; i < 6; i++) {    // 6 directions
            if(Physics.Raycast(GetRaycastOrigin(i), GetRaycastDirection(i), out RaycastHit hitInfo, GameController.Instance.CuboidSnapRange, LayerMasks.Object)) {
                // Debug.DrawRay(hitInfo.point, hitInfo.normal * GameController.Instance.CuboidSnapRange, Color.blue, 10f);
                transform.position = GetSnapPosition(hitInfo, i);
                // Debug.DrawRay(transform.position, transform.up * 3f, Color.blue, 10f);
                SetRotation(hitInfo, i);
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

    private void SetRotation(RaycastHit hitInfo, int index) {
        // transform.LookAt(hitInfo.point);

        // HERE LAST: Maybe use up vector
        // Vector3 direction = hitInfo.point - transform.position;
        // Quaternion rotation = Quaternion.LookRotation(direction);
        // switch(index / 2) {
        //     case 0:     // For top, bottom
        //         rotation *= Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        //         break;
        //     case 1:     // For right, left
        //         rotation *= Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        //         break;
        //     default:    // For forward, backward
        //         rotation *= Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        //         break;
        // }
        // transform.rotation = rotation;

        // transform.LookAt(hitInfo.point, Vector3.up);




        // Mesh mesh1 = GetComponent<MeshFilter>().mesh;
        // Vector3[] normals = mesh1.normals;
        // Color[] c = new Color[]{
        //     Color.black, Color.blue,
        //     Color.green, Color.yellow,
        //     Color.magenta, Color.cyan
        // };

        // // The first normal in the array corresponds to the first vertex of the mesh
        // // Draw a ray to visualize the normal vector
        // for(int i = 0; i < normals.Length; i++) {
        //     Debug.Log(normals.Length + " : " + i);
        //     Debug.DrawRay(transform.position, normals[i], c[i], 10f);
        // }


        // Vector3 targetPosition = hitInfo.point;
        // Vector3 normal = GetRaycastDirection(index);
        // Quaternion rotation = Quaternion.LookRotation(normal, Vector3.up);
        // transform.LookAt(targetPosition, rotation * Vector3.up);

        // Vector3[] vectors = new Vector3[] {
        //     transform.up,           -transform.up,
        //     transform.right,        -transform.right,
        //     transform.forward,      -transform.forward
        // };
        // Vector3 closestUpVector = vectors[0];

        // float maxDotProduct = Vector3.Dot(vectors[0], Vector3.up);
        // for (int i = 1; i < vectors.Length; i++)
        // {
        //     float dotProduct = Vector3.Dot(vectors[i], Vector3.up);
        //     if (dotProduct > maxDotProduct)
        //     {
        //         maxDotProduct = dotProduct;
        //         closestUpVector = vectors[i];
        //     }
        // }

        // Vector3 direction = hitInfo.point - transform.position;
        // Quaternion rotation = Quaternion.LookRotation(direction);
        // switch(index / 2) {
        //     case 0:     // For top, bottom
        //         rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        //         break;
        //     case 1:     // For right, left
        //         rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        //         break;
        //     default:    // For forward, backward
        //         rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        //         break;
        // }

        // transform.LookAt(hitInfo.point, closestUpVector);
        // transform.rotation *= rotation;
    }

    private void SetVisualGuide() {
        GameController.Instance.VisualGuide.SetActive(false);
        for(int i = 0; i < 6; i++) {
            if(Physics.Raycast(GetRaycastOrigin(i), GetRaycastDirection(i), out RaycastHit hitInfo, GameController.Instance.CuboidSnapRange, LayerMasks.Object)) {
                GameController.Instance.VisualGuide.SetActive(true);
                GameController.Instance.VisualGuide.SetPosition(GetSnapPosition(hitInfo, i));
                GameController.Instance.VisualGuide.SetRotation(transform.eulerAngles);         // TODO: Change value to resulting rotation after snap
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