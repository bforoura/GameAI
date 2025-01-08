using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Camera mainCamera;           // Reference to the main camera
    public Transform bearTransform;     // Reference to the Bear's Transform directly
    public Vector3 offset = new Vector3(-20, 7, 10);  // Camera position offset relative to the Bear
    public float smoothSpeed = 0.125f;  // Smooth follow speed

    void Start()
    {
        // Set initial camera position for third-person view
        if (bearTransform != null)
        {
            // Set the camera's initial position
            mainCamera.transform.position = bearTransform.position + offset;
            mainCamera.transform.LookAt(bearTransform);  // Ensure the camera is looking at the Bear
        }
    }

    void LateUpdate()
    {
        if (bearTransform != null)
        {
            // Only adjust the camera's position without interfering with the Bear's position.
            Vector3 desiredPosition = bearTransform.position + offset;

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position (This does not change the Bear's position)
            mainCamera.transform.position = smoothedPosition;

            // Make the camera always look at the Bear
            mainCamera.transform.LookAt(bearTransform);
        }
    }
}



