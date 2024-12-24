using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(moveX, moveY, 0);

         // Flip the player sprite based on direction
        if (moveX > 0) // Moving right
        {
            spriteRenderer.flipX = true; // Face right
        }
        else if (moveX < 0) // Moving left
        {
            spriteRenderer.flipX = false; // Face left
        }
    }
}