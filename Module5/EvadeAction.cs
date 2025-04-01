using UnityEngine;
using Pada1.BBCore;           // Core attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using BBUnity.Actions;
using UnityEngine.AI;          // agent


// <summary>
/// EvadeAction makes Chomper run away from the target when too close.
/// </summary>

[Action("Chomper/Evade")] // New Evade action for running away from the target
[Help("Chomper evades the target when too close.")]
public class EvadeAction : GOAction
{
    [InParam("target")]
    public Transform target;  // The target to evade

    [InParam("evadeDistance")]
    public float evadeDistance = 5f; // Distance to run away from the target

    [InParam("maxEvadeTime")]
    public float maxEvadeTime = 5f; // Max time allowed to evade in seconds

    private float evadeTime = 0f;  // Timer to limit evasion duration
    private NavMeshAgent agent;

    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (target == null)
        {
            return TaskStatus.FAILED; // Return failed if there is no target
        }

        // Increment the evade timer
        evadeTime += Time.deltaTime;

        // Calculate a position to run away from the target
        Vector3 directionAwayFromTarget = gameObject.transform.position - target.position;
        Vector3 newDestination = gameObject.transform.position + directionAwayFromTarget.normalized * evadeDistance;

        // Set the new destination to move away from the target
        agent.SetDestination(newDestination);

        // If the evade time exceeds the allowed duration, stop evading
        if (evadeTime >= maxEvadeTime)
        {
            // You can return TaskStatus.COMPLETED here because the evasion action has finished.
            return TaskStatus.COMPLETED;  // Finish the evasion
        }

        // Continue evading
        return TaskStatus.RUNNING;  // Keep running if still evading
    }
}