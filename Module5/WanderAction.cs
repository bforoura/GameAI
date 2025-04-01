using Pada1.BBCore;
using BBUnity.Actions;
using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore.Tasks;

[Action("Chomper/Wander")]
public class WanderAction : GOAction
{
    [InParam("pointA")]
    public Transform pointA;
    [InParam("pointB")]
    public Transform pointB;
    [InParam("Speed")]
    public int speed;
    private NavMeshAgent agent;
    private Animator animator;
    private bool movingToB = true; // Flag to toggle between A and B


    public override void OnStart()
    {
        Debug.Log("Executing Wander action...");
        
        agent = gameObject.GetComponent<NavMeshAgent>();
        
        agent.speed = speed;  // Speed up Chomper
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("IsWalking", true);

        // Initially set the destination to point A or B
        agent.SetDestination(pointA.position);
    }

    public override TaskStatus OnUpdate()
    {
        // Check if the agent has reached the destination and is not waiting for a path
        if ((agent.remainingDistance < 0.5f && !agent.pathPending))  
        {
            // If at point A, move to point B, otherwise move to point A
            if (movingToB)
            {
                agent.SetDestination(pointB.position);
            }
            else
            {
                agent.SetDestination(pointA.position);
            }
            
            // Toggle the movement flag
            movingToB = !movingToB;
        }

        return TaskStatus.RUNNING;  
    }

}


