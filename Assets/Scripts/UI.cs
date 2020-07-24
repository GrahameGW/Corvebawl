using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance { get; set; }
    Game game;

    [SerializeField] TextMeshProUGUI title = default;
    [SerializeField] TextMeshProUGUI playerScore = default;
    [SerializeField] TextMeshProUGUI compScore = default;

    [SerializeField] GameObject endlessModeBtn = default;
    [SerializeField] GameObject endlessDifficultyOpts = default;
    [SerializeField] GameObject resumeBtn = default;

    // Start is called before the first frame update
    void Start()
    {
        game = Game.Instance;
        Instance = this;
        playerScore.enabled = false;
        compScore.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerScore.text = game.playerScore.ToString();
        compScore.text = game.compScore.ToString();
    }

    public void HideTitle() {
        title.enabled = false;
        playerScore.enabled = true;
        compScore.enabled = true;
        endlessDifficultyOpts.SetActive(false);
        endlessModeBtn.SetActive(false);
        resumeBtn.SetActive(false);
    }

    public void ShowTitle() {
        title.enabled = true;
        //endlessDifficultyOpts.SetActive(true);
        endlessModeBtn.SetActive(true);
        resumeBtn.SetActive(true);
    }

    public void ToggleDifficulty() {
        if (endlessDifficultyOpts.activeInHierarchy)
            endlessDifficultyOpts.SetActive(false);
        else
            endlessDifficultyOpts.SetActive(true);
    }

}
