using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;        // The player object to follow
    public float followDistance = 5f; // Distance from the player
    public float height = 2f;         // Height of the camera above the player
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement

    private Vector3 offset;          // The initial offset from the player

    void Start()
    {
        // Calculate the initial offset based on the player's position
        // If you want the camera to have a more specific angle or view, 
        // you can tweak the offset vector's Y and Z values
        offset = new Vector3(0f, height, -followDistance);
    }

    // LateUpdate() is used here to ensure the camera follows the player
    // after the player has moved, preventing the camera from lagging behind.
    void LateUpdate()
    {
        // Calculate the desired position based on the player's position and offset
        Vector3 desiredPosition = player.position + offset;

        // Smoothly move the camera towards the desired position
        // Lerp interpolates between the current camera position and the desired position
        // to make the movement appear smooth rather than abrupt.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position with the smoothed result
        transform.position = smoothedPosition;

        // Make sure the camera is always looking at the player
        // This ensures the camera stays focused on the player no matter where it moves.
        transform.LookAt(player);
    }
}


