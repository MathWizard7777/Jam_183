using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start_Menu : MonoBehaviour
{
    public Button levelSelectButton; // Reference to the button in the start menu
    public Button settingsButton; // Reference to the settings button
    public Button quitButton; // Reference to the quit button

    public void GameStart() {
        SceneManager.LoadScene("Level Select"); // Load the game scene
    }

    public void Settings() {
        SceneManager.LoadScene("Settings"); // Load the settings scene
    }

    public void Quit() {
        Application.Quit(); // Quit the application
    }
}
