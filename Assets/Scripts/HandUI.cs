using System.Collections;
using TMPro;
using UnityEngine;

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
    [SerializeField] private GameObject screenObjects;
    [Header("Settings Screen")]
    [SerializeField] private GameObject screenSettings;
    [SerializeField] private TMP_Text settingsMoveSpeed;

    void Start() {
        SetScreen(HandUIScreen.MAIN);
        ChangeMoveSpeed(0);
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
            }
            handUIContainer.localScale = newScale;
            yield return null;
        }
        if(add == -1f) { handUIContainer.gameObject.SetActive(false); }
        animateUI = true;
    }

    public void SetScreen(HandUIScreen newScreen) {
        currentScreen = newScreen;

        screenMain.SetActive(false);
        screenObjects.SetActive(false);
        screenSettings.SetActive(false);
        switch(currentScreen) {
            case HandUIScreen.OBJECTS:
                screenObjects.SetActive(true);
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

    // Settings Screen
    public void ChangeMoveSpeed(int x) { settingsMoveSpeed.text = GameController.Instance.ChangeMoveSpeed(x).ToString(); }
}
