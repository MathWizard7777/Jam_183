using System.Runtime.InteropServices;
using UnityEngine;

public class Addition : Machine {
    protected override void Start() {
        numberOfInputs = 2;
        numberOfOutputs = 1;
        base.Start();

        inputPositions[0] = transform.position;
        inputPositions[1] = transform.position - transform.rotation * Vector3.up;

        outputPositions[0] = transform.position + transform.rotation * Vector3.right;
    }

    protected override void DetermineOutputs() {
        if (inputNodeComponents[0].color == GameColor.None) {
            outputColors[0] = inputNodeComponents[1].color;
        } else {
            outputColors[0] = inputNodeComponents[0].color;
        }
        outputValues[0] = inputNodeComponents[0].value + inputNodeComponents[1].value;
    }

    protected override bool AdditionalProcessPossibleLogic(GameObject[] nodes, bool canProcess)
    {
        bool output = false;
        if (nodes[0] != null && nodes[1] != null) {
            if (nodes[0].GetComponent<Node>().color == GameColor.None || nodes[1].GetComponent<Node>().color == GameColor.None) {
                output = true;
            }
        }
        return output || canProcess; // Return the original canProcess value if no additional logic is needed
    }
}