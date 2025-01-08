using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VillagerBehavior : MonoBehaviour
{
    // Animation state names
    public string idleAnimationName = "Idle";  // Name of the idle animation state
    public string workAnimationName = "Working"; // Name of the working animation state

    public float detectionRadius = 10f; // The detection radius for when the Bear is nearby

    private Animator animator;  // Reference to the villager's Animator
    private Transform bearTransform; // Reference to the Bear's Transform

    private void Start()
    {
        // Automatically find the Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        // Ensure animator is found, otherwise log a warning
        if (animator == null)
        {
            Debug.LogWarning("No Animator component found on " + gameObject.name);
        }

        // Try to find the Bear in the scene by its tag (make sure Bear is tagged as "Bear")
        bearTransform = GameObject.FindGameObjectWithTag("Bear")?.transform;

        if (bearTransform == null)
        {
            Debug.LogWarning("Bear not found. Make sure it is tagged as 'Bear'");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the Bear is within the trigger zone
        if (other.CompareTag("Bear"))  // Make sure Bear is tagged as "Bear"
        {
            StopWorking();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the Bear exits the detection zone
        if (other.CompareTag("Bear"))
        {
            ResumeWorking();
        }
    }

    private void StopWorking()
    {
        // Transition to Idle animation when the Bear is detected
        if (animator != null)
        {
            animator.SetBool("IsIdle", true);  // Assuming "IsIdle" is a bool in the Animator controller
            animator.SetBool("IsWorking", false); // Assuming "IsWorking" is a bool in the Animator controller
        }

    
    }

    private void ResumeWorking()
    {
        // Transition back to Working animation when Bear leaves
        if (animator != null)
        {
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWorking", true);
        }
    }

}
