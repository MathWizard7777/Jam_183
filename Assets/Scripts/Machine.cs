using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Machine : MonoBehaviour {
    public GameObject nodePrefab; // Prefab for the output node
    public float processTime; // Time taken to process the nodes

    protected int numberOfInputs; // Number of inputs for the machine
    protected int numberOfOutputs; // Number of outputs for the machine

    protected Vector3[] inputPositions; // Positions of the inputs
    protected Vector3[] outputPositions; // Positions of the outputs
    protected GameColor[] outputColors; // Colors of the outputs
    protected int[] outputValues; // Values of the outputs

    protected GameObject[] inputNodes; // Array to hold the input nodes
    protected Node[] inputNodeComponents; // Array to hold the components of the input nodes

    protected float detectionRadius = 0.5f; // Radius to detect nodes
    protected bool isProcessing = false; // Flag to track if the machine is currently processing

    protected virtual void Start() {
        InitializeInputsAndOutputs();
    }

    protected virtual void Update() {
        if (!isProcessing) {
            HandleMachineLogic();
        }
    }

    protected void InitializeInputsAndOutputs() {
        inputPositions = new Vector3[numberOfInputs];
        outputPositions = new Vector3[numberOfOutputs];
        outputColors = new GameColor[numberOfOutputs];
        outputValues = new int[numberOfOutputs];
        inputNodes = new GameObject[numberOfInputs];
        inputNodeComponents = new Node[numberOfInputs];
    }

    protected void HandleMachineLogic() {
        inputNodes = new GameObject[numberOfInputs];
        inputNodeComponents = new Node[numberOfInputs];

        for (int i = 0; i < numberOfInputs; i++) {
            inputNodes[i] = GetNodeAtPosition(inputPositions[i]);
            if (inputNodes[i] != null) {
                inputNodeComponents[i] = inputNodes[i].GetComponent<Node>();
            }
        }

        if (CheckIfProcessPossible(inputNodes)) {
            DetermineOutputs();
            StartCoroutine(ProcessNodes(inputNodes));
        }
    }

    protected virtual void DetermineOutputs() {
        // Logic to determine the output positions, colors, and values based on the machine type
    }

    protected virtual bool CheckIfProcessPossible(GameObject[] nodes) {
        bool canProcess = true;
        foreach (GameObject node in nodes) {
            if (node == null) {
                canProcess = false;
                break;
            }
        }

        if (canProcess) {
            GameColor commonColor = nodes[0].GetComponent<Node>().color;
            foreach (GameObject node in nodes) {
                if (node.GetComponent<Node>().color != commonColor) {
                    canProcess = false;
                    break;
                }
            }
        }

        canProcess = AdditionalProcessPossibleLogic(nodes, canProcess);
        
        foreach (Vector3 position in outputPositions) {
            if (GetNodeAtPosition(position) != null) {
                canProcess = false; // Output position is occupied
                break;
            }
        }
        return canProcess;
    }

    protected virtual bool AdditionalProcessPossibleLogic(GameObject[] nodes, bool canProcess) {
        return canProcess;
    }

    protected GameObject GetNodeAtPosition(Vector3 position) {
        Collider2D collider = Physics2D.OverlapCircle(position, detectionRadius, LayerMask.GetMask("Node"));
        return collider != null ? collider.gameObject : null;
    }

    protected virtual IEnumerator ProcessNodes(GameObject[] nodes) {
        isProcessing = true; // Set the processing flag to true

        // Destroy the input nodes
        foreach (GameObject node in nodes) {
            Destroy(node);
        }

        // Wait for the process time
        yield return new WaitForSeconds(processTime);

        // Create the output nodes
        for (int i = 0; i < numberOfOutputs; i++) {
            CreateOutputNode(outputValues[i], outputColors[i], i);
        }
        isProcessing = false; // Reset the processing flag
    }

    protected virtual void CreateOutputNode(int value, GameColor color, int index) {
        GameObject outputNode = Instantiate(nodePrefab, outputPositions[index], Quaternion.identity);
        Node nodeComponent = outputNode.GetComponent<Node>();
        nodeComponent.value = value; // Set the new value
        nodeComponent.color = color; // Set the new color
    }
}