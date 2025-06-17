using System.Collections.Generic;
using UnityEngine;

public enum GameColor
{
    None,
    Red,
    Green,
    Blue,
    Yellow
}

public class Utils
{
    public Dictionary<GameColor, Color> colorMap = new Dictionary<GameColor, Color>()
    {
        { GameColor.None, new Color(1, 1, 1) },
        { GameColor.Red, new Color(0.88f, 0.12f, 0.12f) }, // #e01f1f
        { GameColor.Green, new Color(0.11f, 0.92f, 0.02f) }, // #1ceb05
        { GameColor.Blue, new Color(0.02f, 0.17f, 0.75f)}, // #062bbf
        { GameColor.Yellow, new Color(1f, 0.94f, 0.23f) } // #ffef3a
    };
}
