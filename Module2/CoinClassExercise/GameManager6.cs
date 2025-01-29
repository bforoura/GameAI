using UnityEngine;

public class GameManager6 : MonoBehaviour
{
    public int coinsToCollect = 10;  // Set how many coins are needed to win
    public GameObject coinPrefab;  // Reference to the coin prefab
    public Transform[] spawnPoints;  // Array of spawn points (SP1, SP2, ..., SP10)

    void Start()
    {
        // Spawn coins at the spawn points before the game starts
        SpawnCoins();
    }

    // Method to spawn coins at the spawn points
    private void SpawnCoins()
    {
        // Ensure that you only spawn the correct number of coins
        for (int i = 0; i < spawnPoints.Length; i++)

            // Instantiate a coin at the spawn point's position
            Instantiate(coinPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
        
    }

}