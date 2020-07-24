using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ButtonHover : MonoBehaviour {

    public TextMeshProUGUI theText;
    public Color defaultColor;
    public Color hoverColor;
    public Color clickColor;

    private void Start() {
        theText.color = defaultColor;
    }

    public void OnPointerEnter() {
        theText.color = hoverColor;
    }

    public void OnPointerExit() { 
        theText.color = defaultColor;
    }

    public void OnMouseDown() {
        theText.color = clickColor;
    }
}