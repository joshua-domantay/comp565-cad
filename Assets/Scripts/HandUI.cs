using System.Collections;
using UnityEngine;
using TMPro;

public enum HandUIScreen {
    MAIN,
    OBJECTS,
    SETTINGS
}

public class HandUI : MonoBehaviour {
    private static HandUIScreen currentScreen;
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

    void Start() {
        SetScreen(HandUIScreen.MAIN);
        ChangeMoveSpeed(0);
    }

    public void ToggleHandUI() { if(animateUI) { StartCoroutine(AnimateHandUI()); } }

    public void SetGameObject(GameObject x) { }

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
            }
            handUIContainer.localScale = newScale;
            yield return null;
        }
        if(add == -1f) { handUIContainer.gameObject.SetActive(false); }
        animateUI = true;
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
    }

    private void SetScreensObjectInactive() {
        foreach(GameObject screen in screensObject) { screen.SetActive(false); }
    }

    // Settings Screen
    public void ChangeMoveSpeed(int x) { settingsMoveSpeed.text = GameController.Instance.ChangeMoveSpeed(x).ToString(); }
}
