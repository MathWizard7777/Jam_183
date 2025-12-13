using UnityEngine;

public class GreaterThanN : MonoBehaviour
{
    public int n;

    void Update() {
        CheckForNode();
    }

    private void CheckForNode() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.01f);
        if (colliders.Length > 0) {
            foreach (var collider in colliders) {
                Node node = collider.gameObject.GetComponent<Node>();
                if (node != null) {
                    if (!node.isJumping) {
                        if (node.value > n) {
                            node.color = GameColor.Blue;
                        }
                    }
                }
            }
        }
    }
}