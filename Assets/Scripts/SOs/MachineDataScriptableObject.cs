using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class MachineDataScriptableObject : ScriptableObject {
    public int levelNumber;

    public float nodeSpeed = 2f;
    public float extractorSpeed = 0.5f;
    public float adderSpeed = 0.5f;
    public float subtractorSpeed = 0.5f;
    public float incrementerSpeed = 0.5f;
    public float decrementerSpeed = 0.5f;
    public float multiplierSpeed = 0.5f;
    public float multx2Speed = 0.5f;
    public float multx3Speed = 0.5f;
    public float multx5Speed = 0.5f;
    public float multx7Speed = 0.5f;
    public float splitterSpeed = 0.5f;
    public float sorterSpeed = 0.5f;
    public float decolorizerSpeed = 0.5f;

    public bool isAdderUnlocked;
    public bool isSubtractorUnlocked;
    public bool isIncrementerUnlocked;
    public bool isDecrementerUnlocked;
    public bool isMultiplierUnlocked;
    public bool isMultx2Unlocked;
    public bool isMultx3Unlocked;
    public bool isMultx5Unlocked;
    public bool isMultx7Unlocked;
    public bool isSplitterUnlocked;
    public bool isSorterUnlocked;
    public bool isDecolorizerUnlocked;
    public bool isJump1Unlocked;
    public bool isJump2Unlocked;
    public bool isJump3Unlocked;
}