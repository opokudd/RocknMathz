using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    public Image[] levelCircles; // Drag your circle UI objects here in the Inspector
    public int currentLevel = 0; // Index of the current level (0-based)
    public int totalLevels = 5; // Total number of levels
    public Button goBack;
    public Button play;

    private MenuManager MenuManager;
    private SaveLoadManager saveLoadManager;
    string username = MenuManager.currentUsername;
    private void Awake()
    {
        // Find the SaveLoadManager in the scene
        saveLoadManager = FindObjectOfType<SaveLoadManager>();

        if (saveLoadManager == null)
        {
            Debug.LogError("SaveLoadManager not found in the scene!");
        }
    }


    private void Start()
        {
            goBack.onClick.AddListener(OngoBackButtonClicked);
            play.onClick.AddListener(OnplayButtonClicked);
            
            PlayerData playerData = saveLoadManager.LoadPlayerData(username);

            if (playerData != null)
            {
                currentLevel = playerData.level - 1; // Convert to 0-based index
            }
            else
            {
                Debug.LogWarning("Player data not found. Using default level.");
                currentLevel = 0; // Default level
            }

            UpdateProgressUI();
        }      

    

    public void UpdateProgressUI()
    {
        for (int i = 0; i < totalLevels; i++)
        {
            if (i < currentLevel)
            {
                levelCircles[i].color = Color.green; // Completed levels
            }
            else if (i == currentLevel)
            {
                levelCircles[i].color = Color.red; // Current level
            }
            else
            {
                levelCircles[i].color = Color.white; // Locked levels
            }
        }
    }

    private void OngoBackButtonClicked()
    {
        Debug.Log("Go Back button clicked!");
        SceneManager.LoadScene("MainMenuManager"); 
    }

    private void OnplayButtonClicked()
    {
        Debug.Log("Go Back button clicked!");
        SceneManager.LoadScene("GamePlayScene"); 
    }
}
