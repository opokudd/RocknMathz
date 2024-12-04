using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private SaveLoadManager saveLoadManager; // Reference to SaveLoadManager
    private PlayerData currentPlayerData; // Player data

    public Text feedbackText; // Feedback text for errors
    public Button playButton;
    public Button instructionsButton;
    public Button settingsButton;
    public Button storeButton; // For store button
    public Button quitButton;

    string username = MenuManager.currentUsername;

    void Start()
    {
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        if (saveLoadManager == null)
        {
            Debug.LogError("SaveLoadManager not found in the scene!");
            return; // Exit if SaveLoadManager is not found
        }

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("No username found! Please check the login flow.");
            feedbackText.text = "No username found!";
            return;
        }

        // Load player data
        currentPlayerData = saveLoadManager.LoadPlayerData(username); // Ensure this matches the username input from the login screen

        if (currentPlayerData == null)
        {
            Debug.LogError("Player data not found. Please check save file.");
            feedbackText.text = "Error loading player data.";
            return;
        }

        playButton.onClick.AddListener(OnPlayButtonClicked);
        instructionsButton.onClick.AddListener(OnInstructionsButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        storeButton.onClick.AddListener(OnStoreButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked!");
        SceneManager.LoadScene("Progress");
    }

    private void OnInstructionsButtonClicked()
    {
        Debug.Log("Instructions button clicked!");
        SceneManager.LoadScene("InstructionsPage");
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Settings button clicked!");
        SceneManager.LoadScene("Settings");
    }

    private void OnStoreButtonClicked()
    {
        Debug.Log("Store button clicked!");
        SceneManager.LoadScene("Store");
    }

    private void OnQuitButtonClicked()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
