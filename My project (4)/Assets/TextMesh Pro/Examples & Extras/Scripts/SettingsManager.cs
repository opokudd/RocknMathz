using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class SettingsMenu : MonoBehaviour
{
    public Button muteButton;        // Reference to the mute button
    public Button contactButton;     // Reference to the contact button
    public Text contactInfoText;     // Reference to the contact info Text UI
    public AudioSource musicSource;  // Reference to the AudioSource playing the music
    public Button goBack;

    private bool isMuted = false;

    void Start()
    {
        // Set initial states
        muteButton.onClick.AddListener(ToggleMute);  // Add listener for mute button
        contactButton.onClick.AddListener(DisplayContactInfo);  // Add listener for contact button
        contactInfoText.gameObject.SetActive(false);  // Hide contact info initially
        goBack.onClick.AddListener(OngoBackButtonClicked);
    }

    // Mutes and unmutes the music
    void ToggleMute()
    {
        isMuted = !isMuted;
        musicSource.mute = isMuted;
    }

    // Displays the contact info text for 30 seconds
    void DisplayContactInfo()
    {
        contactInfoText.text = "Contact Info: support@example.com"; // Example contact info
        contactInfoText.gameObject.SetActive(true);  // Show the contact info

        // Start a coroutine to hide the contact info after 30 seconds
        StartCoroutine(HideContactInfoAfterDelay());
    }
     private void OngoBackButtonClicked()
    {
        Debug.Log("Go Back button clicked!");
        SceneManager.LoadScene("MainMenuManager"); 
    }


    // Coroutine to hide the contact info after 30 seconds
    IEnumerator HideContactInfoAfterDelay()
    {
        yield return new WaitForSeconds(30f);
        contactInfoText.gameObject.SetActive(false);  // Hide the contact info
    }
}
