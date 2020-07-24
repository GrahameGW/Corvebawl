using UnityEngine.InputSystem;
using UnityEngine;

public class PaddlePlayer : Paddle
{
    private InputAction moveAction;
    private InputAction serveAction;
    private Vector3 previousPosition;

    new public string name = "Player";
    [SerializeField] float sensitivity = 0.05f;
    [SerializeField] float speed = 0.5f;


    // Start is called before the first frame update
    void Awake()
    {
        moveAction = new InputAction(type: InputActionType.Value, binding: "<Mouse>/delta");
        serveAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        moveAction.Enable();
        serveAction.Enable();
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Game game = Game.Instance;
        if (!game.Paused) {
            // Paddle position = mouse position
            var delta = moveAction.ReadValue<Vector2>();
            delta *= sensitivity;
            var clampedDelta = Vector3.ClampMagnitude(delta, speed);
            transform.Translate(0f, clampedDelta.y, clampedDelta.x);

            ClampPaddlePosition();

            // Serve
            if (!game.InPlay && serveAction.triggered) {
                game.Serve();
            }
        }

        CalculateVelocity();
    }

    private void CalculateVelocity() {
        var moveDist = Vector3.Distance(previousPosition, transform.position);
        var moveDir = (previousPosition - transform.position).normalized;
        Velocity = moveDir * (moveDist / Time.deltaTime);
        previousPosition = transform.position;
    }
}
