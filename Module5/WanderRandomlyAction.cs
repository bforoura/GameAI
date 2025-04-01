using Pada1.BBCore;
using BBUnity.Actions;
using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore.Tasks;

[Action("Chomper/WanderRandomly")]
public class WanderRandomlyAction : GOAction
{
    [InParam("pointA")]
    public Transform pointA;  // Starting point A

    [InParam("pointB1")]
    public Transform pointB1;  // Random target point B1

    [InParam("pointB2")]
    public Transform pointB2;  // Random target point B2

    private NavMeshAgent agent;
    private Animator animator;
    private bool movingToA = false;  // Start by moving to a random point, not A
    private Transform targetPoint;

    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = 30;   // Speed up Chomper and enable its walking animation
        
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("IsWalking", true);

        // Start by randomly choosing either B1 or B2
        targetPoint = (Random.Range(0, 2) == 0) ? pointB1 : pointB2;
        agent.SetDestination(targetPoint.position);
    }

    public override TaskStatus OnUpdate()
    {
        // Check if we've reached the current destination (either one of the random points or Point A)
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            // If we are at one of the random points (B1 or B2), move back to A
            if (!movingToA)
            {
                agent.SetDestination(pointA.position);  // Move back to A
                targetPoint = pointA;  // Set target to A
                movingToA = true;  // After reaching A, we will move randomly to B1 or B2 next
            }
            else
            {
                // If we are at A, randomly choose B1 or B2
                targetPoint = (Random.Range(0, 2) == 0) ? pointB1 : pointB2;
                agent.SetDestination(targetPoint.position);  // Move to the selected random point
                movingToA = false;  // After reaching B1 or B2, we will move back to A next
            }
        }

        return TaskStatus.RUNNING;  // Keep the task running to repeat the cycle
    }
}
