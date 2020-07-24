using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpeakerToggle : MonoBehaviour
{
    [SerializeField] Sprite speakerOn = default;
    [SerializeField] Sprite speakerOff = default;

    Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    public void ToggleMute() {
        if (Game.Instance.Paused) {
            var listener = Camera.main.GetComponent<AudioListener>();

            listener.enabled = !listener.enabled;

            image.sprite = listener.enabled ? speakerOn : speakerOff;
        }
    }
}
