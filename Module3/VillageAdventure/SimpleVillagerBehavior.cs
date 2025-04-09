using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Needed for UI components like Text, Button, etc.


public class SimpleVillagerBehavior : MonoBehaviour
{
    // Animation state names
    public string idleAnimationName = "Idle";  // Name of the idle animation state
    public string workAnimationName = "Working"; // Name of the working animation state

    public float detectionRadius = 10f; // The detection radius for when the Bear is nearby

    private Animator animator;  // Reference to the villager's Animator
    private GameObject bearPlayer;  // Reference to the Bear
    private Transform bearTransform; // Reference to the Bear's Transform
    private Animator bearAnimator; // Referece to the Bear's Animator
    private DialogueManager dialogueManager;  // Reference to the Dialogue Manager

    private void Start()
    {
        // Automatically find the Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        // Ensure animator is found, otherwise log a warning
        if (animator == null)
        {
            Debug.LogWarning("No Animator component found on " + gameObject.name);
        }

        // Try to find the Bear in the scene by its tag 
        bearPlayer =  GameObject.FindGameObjectWithTag("Bear");
        bearTransform = bearPlayer.transform;
        bearAnimator = bearPlayer.GetComponent<Animator>();

        if (bearTransform == null)
        {
            Debug.LogWarning("Bear not found. Make sure it is tagged as 'Bear'");
        }

        // Find the DialogueManager in the scene
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the Bear is within the trigger zone
        if (other.CompareTag("Bear"))  // Make sure Bear is tagged as "Bear"
        {
            StopWorking();
            ShowDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the Bear exits the detection zone
        if (other.CompareTag("Bear"))
        {
            ResumeWorking();
            HideDialogue();
            
            bearAnimator.SetTrigger("ExcitedTrigger"); // Get the Bear excited  
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

    private void ShowDialogue()
    {
        if (dialogueManager != null)
        {
        dialogueManager.ShowDialogue("The last time I saw you, I thought you were in a bear costume, but nope, it's just the 'tax collector'!");
        }
    }

    private void HideDialogue()
    {
        if (dialogueManager != null)
        {
            dialogueManager.HideDialogue();
        }
    }
}
