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
        this.musicOwned[0] = true; 
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

    
public PlayerData LoadPlayerData(string username)
{
    if (!File.Exists(Application.persistentDataPath + "/playerData.json"))
    {
        Debug.LogError("Save file not found at: " + saveFilePath);
        Debug.LogError("Save file  at: " + Application.persistentDataPath);

        // Optional: Create a default file to prevent errors
        CreateDefaultSaveFile();
        return null;
    }

    try
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/playerData.json");
        PlayerDatabase database = JsonUtility.FromJson<PlayerDatabase>(json);

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

    return null;  // Return null if player data not found
}

private void CreateDefaultSaveFile()
{
    PlayerDatabase defaultDatabase = new PlayerDatabase
    {
        players = new List<PlayerData>
        {
            new PlayerData("TestUser", "password123")
        }
    };

    string defaultJson = JsonUtility.ToJson(defaultDatabase, true);
    File.WriteAllText(Application.persistentDataPath + "/playerData.json", defaultJson);
    Debug.Log("Default save file created at: " + Application.persistentDataPath + "/playerData.json");
}



    // Save player data to file
    public void SavePlayerData(PlayerData playerData)
{
    PlayerDatabase database;

    // Load existing data or initialize a new database
    if (File.Exists(saveFilePath))
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/playerData.json");
        database = JsonUtility.FromJson<PlayerDatabase>(json);
    }
    else
    {
        database = new PlayerDatabase();
        database.players = new List<PlayerData>();
    }

    // Update or add player data
    bool playerExists = false;
    for (int i = 0; i < database.players.Count; i++)
    {
        if (database.players[i].username == playerData.username)
        {
            database.players[i] = playerData;
            playerExists = true;
            break;
        }
    }
    if (!playerExists)
    {
        database.players.Add(playerData);
    }

    // Save the updated database
    string jsonToSave = JsonUtility.ToJson(database, true);
    File.WriteAllText(Application.persistentDataPath + "/playerData.json", jsonToSave);
    Debug.Log("Player data saved successfully.");
}



    // Load all player data from the file
    private void LoadAllPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/playerData.json");
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
        File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
    }
}

// Helper class to hold a collection of players
[System.Serializable]
public class PlayerDatabase
{
    public List<PlayerData> players; // Changed from Dictionary to List
}
