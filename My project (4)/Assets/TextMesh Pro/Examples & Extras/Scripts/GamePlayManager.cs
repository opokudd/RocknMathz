using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public Text questionText;
    public InputField answerInputField;
    public Button submitButton;
    public Button exitButton;
    public Text scoreText;
    public Text timerText;
    public Text feedbackText;
    public Text levelText;
    public AudioClip[] musicTracks;
    public AudioSource musicSource;

    // Health bars
    public Slider playerHealthBar; // Player's health slider
    public Slider enemyHealthBar; // Enemy's health slider

    // Health values
    private float playerHealth = 100f; // Player's health (0 - 100)
    private float enemyHealth = 100f; // Enemy's health (0 - 100)

    private SaveLoadManager saveLoadManager;
    private PlayerData currentPlayerData;

    private int score = 0;
    private int timeRemaining = 60;
    private int correctAnswer;
    private bool isGameOver = false;
    private int currentLevel = 1;
    private int[] levelThresholds = { 200, 400, 600, 900, 1200 };

    void Start()
    {
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        if (saveLoadManager == null)
        {
            Debug.LogError("SaveLoadManager is not found in the scene.");
            return;
        }

        currentPlayerData = saveLoadManager.LoadPlayerData(MenuManager.currentUsername);
        feedbackText.text = "";
        score = 0;
        currentLevel = 1;
        UpdateScore();
        UpdateLevel();
        StartCoroutine(CountdownTimer());

        submitButton.onClick.AddListener(OnSubmitAnswer);
        exitButton.onClick.AddListener(OnExitClick);

        GenerateQuestion();
    }

    void Update()
    {
        if (isGameOver)
            return;

        timerText.text = "Time: " + timeRemaining.ToString();
    }

    IEnumerator CountdownTimer()
    {
        while (timeRemaining > 0 && !isGameOver)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        isGameOver = true;
        feedbackText.text = "Game Over! Final Score: " + score;

        if (currentPlayerData != null)
        {
            currentPlayerData.score = score;
            currentPlayerData.level = currentLevel;
            saveLoadManager.SavePlayerData(currentPlayerData);
        }
    }

    void GenerateQuestion()
    {
        int minTable = 1;
        int maxTable = 4;

        if (currentLevel >= 2) maxTable = 5;
        if (currentLevel >= 3) maxTable = 7;
        if (currentLevel >= 4) maxTable = 9;
        if (currentLevel >= 5) maxTable = 11;

        int number1 = Random.Range(minTable, maxTable + 1);
        int number2 = Random.Range(minTable, maxTable + 1);
        correctAnswer = number1 * number2;
        questionText.text = "What is " + number1 + " x " + number2 + "?";
    }

    void OnSubmitAnswer()
    {
        if (isGameOver)
            return;

        int playerAnswer;
        bool isValid = int.TryParse(answerInputField.text, out playerAnswer);

        if (isValid && playerAnswer == correctAnswer)
        {
            // Correct answer: decrease enemy's health
            float damage = 10f;
            enemyHealth -= damage;
            feedbackText.text = $"Correct! You dealt {damage} damage to the enemy.";

        }
        else
        {
            // Incorrect answer: decrease player's health
            float damage = 10f;
            playerHealth -= damage;
            feedbackText.text = $"Incorrect! You took {damage} damage.";

        }

        // Update health bars
        UpdateHealthBars();

        // Check for game over condition (if either health reaches 0)
        if (playerHealth <= 0)
        {
            isGameOver = true;
            feedbackText.text = "Game Over! You lost!";
        }
        else if (enemyHealth <= 0)
        {
            isGameOver = true;
            feedbackText.text = "You win! Enemy health reached 0!";
        }

        UpdateScore();
        CheckLevelUp();
        SavePlayerData();
        GenerateQuestion();
        answerInputField.text = "";
    }

    void UpdateHealthBars()
    {
        // Update the health bars
        playerHealthBar.value = playerHealth / 100f; // Normalize health to range [0, 1]
        enemyHealthBar.value = enemyHealth / 100f; // Normalize health to range [0, 1]
    }

    void OnExitClick()
    {
        SavePlayerData();
        SceneManager.LoadScene("MainMenuManager");
    }

    void SavePlayerData()
    {
        if (currentPlayerData != null)
        {
            currentPlayerData.score = score;
            currentPlayerData.level = currentLevel;
            saveLoadManager.SavePlayerData(currentPlayerData);
        }
    }

    void CheckLevelUp()
    {
        for (int i = 0; i < levelThresholds.Length; i++)
        {
            if (score >= levelThresholds[i] && currentLevel <= i + 1)
            {
                currentLevel = i + 2;
                UpdateLevel();
                SavePlayerData();
                break;
            }
        }
    }

    void UpdateLevel()
    {
        levelText.text = "Level: " + currentLevel;
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}