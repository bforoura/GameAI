using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement; // Required for Scene management
using UnityEngine.UI; // Required for UI components like Button


public class GameController : MonoBehaviour
{
    // Reference to the StartButton and StopButton in the Inspector
    public Button StartButton; // Reference to the Start button
    public Button StopButton;  // Reference to the Stop button

    private bool isGameRunning = true; // Flag to check whether the game is running or paused

    void Start()
    {
        // Check if both buttons are assigned in the Inspector
        if (StartButton != null && StopButton != null)
        {
            // Add listeners for the button click events
            StartButton.onClick.AddListener(StartGame); // When StartButton is clicked, call StartGame()
            StopButton.onClick.AddListener(StopGame);   // When StopButton is clicked, call StopGame()

            // Initially hide the Stop button and show the Start button
            //StopButton.gameObject.SetActive(false); // Hide the Stop button at the start of the game
        }
    }

    // Start the game (unpause)
    public void StartGame()
    {
        if (!isGameRunning)
        {
            isGameRunning = true;
            Time.timeScale = 1; // Unpause the game
            Debug.Log("Game Started!");

            // Hide the Start button and show the Stop button
            //StartButton.gameObject.SetActive(false); // Hide the Start button
           // StopButton.gameObject.SetActive(true);   // Show the Stop button
        }
    }

    // Stop the game (pause)
    public void StopGame()
    {
        if (isGameRunning)
        {
            isGameRunning = false;
            Time.timeScale = 0; // Pause the game
            Debug.Log("Game Stopped!");

            // Show the Start button and hide the Stop button
            //StartButton.gameObject.SetActive(true);  // Show the Start button
           // StopButton.gameObject.SetActive(false); // Hide the Stop button
        }
    }
}
