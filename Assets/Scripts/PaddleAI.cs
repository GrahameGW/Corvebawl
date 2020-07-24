using UnityEngine;

public class PaddleAI : Paddle
{
    private float speed = 1f;
    public float resetSpeed = 2f;
    private float speedMagnitude = 0.1f;
    private int bounceVision = 2;
    Vector3 target;
    Vector3 centerPos;

    new public string name = "Computer";

    private void Awake() {
        centerPos = transform.position;
        speed *= speedMagnitude;
        resetSpeed *= speedMagnitude;
    }

    void Update()
    {
        if (!ball) 
            FindBall();

        if (ball.Trajectory.Count > 0) {
            PredictTarget();
        }
            
        if (Game.Instance.InPlay) {
            // move towards predicted target
            if (ballRb.velocity.z >= 0 &&
                Vector3.Distance(transform.position, target) > 0.001f) {
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
            }
            // drift towards center
            else {
                if (Vector3.Distance(transform.position, centerPos) > 0.001f) {
                    transform.position = Vector3.MoveTowards(transform.position, centerPos, Time.deltaTime * resetSpeed);
                }
            }
        }

        ClampPaddlePosition();
    }

    void PredictTarget() {
        Vector3 point;
        if (ball.Trajectory.Count <= bounceVision) {
            point = ball.Trajectory[ball.Trajectory.Count - 1];
        }
        else {
            int depth = (int)bounceVision - 1 > 0 ? (int)bounceVision - 1 : 0;
            point = ball.Trajectory[depth]; 
        }
        target = new Vector3(point.x, point.y, centerPos.z);
    }

    public void UpdateSettings() {
        var newSettings = Game.Instance.Settings;
        speed = newSettings.aiSpeed;
        speedMagnitude = newSettings.aiSpeedMag;
        bounceVision = newSettings.visionDepth;
    }
}
