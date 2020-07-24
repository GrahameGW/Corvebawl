using UnityEngine;

public class Paddle : MonoBehaviour 
{
    public GameObject Defending { get; private set; }
    public Vector3 Velocity { get; protected set; }

    [SerializeField] GameObject defending = default;

    protected Ball ball;
    protected Rigidbody ballRb;


    void Start() {
        Defending = defending;
    }

    protected void ClampPaddlePosition() {
        var clampedX = Mathf.Clamp(transform.position.x, -0.875f, 0.875f);
        var clampedY = Mathf.Clamp(transform.position.y, -0.465f, 0.465f);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    protected void FindBall() {

        ball = Game.Instance.GameBall;

        if (ball) {
            ballRb = Game.Instance.GameBall.GetComponent<Rigidbody>();
        }
        else {
            return; // break on lack of ball to resolve issues
        }
    }


}
