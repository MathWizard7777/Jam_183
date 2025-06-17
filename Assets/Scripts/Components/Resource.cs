using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resource : MonoBehaviour
{
    public int value; // Value of the resource
    public GameColor color; // Color of the resource

    public SpriteRenderer sr;
    public TextMeshProUGUI text; // Text component to display the value

    private Utils utils = new Utils(); // Reference to the Utils class

    void Start()
    {
        sr.color = utils.colorMap[color];
        text.text = value.ToString();
    }
}