using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public InputField usernameInput;   // For username input
    public InputField passwordInput;   // For password input
    public Button loginButton;         // For login button
    public Button registerButton;      // For registration button
    public Text feedbackText;          // For displaying feedback to the user

    private SaveLoadManager saveLoadManager;   // Reference to the SaveLoadManager script
    public static string currentUsername;

    private void Start()
    {
        saveLoadManager = FindObjectOfType<SaveLoadManager>(); // Get SaveLoadManager in the scene

        // Adding button listeners
        loginButton.onClick.AddListener(OnLoginClick);
        registerButton.onClick.AddListener(OnRegisterClick);
    }

    // Method to handle user login
    private void OnLoginClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // Check if user exists in saved data
        PlayerData playerData = saveLoadManager.LoadPlayerData(username);

        if (playerData != null && playerData.password == password)
        {
            feedbackText.text = "Login Successful!";
            currentUsername = username;
            // After successful login, load the gameplay scene
            LoadGameplayScene();
        }
        else
        {
            feedbackText.text = "Invalid username or password.";
        }
    }

    // Method to handle user registration
    private void OnRegisterClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // Check if the username already exists
        PlayerData existingPlayer = saveLoadManager.LoadPlayerData(username);

        if (existingPlayer != null)
        {
            feedbackText.text = "Username already exists. Try another one.";
        }
        else
        {
            // Create new player data and save it
            PlayerData newPlayer = new PlayerData(username, password);
            saveLoadManager.SavePlayerData(newPlayer);
            feedbackText.text = "Registration Successful!";
        }
    }

    // Method to load the mainMenu scene
    private void LoadGameplayScene()
    {
        SceneManager.LoadScene("MainMenuManager"); 
    }
}
