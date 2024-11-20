using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}



public class GameManager : MonoBehaviour
{
    //Declare currentState inside the class
    private GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        // Start in the main menu
        currentState = GameState.MainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                // Handle MainMenu logic, show main menu UI
                break;
            case GameState.Playing:
                // Handle gameplay logic (e.g., music sync, score updates)
                break;
            case GameState.Paused:
                // Handle Pause (e.g., show pause menu)
                break;
            case GameState.GameOver:
                // Handle Game Over (e.g., show game over UI)
                break;
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
    }

    public void PauseGame()
    {
        currentState = GameState.Paused;
    }

    public void EndGame()
    {
        currentState = GameState.GameOver;
    }
}

