using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class Node : MonoBehaviour {
    public TextMeshProUGUI text;
    public SpriteRenderer sr;

    public float speed; // Speed of the resource
    public float moveSpeed;

    public Vector2 target; // Target position for the resource to move towards
    public Vector2 direction; // Direction of the resource's movement

    public int value; // Value of the resource
    public GameColor color; // Color of the resource

    private Utils utils = new Utils(); // Reference to the Utils class
    public bool isJumping = false;

    void Start() {
        InitializeNode();
    }

    void OnDrawGizmos() {
        DrawDirectionGizmo();
        Gizmos.color = Color.red;
        Gizmos.DrawSphere((Vector3)target, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + (Vector3)direction * 0.5f, 0.45f);
    }

    void Update() {
        UpdateNodeColorAndText();
        List<GameObject> touchingObjects = GetTouchingObjects();

        if (touchingObjects.Count > 0 || isJumping) {
            if (!isJumping) {
                UpdateDirectionBasedOnConveyor(touchingObjects[0]);
                HandleMovement(touchingObjects[0]);
            } else {
                HandleJumping(); // Handle jumping state
            }
        } else {
            CheckAndDestroyNode(); // Check if the node should destroy itself
        }
        CheckTouchingNodes(); // Check if the node is touching other nodes
    }

    private void CheckAndDestroyNode() {
        // Check if the node is not on a GameObject with a specific tag
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);
        bool isOnValidObject = false;

        foreach (var collider in colliders) {
            if (collider.CompareTag("Component")) {
                isOnValidObject = true;
                break;
            }
        }

        if (!isOnValidObject) {
            Destroy(gameObject); // Destroy the node if no valid object is detected
        }
    }

    private void InitializeNode() {
        target = transform.position; // Initialize target to the current position
        direction = Vector2.zero; // Initialize direction to zero
        sr.color = utils.colorMap[color]; // Set the color of the sprite renderer based on the resource's color
        text.text = value.ToString(); // Update the text to show the initial value
        moveSpeed = speed; // Set the move speed to the specified speed
    }

    private void DrawDirectionGizmo() {
        Gizmos.DrawSphere(transform.position + (Vector3)direction * 0.4f, 0.2f);
    }

    private void UpdateNodeColorAndText() {
        sr.color = utils.colorMap[color]; // Set the color of the sprite renderer based on the resource's color
        text.text = value.ToString(); // Update the text to show the current value
    }

    private List<GameObject> GetTouchingObjects() {
        List<GameObject> touchingObjects = new List<GameObject>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, LayerMask.GetMask("Conveyor"));
        foreach (var collider in colliders) {
            touchingObjects.Add(collider.gameObject);
        }
        return touchingObjects;
    }

    private void CheckTouchingNodes() {
        List<GameObject> touchingObjects = new List<GameObject>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, LayerMask.GetMask("Node"));
        if (colliders.Length > 1) {
            Destroy(gameObject);
        }
    }

    private void UpdateDirectionBasedOnConveyor(GameObject conveyor) {
        float rotationZ = conveyor.transform.rotation.eulerAngles.z;

        if (rotationZ == 0) {
            direction = Vector2.right; // Move right if the conveyor is horizontal
        } else if (rotationZ == 90) {
            direction = Vector2.up; // Move up if the conveyor is vertical
        } else if (rotationZ == 270) {
            direction = Vector2.down; // Move down if the conveyor is vertical in the opposite direction
        } else if (rotationZ == 180) {
            direction = Vector2.left; // Move left
        }
    }

    private void HandleJumping() {
        gameObject.layer = LayerMask.NameToLayer("Default"); // Set the layer to "Jumping" if the node is jumping
        moveSpeed = speed * 2; // Increase speed if the node is jumping
        MoveTowardsTarget();
        if (IsTargetReached()) {
            isJumping = false; // Reset jumping state if the target is reached
            gameObject.layer = LayerMask.NameToLayer("Node");
        }
    }

    private void HandleMovement(GameObject conveyor) {
        if (IsNodeInPath()) {
            moveSpeed = 0; // Stop moving if a node is detected in the direction of movement
        } else {
            moveSpeed = speed; // Reset move speed if no node is detected
        }

        if (IsTargetReached()) {
            SetNextTarget(conveyor);
        } else {
            MoveTowardsTarget();
        }
    }

    private bool IsNodeInPath() {
        return Physics2D.OverlapCircleAll(transform.position + (Vector3)direction * 0.5f, 0.45f, LayerMask.GetMask("Node")).Length > 1;
    }

    private bool IsTargetReached() {
        return (transform.position - (Vector3)target).magnitude <= 0.001f;
    }

    private void SetNextTarget(GameObject conveyor) {
        target = conveyor.transform.position + (Vector3)direction; // Set target to the next position on the conveyor
    }

    private void MoveTowardsTarget() {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime); // Move towards the target
    }
}