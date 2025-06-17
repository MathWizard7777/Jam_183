using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private Button settingsButton;

    void Start()
    {
        // Get the root visual element of the UIDocument
        VisualElement root = document.rootVisualElement;

        // Find the buttons in the UI hierarchy
        levelSelectButton = root.Q<Button>("LevelSelect");
        settingsButton = root.Q<Button>("Settings");

        // Register button click events
        levelSelectButton.RegisterCallback<ClickEvent>(OnLevelSelectButtonClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClick);   
    }

    private void OnLevelSelectButtonClick(ClickEvent evt)
    {
        // Load the level select scene
        SceneManager.LoadScene("Level Select");
    }

    private void OnSettingsButtonClick(ClickEvent evt)
    {
        // Load the settings scene
        SceneManager.LoadScene("Settings");
    }
}