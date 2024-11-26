using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public string username;
    public string password;
    public int score;
    public int level;
    public int currency;
    public bool[] musicOwned; // Tracks music ownership

    // Constructor for PlayerData
    public PlayerData(string username, string password)
    {
        this.username = username;
        this.password = password;
        this.score = 0;
        this.level = 1;
        this.currency = 0;
        this.musicOwned = new bool[5]; // Assuming 5 music tracks
    }
}


public class SaveLoadManager : MonoBehaviour
{
    
    private Dictionary<string, PlayerData> playerDatabase; // In-memory dictionary
    private string saveFilePath;

    private void Start()
        {
            saveFilePath = Application.persistentDataPath + "/playerData.json";
            Debug.Log("Save file path: " + saveFilePath); // Add logging to confirm the path
        }


    // Load player data from file by username

    // Assuming PlayerData is a class representing the player's information
public PlayerData LoadPlayerData(string username)
{
    if (File.Exists(saveFilePath))
    {
        try
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerDatabase database = JsonUtility.FromJson<PlayerDatabase>(json);

            // Debug: Check the contents of the player database
            Debug.Log("Database loaded, players count: " + database.players.Count);

            foreach (PlayerData data in database.players)
            {
                if (data.username == username)
                {
                    return data;  // Player found
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading player data: " + ex.Message);
        }
    }
    else
    {
        Debug.LogError("Save file not found at: " + saveFilePath);
    }

    return null;  // Return null if player data not found
}





    // Save player data to file
    public void SavePlayerData(PlayerData playerData)
{
    PlayerDatabase database = new PlayerDatabase();

    // Load existing data from the save file (if any)
    if (File.Exists(saveFilePath))
    {
        string json = File.ReadAllText(saveFilePath);
        database = JsonUtility.FromJson<PlayerDatabase>(json);
    }

    // Check if the player already exists in the database
    bool playerExists = false;
    for (int i = 0; i < database.players.Count; i++)
    {
        if (database.players[i].username == playerData.username)
        {
            database.players[i] = playerData; // Update existing player data
            playerExists = true;
            break;
        }
    }

    // If player doesn't exist, add new player
    if (!playerExists)
    {
        database.players.Add(playerData);
    }

    // Save the updated player database back to the file
    string jsonToSave = JsonUtility.ToJson(database, true);
    File.WriteAllText(saveFilePath, jsonToSave);
}



    // Load all player data from the file
    private void LoadAllPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerDatabase database = JsonUtility.FromJson<PlayerDatabase>(json);
            playerDatabase = new Dictionary<string, PlayerData>();

            // Convert List to Dictionary
            foreach (PlayerData data in database.players)
            {
                playerDatabase[data.username] = data;
            }
        }
        else
        {
            playerDatabase = new Dictionary<string, PlayerData>(); // Initialize empty dictionary if no file exists
        }
    }

    // Save all player data to the file
    private void SaveAllPlayerData()
    {
        PlayerDatabase database = new PlayerDatabase();
        database.players = new List<PlayerData>(playerDatabase.Values);

        string json = JsonUtility.ToJson(database, true);
        File.WriteAllText(saveFilePath, json);
    }
}

// Helper class to hold a collection of players
[System.Serializable]
public class PlayerDatabase
{
    public List<PlayerData> players; // Changed from Dictionary to List
}
