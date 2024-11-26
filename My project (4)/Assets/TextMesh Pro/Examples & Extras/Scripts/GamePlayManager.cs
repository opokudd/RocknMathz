using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{

    // Start is called before the first frame update
    public Text questionText; // The UI Text for displaying questions
    public InputField answerInputField; // The input field where the player types their answer
    public Button submitButton; // Button to submit the answer
    public Button exitButton; // Button to go to scoreshop
    public Text scoreText; // The UI Text for the score
    public Text timerText; // The UI Text for the timer
    public Text feedbackText; // The UI Text for feedback (correct or incorrect)
    public Text levelText; //The UI Text for displaying the player's level
    public AudioClip[] musicTracks; // Array to hold music tracks
    

   
    public AudioSource musicSource; // The AudioSource for background music
    private SaveLoadManager saveLoadManager;
    private PlayerData currentPlayerData;
    
    
    private int score = 0; // Player's score
    private int timeRemaining = 60; // Timer set to 60 seconds
    private float timer = 0f; // Timer for controlling the countdown
    private int correctAnswer; // Correct answer for the current question
    private bool isGameOver = false; // Flag to check if the game is over
    private int currentLevel = 1; // Player's current level (starts at 1)
    private int[] levelThresholds = {100, 500, 500, 700, 1000 }; //Score thresholds to unlock new levels

    void Start()
    {
       saveLoadManager = FindObjectOfType<SaveLoadManager>(); // Finds SaveLoadManager in the scene
    if (saveLoadManager == null)
    {
        Debug.LogError("SaveLoadManager is not found in the scene.");
        return; // Don't proceed if saveLoadManager is not assigned
    }

    currentPlayerData = saveLoadManager.LoadPlayerData("playerUsername");
        // Check if the player owns the first music track and play it if so
        if (currentPlayerData != null && currentPlayerData.musicOwned[0]) 
        {
            musicSource.clip = musicTracks[0]; // Set the first music track
            musicSource.Play(); // Play it
        }

        feedbackText.text = "";
        score = 0;
        currentLevel = 1; // Start from level 1
        UpdateScore();
        UpdateLevel();
        StartCoroutine(CountdownTimer());

        // Add listener for submit button
        submitButton.onClick.AddListener(OnSubmitAnswer);

        exitButton.onClick.AddListener(OnExitClick);

        // Generate the first question
        GenerateQuestion();

        // Set up the music purchase buttons if available
        
    }

    // Update is called once per frame
    void Update()
        {
            if (isGameOver)
                return;

            // Update the timer display every frame
            timerText.text = "Time: " + timeRemaining.ToString();

            
        }

    // Countdown timer that runs in the background
    IEnumerator CountdownTimer()
    {
        while (timeRemaining > 0 && !isGameOver)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        // End game when timer hits 0
        isGameOver = true;
        feedbackText.text = "Game Over! Final Score: " + score;
    }

    void GenerateQuestion()
    {
        // Determine the range of times tables to use based on the current level
        int minTable = 1;
        int maxTable = 4;

        // Increase the range based on the player's level
        if (currentLevel >= 2) maxTable = 5;
        if (currentLevel >= 3) maxTable = 7;
        if (currentLevel >= 4) maxTable = 9;
        if (currentLevel >= 5) maxTable = 11;

       

        // Randomly choose two numbers within the current times table range
        int number1 = Random.Range(minTable, maxTable + 1); 
        int number2 = Random.Range(minTable, maxTable + 1); 
        correctAnswer = number1 * number2; // Correct answer is the product of the two numbers
        questionText.text = "What is " + number1 + " x " + number2 + "?"; // Display the question
    }

    void OnSubmitAnswer()
    {
        if (isGameOver)
            return;

        int playerAnswer;
        bool isValid = int.TryParse(answerInputField.text, out playerAnswer); // Parse the input

        if (isValid && playerAnswer == correctAnswer)
        {
            // Correct answer
            score += (int)(timeRemaining * 1.5f);
            feedbackText.text = "Correct!"; // Show feedback
        }
        else
        {
            // Incorrect answer
            feedbackText.text = "Incorrect!"; // Show feedback
        }

        // Check if the player has reached the next level
        CheckLevelUp();

        // Update the score UI
        UpdateScore();

        // Generate a new question
        GenerateQuestion();

        // Clear the input field for the next answer
        answerInputField.text = "";
    }

    void OnExitClick()
    {
        SceneManager.LoadScene("ScoreShop");
    }

        void CheckLevelUp()
    {
        for (int i = 0; i < levelThresholds.Length; i++)
        {
            if (score >= levelThresholds[i] && currentLevel <= i + 1)
            {
                currentLevel = i + 2; // Set the level to the next one
                UpdateLevel();
                break;
            }
        }
    }

    // Update the level UI text
    void UpdateLevel()
    {
        levelText.text = "Level: " + currentLevel;
    }

    // Update the score UI
    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}

