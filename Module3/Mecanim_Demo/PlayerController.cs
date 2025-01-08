using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator; // Reference to Animator
    private Rigidbody rb;      // Reference to Rigidbody for movement

    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 700f; // Rotation speed

    void Start()
    {
        // Get the Animator and Rigidbody components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Handle movement inputs (WASD or arrow keys)
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow keys

        // Move the player in the direction of the input
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDirection.magnitude > 0)
        {
            // Rotate to face the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the player forward
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);

            // Trigger "Walk" animation
            animator.SetBool("IsWalking", true);
        }
        else
        {
            // Trigger "Idle" animation when no movement is detected
            animator.SetBool("IsWalking", false);
        }

        // Handle "Talk" input (press "T")
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Trigger "Talk" animation, no effect on IsWalking
            animator.SetTrigger("Talk");
        }
    }
}


