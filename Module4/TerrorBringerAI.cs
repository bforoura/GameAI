using UnityEngine;
using UnityEngine.AI;

namespace Gamekit3D
{
    public class TerrorBringerAI : MonoBehaviour
    {
        public Transform target;  // Ellen's Transform
        public float chaseDistance = 10f;  // Distance at which the dragon will start chasing
        public float stopDistance = 1f;    // Distance at which the dragon stops chasing
        public float moveSpeed = 3.5f;    // Speed of the dragon's movement
        public float stuckThreshold = 0.5f; // Threshold to consider the AI stuck
        public float stuckTime = 2f; // Time before resetting if the AI is stuck

        private NavMeshAgent agent;
        private Animator animator;
        private float timeStuck = 0f; // Timer for stuck detection
        private Vector3 lastPosition;
        private bool isFirstFrame = true; // Flag to check if it's the first frame

        void Start()
        {
            // Get references to necessary components
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            // Initialize the NavMeshAgent with the speed
            agent.speed = moveSpeed;
            lastPosition = transform.position;
        }

        void Update()
        {
            // Only chase if Ellen is assigned and is within chase range
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (distance <= chaseDistance)
                {
                    ChaseTarget();
                }
                else
                {
                    StopChasing();
                }

                // Check if the dragon is stuck (skip on first frame)
                if (!isFirstFrame)
                {
                    CheckIfStuck();
                }
            }

            // Update animator parameters (Speed and IsChasing)
            UpdateAnimation();
        }

        // Called when Ellen enters the trigger zone
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Check if the collider belongs to Ellen
            {
                target = other.transform; // Set Ellen as the target to chase
            }
        }

        // Called when Ellen exits the trigger zone
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                target = null;  // Stop chasing when Ellen leaves the zone
            }
        }

        // Chase Ellen using the NavMeshAgent
        private void ChaseTarget()
        {
            if (target != null)
            {
                // Set the dragon's destination to Ellen's position
                agent.SetDestination(target.position);
                animator.SetBool("IsChasing", true);  // Trigger chase animation
            }

            // Stop chasing when the dragon gets close enough to Ellen
            if (Vector3.Distance(transform.position, target.position) <= stopDistance && agent.hasPath)
            {
                StopChasing();
            }
        }

        // Stop chasing and reset movement
        private void StopChasing()
        {
            // Stop the NavMeshAgent movement
            agent.SetDestination(transform.position);  
            animator.SetBool("IsChasing", false);  // Stop chasing animation
        }

        // Check if the dragon is stuck
        private void CheckIfStuck()
        {
            // Skip stuck detection on the first frame
            if (isFirstFrame)
            {
                isFirstFrame = false; // Mark that the first frame has passed
                return;
            }

            // Check if the dragon hasn't moved significantly in the last frame
            if (Vector3.Distance(transform.position, lastPosition) < stuckThreshold)
            {
                timeStuck += Time.deltaTime; // Increment stuck time
            }
            else
            {
                timeStuck = 0f; // Reset stuck time if the dragon moved
            }

            lastPosition = transform.position;

            // If the dragon has been stuck for too long, reset its path
            if (timeStuck >= stuckTime)
            {
                HandleStuckState();
            }
        }

        // Handle the stuck state by resetting the AI's path
        private void HandleStuckState()
        {
            Debug.Log("TerrorBringer is stuck. Resetting path...");
            agent.ResetPath();  // Reset the NavMeshAgent path to force it to find a new one
            timeStuck = 0f; // Reset the stuck timer
        }

        // Update animation parameters
        private void UpdateAnimation()
        {
            // Get the current speed of the dragon using the NavMeshAgent's velocity
            float speed = agent.velocity.magnitude;

            // Update the Speed parameter in the animator
            animator.SetFloat("Speed", speed);

            // Update the IsChasing boolean to control the chase animation
            animator.SetBool("IsChasing", target != null);
        }
    }
}
