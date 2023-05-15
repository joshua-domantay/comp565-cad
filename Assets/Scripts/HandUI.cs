using System.Collections;
using UnityEngine;
using TMPro;

public enum HandUIScreen {
    MAIN,
    OBJECTS,
    SETTINGS
}

public class HandUI : MonoBehaviour {
    private static HandUI instance;
    private static HandUIScreen currentScreen;
    private Material materialToUse;
    [SerializeField] private bool animateUI = true;

    [SerializeField] private RectTransform handUIContainer;
    [SerializeField] private float handUIAnimateSpeed;
    [Header("Main Screen")]
    [SerializeField] private GameObject screenMain;
    [Header("Objects Screen")]
    [SerializeField] private GameObject screenObjectSelection;
    [SerializeField] private GameObject[] screensObject;
    [Header("Settings Screen")]
    [SerializeField] private GameObject screenSettings;
    [SerializeField] private TMP_Text settingsMoveSpeed;
    [SerializeField] private TMP_Text settingsGizmoUIPosition;
    [Header("Photon Screen")]
    [SerializeField] private GameObject[] screensPhoton;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        SetScreen(HandUIScreen.MAIN);
        ChangeMoveSpeed(0);
        ChangeGizmoUIPosition(GameController.Instance.GizmoUIOnObject);
    }

    public void ToggleHandUI() { if(animateUI) { StartCoroutine(AnimateHandUI()); } }

    private IEnumerator AnimateHandUI() {
        animateUI = false;
        Vector3 newScale = handUIContainer.localScale;
        float add = handUIContainer.gameObject.activeSelf ? -1f : 1f;
        handUIContainer.gameObject.SetActive(true);
        while(((add == -1f) && (newScale.x > 0f)) || ((add == 1f) && (newScale.x < 1f))) {
            newScale.x += add * Time.deltaTime * handUIAnimateSpeed;
            newScale.y += add * Time.deltaTime * handUIAnimateSpeed;
            if(newScale.x < 0f) {
                newScale.x = 0f;
                newScale.y = 0f;
            } else if(newScale.x > 1.0f) {
                newScale.x = 1.0f;
                newScale.y = 1.0f;
            }
            handUIContainer.localScale = newScale;
            yield return null;
        }
        if(add == -1f) { handUIContainer.gameObject.SetActive(false); }
        animateUI = true;
    }

    // Specific SetScreen
    public void SetScreen(GameObject screen) {
        SetScreensInactive();
        screen.SetActive(true);
    }

    public void SetScreen(HandUIScreen newScreen) {
        currentScreen = newScreen;

        SetScreensInactive();
        switch(currentScreen) {
            case HandUIScreen.OBJECTS:
                screenObjectSelection.SetActive(true);
                break;
            case HandUIScreen.SETTINGS:
                screenSettings.SetActive(true);
                break;
            default:    // HandUIScreen.MAIN
                screenMain.SetActive(true);
                break;
        }
    }

    public void SetScreen(int newScreen) { SetScreen((HandUIScreen) newScreen); }

    public void SetScreensInactive() {
        screenMain.SetActive(false);
        screenObjectSelection.SetActive(false);
        screenSettings.SetActive(false);
        SetScreensObjectInactive();
        SetScreensPhotonInactive();
    }

    private void SetScreensObjectInactive() {
        foreach(GameObject screen in screensObject) { screen.SetActive(false); }
    }

    private void SetScreensPhotonInactive() {
        foreach(GameObject screen in screensPhoton) { screen.SetActive(false); }
    }

    public void SetMaterialToUse(Material mat) {
        materialToUse = mat;
    }

    public void GenerateWedge() {
        WedgeFactory.Instance.GenerateWedge();
    }

    // Settings Screen
    public void ChangeMoveSpeed(int x) { settingsMoveSpeed.text = GameController.Instance.ChangeMoveSpeed(x).ToString(); }

    public void ChangeGizmoUIPosition(bool x) {
        settingsGizmoUIPosition.text = GameController.Instance.ChangeGizmoUIPosition(x);
    }

    public static HandUI Instance { get { return instance; } }

    public Material MaterialToUse { get { return materialToUse; } }
}
