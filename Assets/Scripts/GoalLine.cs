using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GoalLine : MonoBehaviour {

    [SerializeField] bool vertical = false;
    LineRenderer line;

    public void DrawLine(Vector3 impact) {
        var extents = vertical ? transform.parent.localScale.z / 2 : transform.parent.localScale.x / 2;

        var wallZ = transform.parent.position.z - transform.parent.localScale.y / 2 - 0.001f;

        Vector3[] verts;

        if (vertical) {
            verts = new Vector3[2] {
                new Vector3(impact.x, transform.parent.position.y - extents, wallZ),
                new Vector3(impact.x, transform.parent.position.y + extents, wallZ)
            };
        }
        else {
            verts = new Vector3[2] {
                new Vector3 (transform.parent.position.x - extents, impact.y, wallZ),
                new Vector3 (transform.parent.position.x + extents, impact.y, wallZ)
            };
        }

        line.SetPositions(verts);

    }

    private void Awake() {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }
}
