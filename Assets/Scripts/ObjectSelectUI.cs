using UnityEngine;

public class ObjectSelectUI : MonoBehaviour {
    private int currentScreen;      // 0 = Options, 1 = Length, 2 = Wedge

    [SerializeField] private GameObject screenMain;
    [SerializeField] private GameObject screenOptions;
    [SerializeField] private GameObject screenSetLength;
    [SerializeField] private GameObject screenSetWedge;

    public void SetScreen(int newScreen) {
        currentScreen = newScreen;

        HandUI.Instance.SetScreensInactive();
        SetScreensInactive();
        screenMain.SetActive(true);
        switch(currentScreen) {
            case 0:     // Options
                screenOptions.SetActive(true);
                break;
            case 1:     // Length
                screenSetLength.SetActive(true);
                break;
            default:    // Wedge
                screenSetWedge.SetActive(true);
                break;
        }
    }

    private void SetScreensInactive() {
        screenOptions.SetActive(false);
        screenSetLength.SetActive(false);
        screenSetWedge.SetActive(false);
    }
}