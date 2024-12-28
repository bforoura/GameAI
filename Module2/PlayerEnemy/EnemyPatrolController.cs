using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolController : MonoBehaviour
{
    public float moveSpeed = 3f;          // Speed at which the enemy moves.
    public float patrolRange = 5f;        // The distance the enemy will patrol.
    private Vector3 startPosition;        // Starting position of the enemy.
    private float patrolTimer;            // Timer to control the patrol movement.

    void Start()
    {
        startPosition = transform.position; // Set the starting position.
    }

    void Update()
    {
        Patrol(ref patrolTimer, in startPosition, out float patrolX);
        transform.position = new Vector3(startPosition.x + patrolX, transform.position.y, transform.position.z);
    }

    // Demonstrating in, ref, and out parameters
    void Patrol(ref float timer, in Vector3 startPos, out float patrolX)
    {
        // Increment the patrol timer, modifying the timer (ref).
        timer += Time.deltaTime * moveSpeed;

        // Calculate patrol X based on a sine function (use in to ensure startPos isn't modified).
        patrolX = Mathf.Sin(timer) * patrolRange;

        // Note: startPos is read-only and cannot be modified here.
    }
}

