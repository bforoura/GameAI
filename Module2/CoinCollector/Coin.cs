using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource audioSource;
    private GameManager gameManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the sound effect
            audioSource.Play();

            // Update the score in the GameManager
            gameManager.AddScore(1); // Increment score by 1

            // Start the rotation and destruction coroutine
            StartCoroutine(RotateAndDestroy());
        }
    }

    private IEnumerator RotateAndDestroy()
    {
        float rotationSpeed = 1080f; // Degrees per second
        float totalRotation = 0f;
        float duration = 0.5f; // Total duration in seconds for the rotation

        while (totalRotation < rotationSpeed * 2 * duration) // Rotate two times
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationThisFrame);
            totalRotation += rotationThisFrame;
            yield return null; // Wait for the next frame
        }

        // Destroy the coin after rotating
        Destroy(gameObject);
    }
}