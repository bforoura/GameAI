using UnityEngine;
using Pada1.BBCore;           // Core attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using BBUnity.Actions;
using UnityEngine.AI;          // GOAction

/// <summary>
/// ChaseAction makes Chomper chase the target until within attack range.
/// </summary>
[Action("Chomper/Chase")] // New Chase action for moving toward the target
[Help("Chomper chases the target until within attack range.")]
public class ChaseAction : GOAction
{
    [InParam("target")]
    public Transform target;  // The target to chase
    private NavMeshAgent agent;
    [InParam("chaseSpeed")]
    public float chaseSpeed = 3.5f; // Speed at which Chomper chases the target

    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed; // Set Chomper's chase speed
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null)
        {
            return TaskStatus.FAILED; // Return failed if there is no target
        }

        // Move Chomper towards the target
        agent.SetDestination(target.position);

        // Check if the target is within attack range
        if (Vector3.Distance(gameObject.transform.position, target.position) <= 2f)
        {
            return TaskStatus.COMPLETED;  // Chomper has reached attack range
        }

        return TaskStatus.RUNNING;  // Continue chasing
    }
}
