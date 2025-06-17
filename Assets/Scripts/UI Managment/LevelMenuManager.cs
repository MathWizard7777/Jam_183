using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Dan.Main;

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

    private TextMeshProUGUI leaderboardText;

    public float timer;
    public float timeLimit = 45f; // Set your desired time limit here

    private bool isPaused = false;
    private bool isBuildMode = true; // Start in build mode

    private List<string> names = new List<string>();
    private List<int> scores = new List<int>();
    private int level;

    private List<string> publicKeys = new List<string>() {
        "1f304614e91e099c1ce67a77169abb1585cce06fb8a5aca4f5b4d92591da9096",
        "9fc618638b3eab5f7b53d59d8fce47c0084b38feeb773aa6e7053f2a615e8b79",
        "b3477288b03e7bc1edf100b7a81ee52767cc50c1f5e98dc76e05683898bf57a0",
        "f841df59a00974b57dd7b77cb1260f9ed90d44903c2af6324b2d925c5472f535",
        "f2ed12a01601cfd345a42d4c2bce91e2385f63d418778b4b715dc1a53a52d26e",
        "dbe554deefdcb76e94790a62c0af98281b106bae48525836bdf267a2e6b8b877",
        "22a34b0216ce2da51760bfadae89ecf3ae71de35a3b65eab5594cfa9bf8f4f67",
        "da3bf4f4d4f79a21818876bab522b773884754b87fb86eef6979fda383551045",
        "a85d70e99d29c32e4ab1e1fd9e77ce61e0cda6f74628ae47ddf936817474ca13",
        "a69ca23e4ddfba53270b07dc95b33f8e130eef8763363a8ed6608100f8e36a0a",
        "f40b9f09fc1c113cc6055e59d3c75c2063fd38ce137508881fe180784f6e739a",
        "0bd98982e176af796f9992836939dd90f161770b7184d368b931deb4181e6e2c",
        "acfc075971f7811646cd7614ef1cf18bcb6766f494dd4b4e7cd0095bf9286e55",
        "2358d66d46df1b7e0b8c5ead95f71b3938b913cb74608730b083f0fa8731ff53",
        "7ffe737e2a27dc2dba605d18b325c45a479726e5d9b50e02663fb2b277d078e5",
        "5fceb259da101ef54cb775b12d8c4047142b4e8371feff84dedb59b9797c29a8",
        "41d8a277d792eb09c7da32cfe7acf8072ac9951b03b7f6d833ad2acaf68c9716",
        "459ced77136f7a986e1c9abae59154b6b4b986496f5d3b66827608844d2aaf2f",
        "3b5aaab7f04b57ffe8a6d886b0aa14ee0b980b22c57fc40eecb32af009146c35",
        "c5e26bb4e4167ef9e72bd8d94536bb771d42690db4082cc53fc98a9f1ee9673a"

    };

    private void GetLeaderboard() {
        names.Clear(); // Clear the existing names list
        scores.Clear(); // Clear the existing scores list
        LeaderboardCreator.GetLeaderboard(publicKeys[level - 1], (msg) => {
            for (int i = 0; i < msg.Length; i++) {
                names.Add(msg[i].Username);
                scores.Add(msg[i].Score);
            }
            leaderboardText.text = "Loading...";
            StartCoroutine(DelayedUpdateLeaderboard()); // Start the coroutine to update the leaderboard after a delay
        });
    }

    private IEnumerator DelayedUpdateLeaderboard() {
        yield return new WaitForSecondsRealtime(1f); // Wait for 200 milliseconds
        UpdateLeaderboard(); // Update the leaderboard after the delay
    }

    private void UpdateLeaderboard() {
        if (leaderboardText != null) {
            leaderboardText.text = "Leaderboard\n";
            for (int i = 0; i < names.Count; i++) {
                leaderboardText.text += (names[i] + ":                                                                          ")[..32] + scores[i] + "\n";
            }
            Debug.Log("Leaderboard updated successfully." + names[0].ToString() + scores[0].ToString());
        } else {
            Debug.LogWarning("Leaderboard TextMeshProUGUI not found as a child of this GameObject.");
        }
    }

    private void SetLeaderboardEntry(string username, int score) {
        LeaderboardCreator.UploadNewEntry(publicKeys[level - 1], username, score, (msg) => {
            GetLeaderboard(); // Refresh the leaderboard after uploading a new entry
        });
    }

    void Start() {
        level = Hub.GetComponent<Hub>().gameData.levelNumber;
        leaderboardText = transform.Find("Level Complete")?.transform.Find("Leaderboard")?.GetComponent<TextMeshProUGUI>();
        if (leaderboardText != null) {
            leaderboardText.text = "Leaderboard";
        } else {
            Debug.LogWarning("Leaderboard TextMeshProUGUI not found as a child of this GameObject.");
        }
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
        Node[] nodes = FindObjectsOfType<Node>();
        foreach (Node node in nodes) {
            Destroy(node.gameObject);
        }
    }

    public void LevelComplete() {
        playModeCanvas.gameObject.SetActive(false);
        levelCompleteCanvas.gameObject.SetActive(true);
        tilesUsedText.text = "Tiles Used: " + GridSystem.GetComponent<GridSystem>().tilesUsed.ToString();
        Time.timeScale = 0; // Pause the game
        GridSystem.SetActive(false); // Disable the grid system when level is complete
        if (PlayerPrefs.HasKey("Username")) {
            string username = PlayerPrefs.GetString("Username");
            int score = GridSystem.GetComponent<GridSystem>().tilesUsed;
            SetLeaderboardEntry(username, score); // Set the leaderboard entry with the username and score
        } else {
            GetLeaderboard(); // Get the leaderboard for the current level
        }
    }

    public void ToggleBuildMode() {
        isBuildMode = !isBuildMode;
        if (isBuildMode) {
            Hub.GetComponent<Hub>().currentAmountSoFar = 0; // Allow input in build mode
        }
        DestroyAllNodes(); // Destroy all nodes when switching to build mode
        timer = isBuildMode ? 0 : timeLimit;
        buildModeCanvas.gameObject.SetActive(isBuildMode);
        playModeCanvas.gameObject.SetActive(!isBuildMode);
        GridSystem.SetActive(isBuildMode); // Enable the grid system in build mode
        Hub.GetComponent<Hub>().canInput = !isBuildMode; // Don't allow input in build mode
        Time.timeScale = isBuildMode ? 1 : 4; // Pause the game in build mode
    }

    public void TogglePauseMenu() {
        isPaused = !isPaused;
        pauseMenuCanvas.gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        GridSystem.SetActive(!isPaused); // Disable the grid system when paused
    }

    public void GoToMainMenu() {
        TogglePauseMenu(); // Close the pause menu if it's open
        SceneManager.LoadScene("Main Menu"); // Replace with your main menu scene name
    }

    public void RestartLevel() {
        Time.timeScale = 1; // Start the game
        GridSystem.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel() {
        Time.timeScale = 1; // Start the game
        GridSystem.SetActive(true);
        LevelDataHolder.selectedLevelData = levels[Hub.GetComponent<Hub>().gameData.levelNumber]; // Set the selected level data
        SceneManager.LoadScene("Level 1"); // Load the selected level scene
    }
}