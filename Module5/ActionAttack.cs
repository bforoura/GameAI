using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using BBUnity.Actions;        // GOAction

/// <summary>
/// AttackAction is a custom action that makes Chomper attack a target.
/// </summary>
[Action("Chomper/Attack")] // This is how it will be listed in the editor
[Help("Chomper attacks the target if within range.")]
public class AttackAction : GOAction
{
    // The target that Chomper is attacking
    [InParam("target")]
    public Transform target;

    // The distance at which Chomper can attack
    [InParam("attackRange")]
    public float attackRange = 2f;

    // Animator for triggering attack animation
    [InParam("animator")]
    public Animator chomperAnimator;

    // The trigger name for the attack animation
    [InParam("attackAnimationTrigger")]
    public string attackAnimationTrigger = "Attack";

    // The start method of the action
    public override void OnStart()
    {
        base.OnStart();
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned for Chomper's attack.");
        }
    }

    // The update method of the action
    public override TaskStatus OnUpdate()
    {
        // If there is no target, fail the action
        if (target == null)
        {
            return TaskStatus.FAILED;
        }

        // Access the Chomper's transform explicitly using gameObject.transform
        float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.position);

        // Check if Chomper is within attack range of the target
        if (distanceToTarget <= attackRange)
        {
            // Trigger the attack animation
            if (chomperAnimator != null)
            {
                chomperAnimator.SetTrigger(attackAnimationTrigger);
            }

            // Attack completed successfully (no damage logic for now)
            return TaskStatus.COMPLETED;
        }
        else
        {
            // If the target is out of range, fail the action
            return TaskStatus.FAILED;
        }
    }
}

