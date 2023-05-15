using UnityEngine;

public class RotateHelper : MonoBehaviour {
    private static RotateHelper instance;
    private GameObject toRotate;

    void Awake() {
        if(instance != null) {
            Destroy(gameObject);
        }
        instance = this;
    }

    // Does everything
    public void Rotate(GameObject toSet, Vector3 oldDirection, Vector3 newDirection) {
        SetPosition(toSet.transform.position);      // Set position
        LookAt(oldDirection);                       // Make parent object look at direction of normal to snap
        PrepareRotate(toSet);
        LookAt(newDirection);
        FinishRotate();
    }

    public void PrepareRotate(GameObject obj) {
        toRotate = obj;
        toRotate.transform.parent = transform;
    }

    public void FinishRotate() {
        if(toRotate != null) { 
            toRotate.transform.parent = null;
        }
    }

    public void LookAt(Vector3 target) {
        transform.rotation = Quaternion.LookRotation(target);
    }

    public void LookAt(Transform target) {
        LookAt(target.position);
    }
    
    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    public void SetPosition(GameObject obj) { SetPosition(obj.transform.position); }

    public void SetRotation(Quaternion eul) { transform.rotation = eul; }

    public static RotateHelper Instance { get { return instance; } }
}