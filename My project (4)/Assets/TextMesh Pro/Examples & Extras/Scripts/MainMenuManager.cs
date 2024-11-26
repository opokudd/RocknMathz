using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Text scoreText; // UI to show player's score
    public Text currencyText; // UI to show player's currency
    public Text feedbackText; // UI to show feedback to the user
    public Button[] musicButtons; // Buttons for displaying music options
    public int[] musicPrices; // Prices for music tracks
    public AudioClip[] musicTracks; // List of available music tracks
    public bool[] musicOwned; // Whether the player owns a particular music track
    public int currency = 0; // Player's current currency
    private MusicManager musicManager; // Reference to MusicManager
    private SaveLoadManager saveLoadManager; // Reference to SaveLoadManager
    private PlayerData currentPlayerData; // Player data
    private int selectedMusicIndex = -1; // The index of the selected music


    string username = MenuManager.currentUsername;
   void Start()
{
    saveLoadManager = FindObjectOfType<SaveLoadManager>();
    if (saveLoadManager == null)
    {
        Debug.LogError("SaveLoadManager not found in the scene!");
        return; // Exit if SaveLoadManager is not found
    }

    musicManager = FindObjectOfType<MusicManager>();
    

    if (string.IsNullOrEmpty(username))
    {
        Debug.LogError("No username found! Please check the login flow.");
        feedbackText.text = "No username found!";
        return;
    }

    // Load player data
    currentPlayerData = saveLoadManager.LoadPlayerData(username); // Ensure this matches the username input from the login screen

    // Check if data is loaded properly
    if (currentPlayerData == null)
    {
        Debug.LogError("Player data not found for username 'test'. Please check save file.");
        feedbackText.text = "Error loading player data.";
        return; // Exit Start if no data is loaded
    }

    // Update the UI with the current score and currency
    scoreText.text = "Score: " + currentPlayerData.score;
    currency = currentPlayerData.currency; // Use currency as separate from score
    currencyText.text = "Currency: " + currency.ToString();

    // Initialize music buttons and set their text and listeners
    for (int i = 0; i < musicButtons.Length; i++)
    {
        int index = i; // Capture the correct index for each button
        musicButtons[i].onClick.AddListener(() => SelectMusic(index)); // On button click, select the corresponding music track

        // Set button text to the name of the music track (for example, "Track 1")
        musicButtons[i].GetComponentInChildren<Text>().text = musicTracks[i].name;

        // Disable button if music is already owned
        if (musicOwned[i])
        {
            musicButtons[i].interactable = false; // Disable the button if already owned
        }
    }
}


void SelectMusic(int index)
{
    selectedMusicIndex = index; // Store the selected music track index
    feedbackText.text = "Selected music: " + musicTracks[index].name; // Show selected track's name
}


public void OnPurchaseMusic()
{
    if (selectedMusicIndex != -1) // Ensure that a music track has been selected
    {
        musicManager.OnPurchaseMusic(selectedMusicIndex); // Pass selected index to purchase music

        // Update UI after purchase attempt
        currentPlayerData = saveLoadManager.LoadPlayerData(username); // Reload updated player data
        currency = currentPlayerData.currency; // Update currency based on current player's currency
        currencyText.text = "Currency: " + currency.ToString();

        // Provide feedback
        if (musicOwned[selectedMusicIndex])
        {
            feedbackText.text = "Music purchased successfully!";
            musicButtons[selectedMusicIndex].interactable = false; // Disable button if music is owned
        }
        else
        {
            feedbackText.text = "Not enough currency or already owned!";
        }
    }
    else
    {
        feedbackText.text = "Please select a music track first!";
    }
}

}
