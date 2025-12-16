using UnityEngine;

public class Sorter : Machine {
    protected override void Start() {
        numberOfInputs = 1;
        numberOfOutputs = 5;
        base.Start();

        inputPositions[0] = transform.position;

        outputPositions[0] = transform.position + transform.rotation * Vector3.right + transform.rotation * Vector3.up * 2;
        outputPositions[1] = transform.position + transform.rotation * Vector3.right + transform.rotation * Vector3.up;
        outputPositions[2] = transform.position + transform.rotation * Vector3.right;
        outputPositions[3] = transform.position + transform.rotation * Vector3.right - transform.rotation * Vector3.up;
        outputPositions[4] = transform.position + transform.rotation * Vector3.right - transform.rotation * Vector3.up * 2;
    }

    protected override void DetermineOutputs() {
        outputColors[0] = inputNodeComponents[0].color;
        outputValues[0] = inputNodeComponents[0].value;
        outputColors[1] = inputNodeComponents[0].color;
        outputValues[1] = inputNodeComponents[0].value;
        outputColors[2] = inputNodeComponents[0].color;
        outputValues[2] = inputNodeComponents[0].value;
        outputColors[3] = inputNodeComponents[0].color;
        outputValues[3] = inputNodeComponents[0].value;
        outputColors[4] = inputNodeComponents[0].color;
        outputValues[4] = inputNodeComponents[0].value;
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
            outputNode = Instantiate(nodePrefab, outputPositions[3], Quaternion.identity);
            outputNode.GetComponent<Node>().value = value;
            outputNode.GetComponent<Node>().color = color;
        } else if (color == GameColor.Blue) {
            outputNode = Instantiate(nodePrefab, outputPositions[4], Quaternion.identity);
            outputNode.GetComponent<Node>().value = value;
            outputNode.GetComponent<Node>().color = color;
        } else if (color == GameColor.None) {
            outputNode = Instantiate(nodePrefab, outputPositions[2], Quaternion.identity);
            outputNode.GetComponent<Node>().value = value;
            outputNode.GetComponent<Node>().color = color;
        }
    }
}