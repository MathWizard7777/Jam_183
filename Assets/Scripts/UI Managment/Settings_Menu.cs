using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Settings_Menu : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private Button backButton;

    void Start()
    {
        // Get the root visual element of the UIDocument
        VisualElement root = document.rootVisualElement;

        // Find the buttons in the UI hierarchy
        backButton = root.Q<Button>("Back");

        // Register button click events
        backButton.RegisterCallback<ClickEvent>(OnBackButtonClick);  
    }

    private void OnBackButtonClick(ClickEvent evt)
    {
        // Load the level select scene
        SceneManager.LoadScene("Main Menu");
    }
}