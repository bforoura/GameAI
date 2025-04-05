using UnityEngine;
using Unity.Barracuda;
using System;
using System.Collections.Generic;
using System.IO;

public class NPCBehavior : MonoBehaviour
{
    public NNModel model; // The ONNX model (ResNet50-v2)
    private IWorker worker; // The worker that runs the inference
    public Camera npcCamera; // Reference to the NPC camera

    private RenderTexture renderTexture; // Store render texture for reuse

    // A TextAsset that will be used for the labels file
    public TextAsset labelsFile; // Drag the text file here in the inspector

    // Dictionary to store labels dynamically loaded from the file
    private Dictionary<int, string> labels = new Dictionary<int, string>();

    void Start()
    {
        // Use the assigned camera, or default to the main camera
        if (npcCamera == null)
        {
            npcCamera = Camera.main;
        }

        // Load the model and create a worker for inference
        var modelAsset = model;
        var runtimeModel = ModelLoader.Load(modelAsset);
        worker = runtimeModel.CreateWorker();

        // Initialize RenderTexture (Reuse it every frame)
        renderTexture = new RenderTexture(224, 224, 24);

        // Load labels from the ImageNet class-to-label mapping file
        if (labelsFile != null)
        {
            LoadLabelsFromTextAsset(labelsFile);
        }
        else
        {
            Debug.LogError("Labels file not assigned in the Inspector.");
        }
    }

    void Update()
    {
        // Capture input for inference (could be from a texture, camera, etc.)
        Tensor inputTensor = GetInputTensor(); // Get input tensor for inference

        // Perform inference: Execute the model with the input tensor
        worker.Execute(inputTensor);

        // Retrieve the output tensor from the worker (use PeekOutput() after Execute)
        Tensor outputTensor = worker.PeekOutput(); // This will return the result as a Tensor

        // Process the output tensor to control NPC behavior
        ProcessOutput(outputTensor);

        // Dispose tensors after use to avoid memory leaks
        inputTensor.Dispose();
        outputTensor.Dispose();
    }

    // Obtain input tensor from the NPC camera's view
    Tensor GetInputTensor()
    {
        // Use the pre-created RenderTexture to capture camera output
        npcCamera.targetTexture = renderTexture;

        npcCamera.Render(); // Capture the camera's view into the render texture

        Texture2D texture = new Texture2D(224, 224, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, 224, 224), 0, 0);
        texture.Apply();

        npcCamera.targetTexture = null;
        RenderTexture.active = null;

        // Normalize the texture to match model's expected input
        NormalizeTexture(texture);

        // Dispose of texture to avoid memory leak
        Destroy(texture); // This ensures that the Texture2D is destroyed and its memory is released

        return new Tensor(texture); // Convert the texture into a tensor
    }

    void NormalizeTexture(Texture2D texture)
    {
        // Convert texture to raw pixel data
        Color[] pixels = texture.GetPixels();

        // Normalize pixel values to [0, 1] range and subtract mean & divide by standard deviation
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i].r = (pixels[i].r - 0.485f) / 0.229f; // Normalize Red channel for ResNet50
            pixels[i].g = (pixels[i].g - 0.456f) / 0.224f; // Normalize Green channel for ResNet50
            pixels[i].b = (pixels[i].b - 0.406f) / 0.225f; // Normalize Blue channel for ResNet50
        }

        texture.SetPixels(pixels);
        texture.Apply();
    }

    // Convert logits to probabilities using softmax
    float[] ApplySoftmax(float[] logits)
    {
        float maxLogit = Mathf.Max(logits);  // Avoid overflow during exponentiation
        float sum = 0.0f;

        // Compute the softmax denominator (sum of exponentials of logits)
        for (int i = 0; i < logits.Length; i++)
        {
            sum += Mathf.Exp(logits[i] - maxLogit); // Subtract maxLogit for numerical stability
        }

        // Compute the softmax probabilities
        float[] probabilities = new float[logits.Length];
        for (int i = 0; i < logits.Length; i++)
        {
            probabilities[i] = Mathf.Exp(logits[i] - maxLogit) / sum;
        }

        return probabilities;
    }

    // Process output to detect animals and humanoids
    void ProcessOutput(Tensor outputTensor)
    {
        // Convert the tensor output to a readable array
        float[] outputArray = outputTensor.ToReadOnlyArray();

        // Apply softmax to convert logits to probabilities
        float[] probabilities = ApplySoftmax(outputArray);

        // Get the class with the maximum probability (for classification tasks)
        int predictedClass = ArgMax(probabilities);  // Get the predicted class from probabilities

        // Check if the predicted class is in the labels dictionary and print the name
        if (labels.ContainsKey(predictedClass))
        {
            // Print only the label name, not the class ID
            Debug.Log($"Detected: {labels[predictedClass]}");
        }
        else
        {
            Debug.Log("Unknown object detected.");
        }
    }


    // Helper function to get the index of the maximum value in an array
    int ArgMax(float[] array)
    {
        int index = 0;
        float maxValue = array[0];
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > maxValue)
            {
                maxValue = array[i];
                index = i;
            }
        }

        return index;
    }

    void LoadLabelsFromTextAsset(TextAsset labelsFile)
    {
        if (labelsFile != null)
        {
            string[] lines = labelsFile.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                // Trim leading and trailing spaces
                string trimmedLine = line.Trim();

                // Skip empty lines or lines that don't contain a colon
                if (string.IsNullOrWhiteSpace(trimmedLine) || !trimmedLine.Contains(":"))
                {
                    continue;
                }

                // Try splitting the line into class ID and label
                string[] parts = trimmedLine.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    try
                    {
                        // Parse class ID and remove any extra spaces
                        int classId = int.Parse(parts[0].Trim());

                        // Remove any quotes or extraneous characters from the label
                        string label = parts[1].Trim().Replace("\"", "").Replace(",", "");

                        // Add the class ID and label to the dictionary
                        labels[classId] = label;
                    }
                    catch (FormatException e)
                    {
                        // Log the error for the problematic line
                        Debug.LogError($"Failed to parse line: '{line}' due to FormatException: {e.Message}");
                    }
                    catch (Exception e)
                    {
                        // Catch any other unexpected errors and log them
                        Debug.LogError($"Error processing line: '{line}' due to {e.Message}");
                    }
                }
                else
                {
                    // Log an error if the line doesn't have exactly 2 parts
                    Debug.LogError($"Skipping line with incorrect format: {line}");
                }
            }

            // After loading, log the number of labels
            Debug.Log($"Labels loaded successfully: {labels.Count} labels.");
        }
        else
        {
            Debug.LogError("Labels file is empty or not assigned.");
        }
    }

    // Proper cleanup on object destruction
    void OnDestroy()
    {
        if (worker != null)
        {
            worker.Dispose(); // Dispose of the worker to release GPU resources
        }

        if (renderTexture != null)
        {
            renderTexture.Release(); // Release the RenderTexture to avoid leaks
        }
    }
}
