using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Button backButton; // Reference to the back button in the settings menu

    public void Back()
    {
        SceneManager.LoadScene("Start Menu"); // Load the start menu scene
    }
}
