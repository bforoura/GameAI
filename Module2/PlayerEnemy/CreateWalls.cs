using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CreateWalls : MonoBehaviour
{
    public GameObject Ground; // Reference to the ground (cube)
    
    public float wallThickness = 1f; // Thickness of the walls
    
    private float groundWidth, groundLength, groundHeight;

    void Start()
    {
        // Ground Validation: The first thing we check is whether the Ground object is assigned in the Inspector. 
        // If it's missing, an error is logged.
        if (Ground == null)
        {
            Debug.LogError("Ground is not assigned!");
            return;
        }

        // Get the dimensions of the ground (assuming it's a cube)
        groundWidth = Ground.transform.localScale.x;
        groundLength = Ground.transform.localScale.z;
        groundHeight = Ground.transform.localScale.y;

        Debug.Log($"Ground size - Width: {groundWidth}, Length: {groundLength}, Height: {groundHeight}");

        // Create the four vertical walls around the ground
        CreateVerticalWalls();
    }

    // Make this public to allow access 
    
    public void CreateVerticalWalls()
    {
      // Ensure the walls are tall enough to reach above the ground
      float wallHeight = 5.0f; // Wall height

      // The ground's position and scale
      Vector3 groundPosition = Ground.transform.position;
      Vector3 groundScale = Ground.transform.localScale;

      // Left Wall: Position it just outside the left side of the ground
      CreateWall("Wall_Left", 
          new Vector3(groundPosition.x - groundScale.x / 2 - wallThickness / 2, groundPosition.y + wallHeight / 2, groundPosition.z), 
          new Vector3(wallThickness, wallHeight, groundScale.z));

      // Right Wall: Position it just outside the right side of the ground
      CreateWall("Wall_Right", 
          new Vector3(groundPosition.x + groundScale.x / 2 + wallThickness / 2, groundPosition.y + wallHeight / 2, groundPosition.z), 
          new Vector3(wallThickness, wallHeight, groundScale.z));

      // Front Wall: Position it just outside the front side of the ground
      CreateWall("Wall_Front", 
          new Vector3(groundPosition.x, groundPosition.y + wallHeight / 2, groundPosition.z + groundScale.z / 2 + wallThickness / 2), 
          new Vector3(groundScale.x, wallHeight, wallThickness));

      // Back Wall: Position it just outside the back side of the ground
      CreateWall("Wall_Back", 
          new Vector3(groundPosition.x, groundPosition.y + wallHeight / 2, groundPosition.z - groundScale.z / 2 - wallThickness / 2), 
          new Vector3(groundScale.x, wallHeight, wallThickness));
    }




    // Keep this method private since it's only used internally
    private void CreateWall(string wallName, Vector3 position, Vector3 size)
    {
        // Create an empty GameObject for each wall
        GameObject wall = new GameObject(wallName);

        // Set the position of the wall
        wall.transform.position = position;

        // Add a BoxCollider to it for collision detection
        BoxCollider boxCollider = wall.AddComponent<BoxCollider>();
        boxCollider.size = size;

        // Make the wall invisible by disabling the MeshRenderer
        MeshRenderer meshRenderer = wall.AddComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }
}





