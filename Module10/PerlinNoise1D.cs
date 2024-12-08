using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PerlinNoise1D : MonoBehaviour
{
    // Parameters for Perlin noise
    public float scale = 1.0f;   // The scale of the Perlin noise
    public float speed = 0.1f;   // Speed at which the noise shifts over time
    public float offset = 0.0f;  // An offset value to animate the noise

    // Update is called once per frame
    void Update()
    {
        // Generate 1D Perlin noise by fixing the 'y' value and varying 'x'
        float x = Time.time * speed; // You can scale this for speed control
        float y = offset;            // You can animate the offset for a dynamic result
        
        // Generate 2D Perlin noise with the y value fixed
        float perlinValue = Mathf.PerlinNoise(x * scale, y);

        // Use the Perlin noise value to affect the GameObject's y position
        transform.position = new Vector3(transform.position.x, perlinValue * 5f, transform.position.z);  // Adjust '5f' to control the range of motion
    }
}
