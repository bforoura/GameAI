using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;
using UnityEngine.AI;

/// <summary>
/// SummonChompersAction is a custom action that makes all Chompers in the pack move towards Ellen's current position.
/// </summary>
[Action("Chomper/SummonChompers")]
[Help("Summons all Chompers in the pack to continuously follow Ellen.")]
public class SummonChompersAction : GOAction
{
    // Transform of Ellen (the player) that Chompers will move toward
    [InParam("ellenPosition")]
    public Transform ellenTransform;

    // Distance at which Chompers should stop walking and reset animation
    public float stoppingDistance = 1f;

    // Called when the action starts
    public override void OnStart()
    {
        base.OnStart();

        // Check if Ellen's transform is assigned
        if (ellenTransform == null)
        {
            Debug.LogWarning("Ellen's transform is not assigned.");
        }
    }

    // Called every frame to execute the action
    public override TaskStatus OnUpdate()
    {
        // If Ellen's position is not assigned, fail the action
        if (ellenTransform == null)
        {
            return TaskStatus.FAILED; // If Ellen's position is not available, fail the action
        }

        // Find all Chompers in the "Pack" layer within a 100-unit radius of Ellen's position
        // Physics.OverlapSphere creates a spherical detection zone centered on Ellen's position
        // The 100f is the radius of this detection zone
        // The LayerMask (1 << packLayer) filters out any non-Chomper objects by checking if they are in the "Pack" layer
        
        // Explanation:
        //    Total Layers: 32 (numbered 0 to 31)
        //    Bitmask for Layer 3: 00000000000000000000000000001000 (binary representation of 8)
        //    In this bitmask:
        //         Bits at positions 0, 1, 2, and 4 to 31 are set to 0, excluding layers 0, 1, 2, and all layers above 3.
        
        // Example:
        //    int layer0 = 1 << 0;      layer 0
        //    int layer2 = 1 << 2;      layer 2
        //    int layer4 = 1 << 4;      layer 4
        //    int combinedMask = layer0 | layer2 | layer4;     Combine all the layers
        
        int packLayer = LayerMask.NameToLayer("Pack"); // Get the "Pack" layer index
        Collider[] chompersInRange = Physics.OverlapSphere(ellenTransform.position, 100f, 1 << packLayer);

        // If no Chompers are found in range, fail the action
        if (chompersInRange.Length == 0)
        {
            Debug.LogWarning("No Chompers found in the Pack layer.");
            return TaskStatus.FAILED;
        }

        // Command each Chomper to move to Ellen's position
        foreach (Collider chomperCollider in chompersInRange)
        {
            // Get the NavMeshAgent of each Chomper and set its destination to Ellen's position
            NavMeshAgent chomperAgent = chomperCollider.GetComponent<NavMeshAgent>();
            if (chomperAgent != null)
            {
                chomperAgent.SetDestination(ellenTransform.position); // Keep following Ellen's position
            }

            // Set the IsWalking boolean in the Animator to true to trigger the walking animation
            Animator chomperAnimator = chomperCollider.GetComponent<Animator>();
            if (chomperAnimator != null)
            {
                chomperAnimator.SetBool("IsWalking", true); // Start walking animation for all Chompers
            }
        }

        // The action will remain ongoing, as the Chompers are supposed to follow Ellen indefinitely
        return TaskStatus.RUNNING;
    }
}
