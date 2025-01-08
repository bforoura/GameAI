using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    public float moveSpeed = 10f;  // Speed of movement.
    public float rotationSpeed = 200f;  // Speed of rotation for smooth turning.
    private Rigidbody rb;  // Reference to the Rigidbody.
    private Animator animator;  // Reference to the Animator.

    void Start()
    {
        // Get references to the Rigidbody and Animator components.
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input from WASD keys (or Arrow keys).
        float horizontal = Input.GetAxis("Horizontal");  // A/D or Left/Right arrows.
        float vertical = Input.GetAxis("Vertical");      // W/S or Up/Down arrows.

        // Move the Bear using WASD input.
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;  // 3D movement.

        // Apply the movement to the Rigidbody.
        if (moveDirection.magnitude >= 0.1f)
        {
            // Move the Bear forward in the direction of input.
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);

            // Rotate the Bear to face the movement direction.
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Set the "Speed" parameter in Animator to make the running animation play.
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
        else
        {
            // Set "Speed" to 0 if not moving.
            animator.SetFloat("Speed", 0);
        }

        // Check if the "T" key is pressed to trigger the aTtack animation.
        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.SetTrigger("AttackTrigger"); // Trigger the Attack animation.
        }
    
    }
}