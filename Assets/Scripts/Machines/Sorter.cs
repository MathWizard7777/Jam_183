using UnityEngine;

public class Sorter : Machine {
    protected override void Start() {
        numberOfInputs = 1;
        numberOfOutputs = 4;
        base.Start();

        inputPositions[0] = transform.position;

        outputPositions[0] = transform.position + transform.rotation * Vector3.right;
        outputPositions[1] = transform.position + transform.rotation * Vector3.right - transform.rotation * Vector3.up;
        outputPositions[2] = transform.position + transform.rotation * Vector3.right - transform.rotation * Vector3.up * 2;
        outputPositions[3] = transform.position + transform.rotation * Vector3.right - transform.rotation * Vector3.up * 3;
    }

    protected override void DetermineOutputs() {
        outputColors[0] = inputNodeComponents[0].color;
        outputValues[0] = inputNodeComponents[0].value;
    }

    protected override void CreateOutputNode(int value, GameColor color, int index)
    {
        GameObject outputNode;

        if (color == GameColor.Red) {
            outputNode = Instantiate(nodePrefab, outputPositions[0], Quaternion.identity);
            outputNode.GetComponent<Node>().value = value;
            outputNode.GetComponent<Node>().color = color;
        } else if (color == GameColor.Yellow) {
            outputNode = Instantiate(nodePrefab, outputPositions[1], Quaternion.identity);
            outputNode.GetComponent<Node>().value = value;
            outputNode.GetComponent<Node>().color = color;
        } else if (color == GameColor.Green) {
            outputNode = Instantiate(nodePrefab, outputPositions[2], Quaternion.identity);
            outputNode.GetComponent<Node>().value = value;
            outputNode.GetComponent<Node>().color = color;
        } else if (color == GameColor.Blue) {
            outputNode = Instantiate(nodePrefab, outputPositions[3], Quaternion.identity);
            outputNode.GetComponent<Node>().value = value;
            outputNode.GetComponent<Node>().color = color;
        }
    }
}