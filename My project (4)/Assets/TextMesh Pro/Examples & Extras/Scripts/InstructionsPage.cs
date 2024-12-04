using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For scene management

public class InstructionsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button backButton;

    void Start()
    {
        // Add listener to the Back button
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        Debug.Log("Back to Menu button clicked!");
        SceneManager.LoadScene("MainMenuManager"); // Replace "MainMenuScene" with your main menu scene name
    }
}