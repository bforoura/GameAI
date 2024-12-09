using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    private AIState currentState;

    // Example variables for state transitions
    public Transform[] patrolPoints;
    private int currentPatrolPoint = 0;
    public float speed = 3f;
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float shootCooldown = 1f; // Time between shots
    private float lastShotTime = 0f;

    void Start()
    {
        // Set the initial state
        currentState = AIState.Patrol;
    }

    void Update()
    {
        // Handle state transitions
        switch (currentState)
        {
            case AIState.Patrol:
                HandlePatrolState();
                break;

            case AIState.Chase:
                HandleChaseState();
                break;

            case AIState.Shoot:
                HandleShootState();
                break;
        }
    }

    // Handle the Patrol state
    private void HandlePatrolState()
    {
        Debug.Log("AI is patrolling.");

        // Move towards the current patrol point
        Transform target = patrolPoints[currentPatrolPoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // If the AI reaches the patrol point, move to the next one
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        }

        // Check if the player is within detection range to switch to Chase state
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = AIState.Chase;
        }
    }

    // Handle the Chase state
    private void HandleChaseState()
    {
        Debug.Log("AI is chasing the player.");

        // Move towards the player's position
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Check if the player is within attack range to switch to Shoot state
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = AIState.Shoot;
        }

        // If the player is out of detection range, go back to Patrol
        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = AIState.Patrol;
        }
    }

    // Handle the Shoot state
    private void HandleShootState()
    {
        Debug.Log("AI is shooting the player.");

        // Only shoot if enough time has passed since the last shot (shootCooldown)
        if (Time.time - lastShotTime >= shootCooldown)
        {
            ShootAtPlayer();
            lastShotTime = Time.time; // Update last shot time
        }

        // After shooting, go back to Patrol state if the player moves out of range
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = AIState.Patrol;
        }
    }

    // Simulate shooting behavior (in this case, just a simple log message)
    private void ShootAtPlayer()
    {
        // This is where you can add actual shooting logic (e.g., spawn projectiles, damage the player, etc.)
        Debug.Log("AI shoots at the player!");

        // For simplicity, let's assume the player dies immediately when shot.
        // You can replace this with your own logic.
        if (player != null && Vector3.Distance(transform.position, player.position) < attackRange)
        {
            Debug.Log("Player killed by AI!");
            // Add player death logic (e.g., destroy the player or disable its movement)
            Destroy(player.gameObject); // For demo purposes, we're destroying the player.
        }
    }
}
