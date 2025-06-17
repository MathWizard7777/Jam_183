using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor : MonoBehaviour {
    public float extractionSpeed; // Speed of the extraction process
    public GameObject nodePrefab; // Prefab for the node to be created

    private GameObject resource; // Reference to the resource being extracted
    private GameColor color; // Color of the resource being extracted
    private int value; // Value of the resource being extracted

    private float timer = 0;

    private Utils utils = new Utils(); // Reference to the Utils class

    void Start() {
        InitializeResource();
    }

    void Update() {
        HandleExtraction();
    }

    private void InitializeResource() {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Resource")); // Check for resources in the vicinity
        if (collider != null) {
            resource = collider.gameObject; // Get the resource object
            color = resource.GetComponent<Resource>().color; // Get the color of the resource
            value = resource.GetComponent<Resource>().value; // Get the value of the resource
        }
    }

    private void HandleExtraction() {
        if (CanExtractNode()) {
            ExtractNode();
        } else {
            IncrementTimer();
        }
    }

    private bool CanExtractNode() {
        return timer > extractionSpeed && !IsNodeBlocking();
    }

    private bool IsNodeBlocking() {
        return Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Node")) != null;
    }

    private void ExtractNode() {
        timer = 0; // Reset the timer
        GameObject node = Instantiate(nodePrefab, transform.position, Quaternion.identity); // Create a new node
        InitializeNode(node);
    }

    private void InitializeNode(GameObject node) {
        node.GetComponent<Node>().color = color; // Set the color of the node
        node.GetComponent<Node>().value = value; // Set the value of the node
    }

    private void IncrementTimer() {
        timer += Time.deltaTime; // Increment the timer
    }
}