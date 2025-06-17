using System.Collections;
using UnityEngine;

public class Splitter : Machine {
    protected bool isTopOutput = true;

    protected override void Start() {
        numberOfInputs = 1;
        numberOfOutputs = 2;
        base.Start();

        inputPositions[0] = transform.position;

        outputPositions[0] = transform.position + transform.rotation * Vector3.right;
        outputPositions[1] = transform.position + transform.rotation * Vector3.right - transform.rotation * Vector3.up;
    }

    protected override void DetermineOutputs() {
        outputColors[0] = inputNodeComponents[0].color;
        outputValues[0] = inputNodeComponents[0].value;
    }

    protected override void CreateOutputNode(int value, GameColor color, int index)
    {
        if (color != GameColor.None || value != 0) {
            GameObject outputNode;
            if (isTopOutput) {
                outputNode = Instantiate(nodePrefab, outputPositions[0], Quaternion.identity);
            } else {
                outputNode = Instantiate(nodePrefab, outputPositions[1], Quaternion.identity);
            }
            outputNode.GetComponent<Node>().value = value;
            outputNode.GetComponent<Node>().color = color;
            isTopOutput = !isTopOutput; // Toggle the output for the next node
        }
    }
}