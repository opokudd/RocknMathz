using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



    // Ensure everything is inside the class
    public class MusicManager : MonoBehaviour
    {
        // Declare variables and fields inside the class
        public int[] musicPrices = new int[5] { 100, 150, 200, 250, 300 }; // Prices for 5 tracks
        private PlayerData currentPlayer;
        private SaveLoadManager saveLoadManager;

        // Start method for initialization
        private void Start()
        {
            // Initialize saveLoadManager and player data
            saveLoadManager = FindObjectOfType<SaveLoadManager>();
            currentPlayer = saveLoadManager.LoadPlayerData("PlayerUsername"); // Replace with actual username
        }

        // Method to handle music purchase
        public void OnPurchaseMusic(int trackIndex)
        {
            // Check if the track index is valid
            if (trackIndex >= 0 && trackIndex < musicPrices.Length)
            {
                int trackPrice = musicPrices[trackIndex]; // Get the price for the selected track

                // Check if the player has enough currency
                if (currentPlayer.currency >= trackPrice)
                {
                    // Player has enough currency, proceed with purchase
                    currentPlayer.currency -= trackPrice;
                    currentPlayer.musicOwned[trackIndex] = true; // Mark the music as owned
                    saveLoadManager.SavePlayerData(currentPlayer); // Save the updated player data
                    Debug.Log("Music purchased successfully!");
                }
                else
                {
                    // Not enough currency
                    Debug.Log("Not enough currency to purchase this track.");
                }
            }
            else
            {
                // Invalid track index
                Debug.Log("Invalid track index.");
            }
        }
    }

