using UnityEngine;
using TMPro;

public class NumpadUI : MonoBehaviour {
    private bool hasPoint;

    [SerializeField] private TMP_Text textToSet;
    [SerializeField] private int[] textLengthMin;
    [SerializeField] private float[] textSize;

    private void Backspace() {
        string text = textToSet.text;
        if(textToSet.text.Substring(text.Length - 1) == ".") { hasPoint = false; }
        text = text.Substring(0, text.Length - 1);
        if(text.Length == 0) { text = "0"; }
        textToSet.text = text;
    }

    private void CheckTextSize() {
        int index = 0;
        for(int i = 0; i < textLengthMin.Length; i++) {
            if(textToSet.text.Length < textLengthMin[i]) {
                break;
            } else {
                index++;
            }
        }
        textToSet.fontSize = textSize[index];
    }

    public void NumpadPress(int val) {
        switch(val) {
            case -1:    // Point
                if(!hasPoint) {
                    hasPoint = true;
                    textToSet.text += ".";
                }
                break;
            case -2:    // Backspace
                Backspace();
                break;
            default:
                if(textToSet.text == "0") {
                    textToSet.text = val.ToString();
                } else {
                    textToSet.text += val;
                }
                break;
        }
        CheckTextSize();
        Debug.Log(float.Parse(textToSet.text));
    }
}