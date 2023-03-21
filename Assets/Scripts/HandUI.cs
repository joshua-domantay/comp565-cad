using TMPro;
using UnityEngine;

public enum HandUIScreen {
    MAIN,
    OBJECTS,
    SETTINGS
}

public class HandUI : MonoBehaviour {
    private static HandUIScreen currentScreen;

    [SerializeField] private GameObject screenMain;
    [SerializeField] private GameObject screenObjects;
    [Header("Settings Screen")]
    [SerializeField] private GameObject screenSettings;
    [SerializeField] private TMP_Text settingsMoveSpeed;

    void Start() {
        SetScreen(HandUIScreen.MAIN);
        ChangeMoveSpeed(0);
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
