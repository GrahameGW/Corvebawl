using UnityEngine;

public class Goal : MonoBehaviour {
    [SerializeField] GoalLine xLine = default;
    [SerializeField] GoalLine yLine = default;

    private void Awake() {
        xLine.gameObject.SetActive(false);
        yLine.gameObject.SetActive(false);
    }

    public void ShowGoalLines(Vector3 impact) {
        xLine.gameObject.SetActive(true);
        yLine.gameObject.SetActive(true);
        xLine.DrawLine(impact);
        yLine.DrawLine(impact);
    }

    public void HideGoalLines() {
        xLine.gameObject.SetActive(false);
        yLine.gameObject.SetActive(false);
    }
}
