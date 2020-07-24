using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    public List<Vector3> Trajectory { get; private set; }
    public float Radius { get; private set; }


    Rigidbody ballRb = default;

    // Don't have default clips since these play once and then ball is respawned
    AudioSource audioSource;
    [SerializeField] Material scoreMaterial = default;
    [SerializeField] AudioClip scoreClip = default;
    [SerializeField] float bounceVolume = 0.7f;
    [SerializeField] float scoreVolume = 0.5f;

    float shotStrength;
    float curveMultiplier;
    float minShotSpeed;
    float maxShotSpeed;

    public const float ANGLE_CLAMP = 30f;


    private void Awake() {
        ballRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        var smallestScale = Mathf.Min(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        Radius = GetComponent<SphereCollider>().radius * smallestScale;
        Trajectory = new List<Vector3>();
    }

    private void Start() {
        GetSettings();
    }

    private void FixedUpdate() {
        var magnus = GetMagnusForce();
        ballRb.AddForce(magnus);
        CalculateTrajectory();
        if (Game.Instance.InPlay) {
            ClampVelocity();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        foreach (Vector3 t in Trajectory) {
            Gizmos.DrawSphere(t, 0.05f);
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == "Goal") {
            var goal = collision.gameObject;
            Game.Instance.Score(goal);
        }
        else { 
            audioSource.PlayOneShot(audioSource.clip, bounceVolume);
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.tag == "Paddle") {
            var paddle = collision.gameObject.GetComponent<Paddle>();
            ClampShotAngle();
            ballRb.velocity += ballRb.velocity.normalized * shotStrength;

            ballRb.AddTorque(paddle.Velocity * curveMultiplier, ForceMode.Impulse);
        } 
    }

    public void Score() {
        Renderer rend = GetComponent<Renderer>();
        rend.material = scoreMaterial;
        Freeze();

        audioSource.PlayOneShot(scoreClip, scoreVolume);
    }

    public void Serve(Vector3 spinVector) {
        ballRb.velocity = new Vector3(0f, 0f, minShotSpeed);
        ballRb.AddForce(ballRb.velocity.normalized * shotStrength, ForceMode.Impulse);
        ballRb.AddTorque(spinVector * curveMultiplier, ForceMode.Impulse);

        audioSource.PlayOneShot(audioSource.clip, bounceVolume);

        var ran = Random.Range(1, 2);
        if (Time.frameCount % 2 == 0)
            ballRb.angularVelocity += new Vector3(ran, 0f, 0f);
        else
            ballRb.angularVelocity += new Vector3(0f, ran, 0f);
    }

    private Vector3 GetMagnusForce() {
        return ballRb.drag * curveMultiplier * Vector3.Cross(ballRb.angularVelocity, ballRb.velocity);
    }

    private void CalculateTrajectory() {
        Trajectory.Clear();
        Ray ray = new Ray(transform.position, ballRb.velocity.normalized);
        RaycastHit hit;
        bool endOfCourt = false;
        int sentinel = 1000;

        while (!endOfCourt && sentinel > 0) {
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.tag == "Wall") {
                    Trajectory.Add(hit.point);
                    var nextDir = Vector3.Reflect(ray.direction, hit.normal);
                    ray = new Ray(hit.point, nextDir);
                }
                else if (hit.collider.gameObject.tag == "Goal") {
                    Trajectory.Add(hit.point);
                    endOfCourt = true;
                }
                else {
                    Trajectory.Add(hit.point); // only other object is paddle, so we count as end
                    endOfCourt = true;
                }
            }

            sentinel--;
        }
    }

    private void ClampShotAngle() {

        var x = Mathf.Abs(ballRb.velocity.x);
        var y = Mathf.Abs(ballRb.velocity.y);
        var z = Mathf.Abs(ballRb.velocity.z);

        var angleMod = ANGLE_CLAMP / 45f;

        if (z < (angleMod * x)) {
            x = z * Mathf.Sign(angleMod * ballRb.velocity.x);
        }
        else {
            x = ballRb.velocity.x;
        }
        if (z < (angleMod * y)) {
            y = z * Mathf.Sign(angleMod * ballRb.velocity.y);
        }
        else {
            y = ballRb.velocity.y;
        }

        var newAngle = new Vector3(x, y, ballRb.velocity.z).normalized;

        ballRb.velocity = ballRb.velocity.magnitude * newAngle;
    }

    private void ClampVelocity() {
        if (ballRb.velocity.magnitude < minShotSpeed) {
            ballRb.velocity = ballRb.velocity.normalized * minShotSpeed;
        }
        else if (ballRb.velocity.magnitude > maxShotSpeed) {
            ballRb.velocity = ballRb.velocity.normalized * maxShotSpeed;
        }
    }

    private void Freeze() {
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ballRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        ballRb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    void GetSettings() {
        var settings = Game.Instance.Settings;
        minShotSpeed = settings.minShotSpeed;
        maxShotSpeed = settings.maxShotSpeed;
        shotStrength = settings.shotStrength;
        curveMultiplier = settings.curveMultiplier;
    }


}


