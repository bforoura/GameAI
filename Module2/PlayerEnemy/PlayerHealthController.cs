using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    public float moveSpeed = 50f; // Movement speed of the player
    public int playerHealth = 100; // Player's starting health
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to the player
        rb = GetComponent<Rigidbody>();

        // Lock rotation on the X and Z axes to prevent the capsule from tilting
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Get input from the player (WASD or arrow keys)
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        // Create a movement vector based on input
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.deltaTime;

        // Apply movement to the player
        rb.MovePosition(transform.position + movement);

        // Check if health is zero or below, destroy the player if so
        if (playerHealth <= 0)
        {
            Debug.Log("Game over: The Player is dead!");
            // Destroy(gameObject); // Destroy the player object

            QuitGame();
        }
    }

    // Detect collisions with the enemy
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Log a message when colliding with an enemy
            Debug.Log("Player has collided with the enemy!");

            // Decrease player health by a fixed amount (e.g., 10)
            playerHealth -= 10;

            // Optionally, display the current health
            Debug.Log("Player health: " + playerHealth);
        }
    }


    void QuitGame()
    {
        // The hash symbols (#) in the code are part of preprocessor directives in C#. 
        // These are used to instruct the compiler to conditionally compile (or exclude) 
        // part of code based on certain conditions.
        
        // Quit the game
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // Stop play mode in the editor
        #else
            Application.Quit();  // Quit the game in a build
        #endif
    }



    void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1;  // Resume the game
    }

}

