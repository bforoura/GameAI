using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Include this for UI components
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
   
    public TMP_Text scoreText; // Reference to the UI Text component

    void Start()
    {
        UpdateScore();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
