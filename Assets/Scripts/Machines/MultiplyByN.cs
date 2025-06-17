using UnityEngine;

public class MultiplyByN : Machine {
    public int multiplier; // The multiplier value

    protected override void Start() {
        numberOfInputs = 1;
        numberOfOutputs = 1;
        base.Start();

        inputPositions[0] = transform.position;

        outputPositions[0] = transform.position + transform.rotation * Vector3.right;
    }

    protected override void DetermineOutputs() {
        outputColors[0] = inputNodeComponents[0].color;
        outputValues[0] = inputNodeComponents[0].value * multiplier;
    }
}