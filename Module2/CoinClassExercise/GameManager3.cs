using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*

public class GameManager : MonoBehaviour
{
    private int score = 0;

    // Method to subscribe to coin events
    public void Start()
    {
        // Find all coins in the scene and subscribe to their OnCoinCollected event
        Coin[] coins = FindObjectsOfType<Coin>(); // Find all Coin objects in the scene
        foreach (Coin coin in coins)
        {
            // Subscribe to the event: When the coin is collected, it calls the HandleCoinCollected method.
            coin.OnCoinCollected += HandleCoinCollected;  // Subscribe to the event
        }
    }

    // Event handler for coin collection. This method gets called when a coin is collected.
    private void HandleCoinCollected(Coin coin)
    {
        // Increment the score when a coin is collected
        score++;
        Debug.Log("Coin collected! Score: " + score);

        // Check if the score has reached 10, and if so, end the game
        if (score == 10)
        {
            EndGame();
        }

        // Destroy the coin that was collected
        Destroy(coin.gameObject);
    }

    // Method to end the game when the score reaches 10
    private void EndGame()
    {
        Debug.Log("Game Over! You reached 10 points!");
        Time.timeScale = 0;  // Freeze the game (stop everything)
    }

    // Optional: Unsubscribe from the event when no longer needed (e.g., when the object is destroyed)
    // You can implement this in OnDestroy() if you want to unsubscribe when the GameManager is destroyed.
    
    // private void OnDestroy()
    // {
    //     foreach (Coin coin in FindObjectsOfType<Coin>())
    //     {
    //         coin.OnCoinCollected -= HandleCoinCollected;
    //     }
    // }
}


*/

