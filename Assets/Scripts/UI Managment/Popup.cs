using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public TextMeshProUGUI usernameInput; // Reference to the input field for the username
    public Canvas menuCanvas; // Reference to the menu canvas
    public Canvas popupCanvas; // Reference to the popup canvas
    public Canvas tutorialCanvas;

    bool isTutorial = false; // Flag to check if tutorial is active

    void Start() {
        if (!PlayerPrefs.HasKey("Username")) {
            popupCanvas.gameObject.SetActive(true); // Show the popup if username is not set
            menuCanvas.gameObject.SetActive(false); // Hide the menu canvas
        } else {
            popupCanvas.gameObject.SetActive(false); // Hide the popup if username is set
            tutorialCanvas.gameObject.SetActive(true); // Hide the tutorial canvas
            menuCanvas.gameObject.SetActive(false); // Show the menu canvas
        }
    }

    public void SetUsername() {
        string username = usernameInput.text; // Get the username from the input field
        if (username.Length > 12) {
            username = username.Substring(0, 12); // Limit the username to 12 characters
        }
        PlayerPrefs.SetString("Username", username); // Save the username to PlayerPrefs
        PlayerPrefs.Save(); // Save PlayerPrefs to disk
        popupCanvas.gameObject.SetActive(false); // Hide the popup canvas
        menuCanvas.gameObject.SetActive(false); // Show the menu canvas
        tutorialCanvas.gameObject.SetActive(true); // Show the tutorial canvas
        usernameInput.text = ""; // Clear the input field
    }

    public void Cancel() {
        popupCanvas.gameObject.SetActive(false); // Hide the popup canvas
        tutorialCanvas.gameObject.SetActive(true); // Hide the tutorial canvas
        menuCanvas.gameObject.SetActive(false); // Show the menu canvas
    }

    public void CloseTutorial() {
        tutorialCanvas.gameObject.SetActive(false); // Hide the tutorial canvas
        menuCanvas.gameObject.SetActive(true); // Show the menu canvas
    }
}
