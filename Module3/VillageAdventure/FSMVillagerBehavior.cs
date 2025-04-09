using System.Collections;
using UnityEngine;


public class FSMVillagerBehavior : MonoBehaviour 
{
    public Transform workLocation; // Reference to the Work location
    public Transform hideLocation; // Reference to the Hide location

    public float moveSpeed = 2f; // Movement speed
    public float stopRadius = 0.5f; // Radius within which the villager will stop at the hide location

    public float rotationSpeed = 5f; // Speed at which the villager rotates towards the hide location
    public float detectionRadius = 10f; // The detection radius for when the Bear is nearby

    private Animator animator; // Reference to the Animator component
    private GameObject bearPlayer;  // Reference to the Bear

    private enum VillagerState
    {
        Working1,  // Moving to work1 location (and hide)
        Working2   // Moving to work2 location (and hide)
    }

    private VillagerState currentState; // State to determine if the villager is working or hiding
    private bool isMoving = false;  // Flag to check if the villager is moving
    private bool bearIsNearby = false; // Tracks if the bear is nearby

    private void Start()
    {
        animator = GetComponent<Animator>();
        bearPlayer = GameObject.FindGameObjectWithTag("Bear");  
        currentState = VillagerState.Working1; // Start in the Working1 state
    }

    private void Update()
    {
        // Always check if the bear is nearby
        bearIsNearby = Vector3.Distance(transform.position, bearPlayer.transform.position) < detectionRadius;

        // Continuous FSM logic to respond to the bear's proximity
        switch (currentState)
        {
            case VillagerState.Working1:
                if (bearIsNearby && !isMoving) // If the bear is nearby, flee to the other side
                {
                    StartCoroutine(MoveToLocation(hideLocation, VillagerState.Working2));
                }
                break;

            case VillagerState.Working2:
                if (bearIsNearby && !isMoving) // If the bear is nearby, flee to the other side
                {
                    StartCoroutine(MoveToLocation(workLocation, VillagerState.Working1));
                }
                break;
        }
    }

    private IEnumerator MoveToLocation(Transform targetLocation, VillagerState nextState)
    {
        if (isMoving) yield break;  // Prevent starting another movement if already moving

        isMoving = true;  // Set the flag to indicate movement has started
        animator.SetBool("IsFleeing", true); // Trigger fleeing animation

        // Move towards the target location until close enough
        while (Vector3.Distance(transform.position, targetLocation.position) > stopRadius)
        {
            Vector3 direction = (targetLocation.position - transform.position).normalized;

            // Smoothly rotate towards the target location
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move towards the target location
            transform.position = Vector3.MoveTowards(transform.position, targetLocation.position, moveSpeed * Time.deltaTime);

            yield return null;
        }

        // Once close enough, stop at the target location
        transform.position = targetLocation.position;
        animator.SetBool("IsFleeing", false); // Stop fleeing animation

        isMoving = false;  // Set the flag to allow for the next movement

        // After reaching the destination, decide where to go next
        if (bearIsNearby)
        {
            currentState = nextState;  // Switch to the opposite state
            StartCoroutine(MoveToLocation(nextState == VillagerState.Working1 ? workLocation : hideLocation, nextState));
        }
        else
        {
            // Stay at the current location if the bear is not nearby
            currentState = nextState == VillagerState.Working1 ? VillagerState.Working1 : VillagerState.Working2;
        }
    }
}
