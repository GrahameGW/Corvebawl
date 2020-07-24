using UnityEngine.InputSystem;
using UnityEngine;

public class Game : MonoBehaviour 
{
    public static Game Instance { get; private set; }
    public Ball GameBall { get; set; }

    [SerializeField] Ball ballPrefab = default;
    [SerializeField] Paddle player = default;
    [SerializeField] Paddle computer = default;
    [SerializeField] GameSettings[] settingsPrefab = default;

    UI ui = default;
    Vector3 launchPoint = new Vector3(0, 0, -1.4f);

    // State
    public bool Paused { get; private set; }
    public bool InPlay { get; private set; }
    public GameSettings Settings { get; private set; }

    public int playerScore = 0;
    public int compScore = 0;

    bool servePrepped = false;

    void Awake() {
        Instance = this;
        Paused = true;
        InPlay = false;
        Cursor.visible = true;
        launchPoint = transform.localPosition + launchPoint;
    }

    private void Start() {
        ui = UI.Instance;
        TogglePaddles(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();

        if (!Paused)
            Mouse.current.WarpCursorPosition(new Vector2(670, 380));
    }

    public void StartGame() {
        if (GameBall) {
            Destroy(GameBall.gameObject);
            GameBall = null;
        };
        ResetScore();
        InPlay = false;
        TogglePause();
        TogglePaddles(true);
        PrepServe();
    }

    public void TogglePause() {
        Paused = !Paused;
        // UI.Instance.menusActive = !UI.Instance.menusActive;

        if (Paused) {
            Time.timeScale = 0; // freezes physics and game clock
            Cursor.visible = true;
            TogglePaddles(false);
            ui.ShowTitle();
        }
        else {
            Time.timeScale = 1;
            Cursor.visible = false;
            TogglePaddles(true);
            ui.HideTitle();
        }

        if (!Paused && !Settings) {
            if (settingsPrefab[0]) ChooseSettings(0);
            else throw new System.Exception("No settings module available");
        }
    }

    void TogglePaddles(bool state) {
        player.gameObject.SetActive(state);
        computer.gameObject.SetActive(state);
    }

    public void Serve() {
        if (!InPlay && !servePrepped) {
            PrepServe();
            return;
        }

        Collider paddleCol = player.GetComponent<Collider>();
        Vector3 paddleCtr = player.gameObject.transform.position;
        PaddlePlayer paddle = player.GetComponent<PaddlePlayer>();

        float paddleX = paddleCol.bounds.extents.x;
        float paddleY = paddleCol.bounds.extents.y;

        if (launchPoint.x <= paddleCtr.x + paddleX && launchPoint.x >= paddleCtr.x - paddleX &&     // between x direction
            launchPoint.y <= paddleCtr.y + paddleY && launchPoint.y >= paddleCtr.y - paddleY)       // between y direction
            {
            GameBall.Serve(paddle.Velocity);

            InPlay = true;
            servePrepped = false;
        }
    }

    void PrepServe() {
        if (GameBall) {
            Destroy(GameBall.gameObject);
            GameBall = null;
        };

        GameBall = Instantiate(ballPrefab, launchPoint, Quaternion.identity, transform);
        servePrepped = true;
        computer.Defending.GetComponent<Goal>().HideGoalLines();
    }

    public void Score(GameObject goal) {
        if (player.Defending == goal) {
            print("Computer Scores!");
            compScore++;

        }
        else if (computer.Defending == goal) {
            print("Player Scores!");
            playerScore++;
            computer.Defending.GetComponent<Goal>().ShowGoalLines(GameBall.gameObject.transform.position);
        }
        else {
            Debug.LogError("Issue with goal scoring!");
        }
        GameBall.Score();
        InPlay = false;
    }

    public void ChooseSettings(int id) {
        Settings = settingsPrefab[id];
        computer.GetComponent<PaddleAI>().UpdateSettings();
    }

    private void ResetScore() {
        playerScore = 0;
        compScore = 0;
    }
}
