using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class NPCMovement : MonoBehaviour
{
    public Transform target; // The target to move to 
    private GameObject chomper;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        chomper = GameObject.Find("Chomper");
        agent = chomper.GetComponent<NavMeshAgent>();
        agent.speed = 10f;
        animator = chomper.GetComponent<Animator>();
        
        StartCoroutine(MoveAgentAfterDelay());
    }

    void Update()
    {
        // Visualize the NavMeshPath with LineRenderer in every frame
        VisualizePath(agent.path);
    }

    private void VisualizePath(NavMeshPath path)
    {
        LineRenderer lineRenderer = chomper.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = chomper.AddComponent<LineRenderer>(); // Add LineRenderer if it's not already attached
        }

        float pathWidth = 1f; // Set the width of the line
        Color pathColor = Color.yellow; // Set the color of the path

        // Set LineRenderer properties
        lineRenderer.startWidth = pathWidth;
        lineRenderer.endWidth = pathWidth;
        lineRenderer.startColor = pathColor;
        lineRenderer.endColor = pathColor;

        // Check if the path has valid corners
        if (path.status == NavMeshPathStatus.PathComplete || path.status == NavMeshPathStatus.PathPartial)
        {
            // Set the number of positions for the LineRenderer
            lineRenderer.positionCount = path.corners.Length;

            // Set the positions of the LineRenderer to match the path corners
            for (int i = 0; i < path.corners.Length; i++)
            {
                lineRenderer.SetPosition(i, path.corners[i]);
            }
        }
        else
        {
            // If no valid path, clear the LineRenderer
            lineRenderer.positionCount = 0;
        }
    }

    private IEnumerator MoveAgentAfterDelay()
    {
        // Wait for 3 seconds before moving
        yield return new WaitForSeconds(3f);

        // Set the destination after the delay
        agent.SetDestination(target.position);

        // Start the walking animation
        animator.SetBool("IsWalking", true);

        // Continue moving until the agent is close to its destination
        while (true)
        {
            // Check if the agent has reached its destination
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                // Stop the walking animation once the agent reaches the target
                animator.SetBool("IsWalking", false);
                break;
            }

            yield return null; // Wait for next frame to check again
        }
    }
}





