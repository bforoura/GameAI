using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Find all GameObjects with the "Player" tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        // Find all GameObjects with the "Coin" tag
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");

        // Print out the names of the Player objects
        Debug.Log("Players in the scene:");
        foreach (var player in players)
        {
            Debug.Log(player.name);
        }

        // Print out the names of the Coin objects
        Debug.Log("Coins in the scene:");
        foreach (var coin in coins)
        {
            Debug.Log(coin.name);
        }
    }
}

