using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;
    [SerializeField] private float fps;  // This will be visible in the Inspector

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;  // Calculate FPS
    }

    // The FPS value will automatically be updated and shown in the Inspector
}