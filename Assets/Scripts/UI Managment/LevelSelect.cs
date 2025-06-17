using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LevelSelect : MonoBehaviour
{
    public MachineDataScriptableObject[] levels; // Reference to the game data scriptable object

    public void Back()
    {
        SceneManager.LoadScene("Start Menu"); // Load the start menu scene
    }

    public void LevelN(int levelNumber)
    {
        LevelDataHolder.selectedLevelData = levels[levelNumber - 1]; // Set the selected level data
        SceneManager.LoadScene("Level 1"); // Load the selected level scene
    }
}
