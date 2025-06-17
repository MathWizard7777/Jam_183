using UnityEngine;

public class Jumper : MonoBehaviour
{
    public int n;

    void Update() {
        CheckForNode();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position - transform.rotation * Vector3.right * 0.5f, 0.2f);
    }

    private void CheckForNode() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.01f);
        if (colliders.Length > 0) {
            foreach (var collider in colliders) {
                Node node = collider.gameObject.GetComponent<Node>();
                if (node != null) {
                    node.target = transform.position + transform.rotation * Vector3.right * n;
                    node.isJumping = true;
                    node.direction = transform.rotation * Vector3.right;
                }
            }
        }
    }
}