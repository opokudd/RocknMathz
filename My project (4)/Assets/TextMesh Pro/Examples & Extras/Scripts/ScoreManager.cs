using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int combo = 0;
    public int maxCombo = 0;

    public void AddScore(int points)
    {
        score += points;
        combo++;
        if (combo > maxCombo)
        {
            maxCombo = combo;
        }
        Debug.Log("Score: " + score + " Combo: " + combo);
    }

    public void ResetCombo()
    {
        combo = 0;
    }
}