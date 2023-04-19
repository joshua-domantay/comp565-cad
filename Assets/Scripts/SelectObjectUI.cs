using System.Collections;
using TMPro;
using UnityEngine;

public class SelectObjectUI : MonoBehaviour {
    private static SelectObjectUI instance;
    private GameObject selectedObj;
    private bool open;

    [SerializeField] private GameObject canvas;
    [SerializeField] private float animateSpeed;
    [SerializeField] private GameObject screenOptions;
    [SerializeField] private GameObject screenChangeLength;
    [SerializeField] private TMP_Text changeLengthText;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Update() {
        // Always look at camera
        transform.LookAt(Camera.main.transform);

        // Always maintain size at any distance
        Vector3 newScale = Vector3.zero;
        newScale.x = Vector3.Distance(canvas.transform.position, Camera.main.transform.position) * GameController.Instance.ScaleFactorSelectObjectUI;
        newScale.y = Vector3.Distance(canvas.transform.position, Camera.main.transform.position) * GameController.Instance.ScaleFactorSelectObjectUI;
        newScale.z = 0.001f;
        canvas.transform.localScale = newScale;
    }

    public void SetUI(GameObject selectedObj, Vector3 pos) {
        transform.position = pos;
        canvas.transform.localPosition = Vector3.zero;
        this.selectedObj = selectedObj;
        CloseUI();
        screenOptions.SetActive(true);
        changeLengthText.text = (selectedObj.transform.localScale.y / GameController.Instance.ScaleFactor).ToString();
        StartCoroutine(PositionCanvas());
        // StartCoroutine(AnimateCanvas());
        canvas.SetActive(true);
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
        screenOptions.SetActive(false);
        screenChangeLength.SetActive(false);
    }

    public void SetScreen(int x) {
        if(x == 0) {    // Options screen
            screenOptions.SetActive(true);
            screenChangeLength.SetActive(false);
        } else {        // Change length screen
            screenOptions.SetActive(false);
            screenChangeLength.SetActive(true);
        }
    }

    public void MoveObject() {
        CloseUI();
        GameController.PlayerMovement.RightController.SetMoveObject(selectedObj);
    }

    public void ChangeLengthObject() {
        float newLength = float.Parse(changeLengthText.text);
        if(newLength <= 0f) {
            RemoveObject();
            return;
        }

        selectedObj.transform.localScale = new Vector3(4 * GameController.Instance.ScaleFactor, newLength * GameController.Instance.ScaleFactor, 4 * GameController.Instance.ScaleFactor);
    }

    public void RemoveObject() {
        CloseUI();
        Destroy(selectedObj);
    }

    public static SelectObjectUI Instance { get { return instance; } }
}