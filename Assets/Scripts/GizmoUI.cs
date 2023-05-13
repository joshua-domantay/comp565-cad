using System.Collections;
using TMPro;
using UnityEngine;

public class GizmoUI : MonoBehaviour {
    private static GizmoUI instance;
    private GameObject selectedObj;
    private Quaternion selectedObjLastRotation;
    private Vector3 gizmoLookAtLastPos;
    private GameObject rotateAxisGO;
    private int rotateAxis;     // x = 0, y = 1, z = 2
    private bool open;
    private bool localRotate;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject gizmoLookAt;
    [SerializeField] private GameObject[] gizmoRotations;
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
        newScale.x = Vector3.Distance(canvas.transform.position, Camera.main.transform.position) * GameController.Instance.ScaleFactorGizmoUI;
        newScale.y = Vector3.Distance(canvas.transform.position, Camera.main.transform.position) * GameController.Instance.ScaleFactorGizmoUI;
        newScale.z = 0.001f;
        canvas.transform.localScale = newScale;

        RotateAxis();
    }

    // Maintain selected object
    public void SetUI() {
        canvas.transform.localPosition = Vector3.zero;
        gizmoLookAt.transform.position = selectedObj.transform.position;

        CloseUI();
        screenOptions.SetActive(true);
        changeLengthText.text = (selectedObj.transform.localScale.y / GameController.Instance.ScaleFactor).ToString();
        StartCoroutine(PositionCanvas());
        // StartCoroutine(AnimateCanvas());
        canvas.SetActive(true);

        SetGizmoRotations(true);
    }

    public void SetUI(GameObject selectedObj, Vector3 pos) {
        transform.position = pos;
        canvas.transform.localPosition = Vector3.zero;
        this.selectedObj = selectedObj;
        gizmoLookAt.transform.position = selectedObj.transform.position;

        CloseUI();
        screenOptions.SetActive(true);
        changeLengthText.text = (selectedObj.transform.localScale.y / GameController.Instance.ScaleFactor).ToString();
        StartCoroutine(PositionCanvas());
        // StartCoroutine(AnimateCanvas());
        canvas.SetActive(true);

        SetGizmoRotations(true);
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

    private void SetGizmoRotations(bool active) {
        foreach(GameObject gizmo in gizmoRotations) {
            gizmo.SetActive(active);
            gizmo.transform.position = selectedObj.transform.position;
            gizmo.transform.rotation = (localRotate ? selectedObj.transform.rotation : Quaternion.Euler(Vector3.zero));
        }
    }

    public void CloseUI() {
        SetGizmoRotations(false);
        GameController.Instance.VisualGuide.SetActive(false);

        open = false;
        canvas.transform.localScale = Vector3.zero;
        canvas.SetActive(false);
        screenOptions.SetActive(false);
        screenChangeLength.SetActive(false);
    }

    private void RotateAxis() {
        if(gizmoLookAt.transform.parent != null) {
            Vector3 newDirection = gizmoLookAt.transform.position - selectedObj.transform.position;
            Vector3 oldDirection = gizmoLookAtLastPos - selectedObj.transform.position;
            Quaternion rotation = Quaternion.FromToRotation(oldDirection, newDirection);

            newDirection = Vector3.zero;
            switch(rotateAxis) {
                case 0:     // X
                    newDirection.x = rotation.eulerAngles.x;
                    break;
                case 1:     // Y
                    newDirection.y = rotation.eulerAngles.y;
                    break;
                default:    // Z
                    newDirection.z = rotation.eulerAngles.z;
                    break;
            }
            selectedObj.transform.rotation = Quaternion.Euler(selectedObjLastRotation.eulerAngles + newDirection);

            // if(localRotate) {
            //     rotation = Quaternion.Euler(selectedObjLastRotation.eulerAngles + newDirection);
            // } else {
            //     selectedObj.transform.rotation = Quaternion.Euler(selectedObjLastRotation.eulerAngles + newDirection);
            // }
        }
    }

    public void RotateObject(GameObject target, Vector3 pos) {
        selectedObjLastRotation = selectedObj.transform.rotation;
        gizmoLookAt.transform.position = pos;
        gizmoLookAtLastPos = pos;

        CloseUI();
        target.transform.parent.gameObject.SetActive(true);

        rotateAxisGO = target.transform.parent.gameObject;
        string[] targetName = rotateAxisGO.name.ToUpper().Split(" ");
        rotateAxis = (targetName[targetName.Length - 1].ToCharArray()[0] - 'X');

        GameController.PlayerMovement.RightController.SetMoveObject(gizmoLookAt);
    }

    public void SetScreen(int x) {
        GameController.Instance.VisualGuide.SetActive(false);

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
        GameController.Instance.VisualGuide.SetActive(false);

        float newLength = float.Parse(changeLengthText.text);
        if(newLength <= 0f) {
            RemoveObject();
            return;
        }

        selectedObj.GetComponent<Cuboid>().SetLength(newLength);
    }

    public void SetVisualGuideLength() {
        GameController.Instance.VisualGuide.SetActive(true);
        GameController.Instance.VisualGuide.SetPosition(selectedObj.transform.position);
        Vector3 scale = selectedObj.transform.localScale;
        scale.y = float.Parse(changeLengthText.text) * GameController.Instance.ScaleFactor;
        GameController.Instance.VisualGuide.SetScale(scale);
    }

    public void RemoveObject() {
        CloseUI();
        Destroy(selectedObj);
    }

    public static GizmoUI Instance { get { return instance; } }
}