using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{
    public Button[] trackButtons; // Assign 5 buttons in the Inspector
    public Button backButton; // Button to return to the menu
    public Text currencyText; // Display user's current currency
    public AudioClip[] musicTracks; // Assign your music tracks here
    private int[] trackCosts = { 0, 200, 400, 600, 1000 }; // Costs of the tracks

    private SaveLoadManager saveLoadManager;
    private PlayerData currentPlayerData;

    private void Start()
    {
        backButton.onClick.AddListener(ReturnToMenu);

        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        if (saveLoadManager == null)
        {
            Debug.LogError("SaveLoadManager not found!");
            return;
        }

        currentPlayerData = saveLoadManager.LoadPlayerData(MenuManager.currentUsername);
        if (currentPlayerData == null)
        {
            Debug.LogError("Player data not found!");
            return;
        }

        UpdateCurrencyDisplay();
        UpdateTrackButtons();
    }

    public void BuyOrSelectTrack(int trackIndex)
{
    if (trackIndex < 0 || trackIndex >= currentPlayerData.musicOwned.Length)
    {
        Debug.LogError($"Invalid trackIndex: {trackIndex}. Valid range: 0 to {currentPlayerData.musicOwned.Length - 1}");
        return;
    }

    if (currentPlayerData.musicOwned[trackIndex])
    {
        // If already owned, play the track
        MusicManager.Instance.musicSource.clip = musicTracks[trackIndex];
        MusicManager.Instance.musicSource.Play();
    }
    else if (currentPlayerData.currency >= trackCosts[trackIndex])
    {
        // Buy the track
        currentPlayerData.currency -= trackCosts[trackIndex];
        currentPlayerData.musicOwned[trackIndex] = true;
        saveLoadManager.SavePlayerData(currentPlayerData);

        // Update UI and play the purchased track
        UpdateCurrencyDisplay();
        UpdateTrackButtons();
        MusicManager.Instance.musicSource.clip = musicTracks[trackIndex];
  }
}
    private void UpdateCurrencyDisplay()
    {
        currencyText.text = "Currency: " + currentPlayerData.currency.ToString();
    }

    private void UpdateTrackButtons()
{
    for (int i = 0; i < trackButtons.Length; i++)
    {
        int index = i; // Local copy for the lambda
        Text buttonText = trackButtons[i].GetComponentInChildren<Text>();
        if (buttonText == null)
        {
            Debug.LogError("No Text component found for button " + i);
            continue;
        }

        if (currentPlayerData.musicOwned[i])
        {
            buttonText.text = "Play";
            trackButtons[i].interactable = true;
        }
        else
        {
            buttonText.text = "Buy - " + trackCosts[i] + " Coins";
            trackButtons[i].interactable = currentPlayerData.currency >= trackCosts[i];
        }

        // Remove any previous listeners to avoid stacking
        trackButtons[i].onClick.RemoveAllListeners();
        trackButtons[i].onClick.AddListener(() => BuyOrSelectTrack(index));
    }
}


    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuManager");
    }
}

