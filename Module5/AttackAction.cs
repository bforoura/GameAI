using UnityEngine;

using Pada1.BBCore;           // Core attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using BBUnity.Actions;
using UnityEngine.AI;        

/// <summary>
/// AttackAction is a custom action that makes Chomper attack a target.
/// </summary>
[Action("Chomper/Attack")] // This is how it will be listed in the editor
[Help("Chomper attacks the target if within range.")]
public class AttackAction : GOAction
{
    // The target that Chomper is attacking
    [InParam("Enemy")]
    public Transform target;
    private NavMeshAgent agent;

    // The distance at which Chomper can attack
    [InParam("attackRange")]
    public float attackRange = 2f;

    // Animator for triggering attack animation
    [InParam("animator")]
    public Animator chomperAnimator;

    // The trigger name for the attack animation
    [InParam("attackAnimationTrigger")]
    public string attackAnimationTrigger;

    // The start method of the action
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        base.OnStart();
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned for Chomper's attack.");
        }
    }

    // The update method of the action
    public override TaskStatus OnUpdate()
    {
        Debug.Log("Executing Attack action...");

        // If there is no target, fail the action
        if (target == null)
        {
            return TaskStatus.FAILED;
        }

        // Access the Chomper's transform explicitly using gameObject.transform
        float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.position);

        Debug.LogWarning("Distance to target:" + distanceToTarget);

        // Check if Chomper is within attack range of the target
        if (distanceToTarget <= attackRange)
        {
            Debug.LogWarning("Close to enemy");

            // Trigger the attack animation
            if (chomperAnimator != null)
            {
                chomperAnimator.SetTrigger("Attack");
            }

            // Attack completed successfully
            return TaskStatus.COMPLETED;
        }
        else
        {
            // If the target is out of range, fail the action
            
            return TaskStatus.FAILED;
        }
    }
}


