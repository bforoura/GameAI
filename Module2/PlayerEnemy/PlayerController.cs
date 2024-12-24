using UnityEngine;
using System;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 50f; // Movement speed of the player
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to the player
        rb = GetComponent<Rigidbody>();
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
    }

    // Detect collisions with the enemy
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Log a message when colliding with an enemy
            Debug.Log("Player has collided with the enemy!");
        }
    }
}

