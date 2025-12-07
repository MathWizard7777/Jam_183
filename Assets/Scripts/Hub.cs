using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static GameColor;

public class Hub : MonoBehaviour
{
    public MachineDataScriptableObject gameData;
    public GameObject levelMenuManager;

    private int levelNumber;
    private int currentTargetNumber;
    private int currentTargetAmount;
    public int currentAmountSoFar;
    private GameColor currentTargetColor;

    public int[] targetNumbers;
    public GameColor[] targetColors;
    public int[] targetAmounts;
    public string[] unlockText;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI amountSoFarText;
    public TextMeshProUGUI amountNeededText;
    public TextMeshProUGUI unlockTextDisplay;
    public RawImage targetColor;
    
    private Dictionary<string, int> storage;
    private Utils utils = new Utils(); // Reference to the Utils class

    public bool canInput;

    void Start() {
        if (LevelDataHolder.selectedLevelData != null) {
            gameData = LevelDataHolder.selectedLevelData;
        }
        
        targetNumbers = new int[] {1, 2, 3, 5, 14, 33, 51, 62, 78, 84, 139, 157, 173, 191, 227, 351, 360, 420, 5040, 571};
        targetColors = new GameColor[] {None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None};
        targetAmounts = new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
        unlockText = new string[] {"Adder", "Splitter", "Decolorizer", "Multiplier", "Jump 1", "Jump 1", "Subtractor", "Jump 2", "Jump 2", "Jump 3", "Jump 3", "Jump 3", "Jump 3", "Sorter", "Sorter", "Decrementer", "Decrementer", "Incrementer", "Incrementer", "Nothing"};
        storage = new Dictionary<string, int>();

        levelNumber = gameData.levelNumber;
        currentTargetNumber = targetNumbers[levelNumber - 1];
        currentTargetColor = targetColors[levelNumber - 1];
        currentTargetAmount = targetAmounts[levelNumber - 1];
        currentAmountSoFar = 0;
        UpdateUI();
        canInput = false; // Allow input at the start
    }

    void Update() {
        CheckForNodes();
    }

    private void CheckForNodes() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(4f, 4f), 0f, LayerMask.GetMask("Node"));
        if (colliders.Length > 0) {
            foreach (var collider in colliders) {
                if (canInput) {
                    OnNode(collider.gameObject);
                } else {
                    Destroy(collider.gameObject); // Destroy the node if input is not allowed
                }
            }
        }
    }

    private void UnlockNextLevel() {
        levelNumber ++;
        if (levelNumber > targetNumbers.Length) {
            levelNumber = 1; // Reset to the first level if all levels are completed
        }

        currentTargetNumber = targetNumbers[levelNumber - 1];
        currentTargetColor = targetColors[levelNumber - 1];
        currentTargetAmount = targetAmounts[levelNumber - 1];
        currentAmountSoFar = 0;
        levelMenuManager.GetComponent<LevelMenuManager>().LevelComplete();
    }

    private void UpdateUI() {
        levelText.text = "Level: " + levelNumber;
        targetText.text = currentTargetNumber.ToString();
        amountSoFarText.text = currentAmountSoFar.ToString();
        amountNeededText.text = currentTargetAmount.ToString();
        targetColor.color = utils.colorMap[currentTargetColor];
        unlockTextDisplay.text = unlockText[levelNumber - 1];
    }

    public void OnNode(GameObject nodeObject) {
        Node node = nodeObject.GetComponent<Node>();
        string key = node.value.ToString() + (node.color == Red ? "r" : (node.color == Yellow ? "y" : (node.color == Green ? "g" : "b")));
        if (!storage.ContainsKey(key)) {
            storage[key] = 0;
        }
        storage[key] ++;
        if (node.color == currentTargetColor && node.value == currentTargetNumber) {
            currentAmountSoFar ++;
            if (currentAmountSoFar >= currentTargetAmount) {
                UnlockNextLevel();
            }
            UpdateUI();
        }
        Destroy(nodeObject);
    }
}
