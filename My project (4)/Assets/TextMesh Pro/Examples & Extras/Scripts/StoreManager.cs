using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{
    public Button[] trackButtons; // Assign 5 buttons in the Inspector
    public Button back;
    public Text[] trackPrices; // Text showing track prices
    public Text currencyText; // Display user's current currency
    public AudioClip[] musicTracks; // Assign your music tracks here
    public AudioSource musicSource; // To play the selected track
    private int[] trackCosts = { 0, 50, 100, 150, 200 }; // Costs of the tracks

    private SaveLoadManager saveLoadManager;
    private PlayerData currentPlayerData;

    private void Start()
    {
        back.onClick.AddListener(ReturnToMenu);
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        if (saveLoadManager == null)
        {
            Debug.LogError("SaveLoadManager not found in the scene!");
            return;
        }

        currentPlayerData = saveLoadManager.LoadPlayerData(MenuManager.currentUsername);
        if (currentPlayerData == null)
        {
            Debug.LogError("Player data not found!");
            return;
        }

        // Update UI
        UpdateCurrencyDisplay();
        UpdateTrackButtons();
    }

    public void BuyOrSelectTrack(int trackIndex)
    {
        if (currentPlayerData.musicOwned[trackIndex])
        {
            // Already owned, select the track
            PlayTrack(trackIndex);
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
            PlayTrack(trackIndex);
        }
        else
        {
            Debug.Log("Not enough currency to buy this track!");
        }
    }

    private void PlayTrack(int trackIndex)
{
    MusicManager.Instance.musicSource.clip = musicTracks[trackIndex];
    MusicManager.Instance.musicSource.Play();
}


    private void UpdateCurrencyDisplay()
    {
        currencyText.text = "Currency: " + currentPlayerData.currency.ToString();
    }

    private void UpdateTrackButtons()
{
    for (int i = 0; i < trackButtons.Length; i++)
    {
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

        Debug.Log("Button " + i + " updated with text: " + buttonText.text);
    }
}


    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuManager");
    }
}
