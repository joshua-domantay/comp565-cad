using System.Collections;
using UnityEngine;

public class SelectObjectUI : MonoBehaviour {
    private static SelectObjectUI instance;
    private GameObject selectedObj;
    private bool open;

    [SerializeField] private GameObject canvas;
    [SerializeField] private float animateSpeed;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Update() { }
    // cuboid.transform.localScale = new Vector3(4 * GameController.Instance.ScaleFactor, cLength * GameController.Instance.ScaleFactor, 4 * GameController.Instance.ScaleFactor);

    public void SetUI(GameObject selectedObj, Vector3 pos) {
        transform.position = pos;
        transform.LookAt(Camera.main.transform);
        canvas.transform.localPosition = Vector3.zero;
        this.selectedObj = selectedObj;
        CloseUI();
        StartCoroutine(PositionCanvas());
        StartCoroutine(AnimateCanvas());
    }

    private IEnumerator PositionCanvas() {
        Vector3[] raycastOrigins = GetRaycastOrigins();
        bool clear = true;

        // Raycast once
        foreach(Vector3 origin in raycastOrigins) {
            Debug.DrawRay(origin, canvas.transform.forward * 0.3f, Color.red, 10f);
            if(Physics.Raycast(origin, -canvas.transform.forward, out RaycastHit hitInfo, 0.3f, LayerMasks.Object)) {
                clear = false;
                break;
            }
        }

        while(!clear) {
            raycastOrigins = GetRaycastOrigins();
            clear = true;

            // Move canvas
            canvas.transform.position = (canvas.transform.position + transform.forward * 0.1f);

            // Raycast
            foreach(Vector3 origin in raycastOrigins) {
                if(Physics.Raycast(origin, -canvas.transform.forward, out RaycastHit hitInfo, 0.3f, LayerMasks.Object)) {
                    clear = false;
                    break;
                }
            }
            yield return null;
        }

        open = true;
    }

    private Vector3[] GetRaycastOrigins() {
        return new Vector3[]{
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (canvas.transform.up * 0.1f) + (-canvas.transform.right * 0.05f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (canvas.transform.up * 0.1f) + (-canvas.transform.right * 0.1f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (canvas.transform.up * 0.1f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (canvas.transform.up * 0.1f) + (canvas.transform.right * 0.1f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (canvas.transform.up * 0.1f) + (canvas.transform.right * 0.05f)),

            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (-canvas.transform.right * 0.05f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (-canvas.transform.right * 0.1f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (canvas.transform.right * 0.1f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (canvas.transform.right * 0.05f)),

            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (-canvas.transform.up * 0.1f) + (-canvas.transform.right * 0.05f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (-canvas.transform.up * 0.1f) + (-canvas.transform.right * 0.1f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (-canvas.transform.up * 0.1f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (-canvas.transform.up * 0.1f) + (canvas.transform.right * 0.1f)),
            (canvas.transform.position + (canvas.transform.forward * 0.15f) + (-canvas.transform.up * 0.1f) + (canvas.transform.right * 0.05f)),
        };
    }

    private IEnumerator AnimateCanvas() {
        while(!open) { yield return null; }

        Vector3 newScale = canvas.transform.localScale;
        newScale.z = 0.001f;
        canvas.SetActive(true);
        while(newScale.x < 0.001f) {
            newScale.x += Time.deltaTime * animateSpeed * 0.001f;
            newScale.y += Time.deltaTime * animateSpeed * 0.001f;
            if(newScale.x < 0f) {
                newScale.x = 0f;
                newScale.y = 0f;
            } else if(newScale.x > 0.001f) {
                newScale.x = 0.001f;
                newScale.y = 0.001f;
            }
            canvas.transform.localScale = newScale;
            yield return null;
        }
    }

    public void CloseUI() {
        open = false;
        canvas.transform.localScale = Vector3.zero;
        canvas.SetActive(false);
    }

    public static SelectObjectUI Instance { get { return instance; } }
}