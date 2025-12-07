using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelMenuManager : MonoBehaviour
{
    public MachineDataScriptableObject[] levels; // Reference to the game data scriptable object

    public Canvas levelCompleteCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas buildModeCanvas;
    public Canvas playModeCanvas;
    public TextMeshProUGUI tilesUsedText;
    public TextMeshProUGUI timerText;
    public GameObject GridSystem;
    public GameObject Hub;

    public float timer;
    public float timeLimit = 30f;

    private bool isPaused = false;
    private bool isBuildMode = true;

    private int level;

    private void Start() {
        level = Hub.GetComponent<Hub>().gameData.levelNumber;
    }

    void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            TogglePauseMenu();
        }
        if (!isBuildMode) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                ToggleBuildMode();
            }
            timerText.text = "Time Remaining: " + Mathf.Round(timer).ToString() + "s";
        }
    }

    private void DestroyAllNodes() {
        Node[] nodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        foreach (Node node in nodes) {
            Destroy(node.gameObject);
        }
    }

    public void LevelComplete() {
        playModeCanvas.gameObject.SetActive(false);
        levelCompleteCanvas.gameObject.SetActive(true);
        tilesUsedText.text = "Tiles Used: " + GridSystem.GetComponent<GridSystem>().tilesUsed.ToString();
        Time.timeScale = 0;
        GridSystem.SetActive(false);
    }

    public void ToggleBuildMode() {
        isBuildMode = !isBuildMode;
        if (isBuildMode) {
            Hub.GetComponent<Hub>().currentAmountSoFar = 0;
        }
        DestroyAllNodes();
        timer = isBuildMode ? 0 : timeLimit;
        buildModeCanvas.gameObject.SetActive(isBuildMode);
        playModeCanvas.gameObject.SetActive(!isBuildMode);
        GridSystem.SetActive(isBuildMode);
        Hub.GetComponent<Hub>().canInput = !isBuildMode;
        Time.timeScale = isBuildMode ? 1 : 4;
    }

    public void TogglePauseMenu() {
        isPaused = !isPaused;
        pauseMenuCanvas.gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : isBuildMode ? 1 : 4;;
        GridSystem.SetActive(!isPaused);
    }

    public void GoToMainMenu() {
        TogglePauseMenu();
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartLevel() {
        Time.timeScale = 1;
        GridSystem.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel() {
        Time.timeScale = 1;
        GridSystem.SetActive(true);
        LevelDataHolder.selectedLevelData = levels[Hub.GetComponent<Hub>().gameData.levelNumber];
        SceneManager.LoadScene("Level");
    }
}