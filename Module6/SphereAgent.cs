using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SphereAgent : Agent
{
    // Rigidbody component to control the agent's physics (movement)
    Rigidbody rBody;

    // Reference to the target (the cube)
    public Transform Target;

    // Multiplier for the force applied to the agent
    public float forceMultiplier = 10;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component attached to the agent (sphere)
        rBody = GetComponent<Rigidbody>();
    }

    // This method is called at the start of a new episode
    override public void OnEpisodeBegin()
    {
        // If the agent falls off the platform (y-position < -1), reset its position and stop movement
        if (transform.position.y < -1.0f)
        {
            // Reset the agent's position to the center of the platform
            transform.position = Vector3.zero;

            // Stop any angular velocity (rotation) and linear velocity (movement)
            rBody.angularVelocity = Vector3.zero;
            rBody.velocity = Vector3.zero;
        }
        else
        {
            // If the agent hasn't fallen, move the target (cube) to a random position within the platform bounds
            Target.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        }
    }

    // This method collects observations about the environment (state) for the agent
    override public void CollectObservations(VectorSensor sensor)
    {
        // Calculate the relative position of the target from the agent's position
        Vector3 relativePosition = Target.position - transform.position;

        // Add observations of the relative position (scaled to make them smaller values)
        sensor.AddObservation(relativePosition.x / 5);  // Relative x position
        sensor.AddObservation(relativePosition.z / 5);  // Relative z position

        // Add observations about the distance to the edges of the platform (x and z directions)
        sensor.AddObservation((transform.position.x + 5) / 5);  // Distance to the left edge of the platform
        sensor.AddObservation((transform.position.x - 5) / 5);  // Distance to the right edge of the platform
        sensor.AddObservation((transform.position.z + 5) / 5);  // Distance to the bottom edge of the platform
        sensor.AddObservation((transform.position.z - 5) / 5);  // Distance to the top edge of the platform

        // Add observations about the agent's velocity (scaled to smaller values)
        sensor.AddObservation(rBody.velocity.x / 5);  // Velocity in the x direction
        sensor.AddObservation(rBody.velocity.z / 5);  // Velocity in the z direction
    }

    // This method is called every step to apply actions based on the agent's decision
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions contain two continuous values: one for the x direction (left/right), one for the z direction (forward/backward)
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];  // Action for horizontal movement
        controlSignal.z = actionBuffers.ContinuousActions[1];  // Action for vertical movement

        // Apply a force to the Rigidbody based on the control signal
        rBody.AddForce(controlSignal * forceMultiplier);  // Adjust the agent's speed based on the force multiplier

        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // If the agent reaches the target (distance < 1.42), reward it and end the episode
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);  // Positive reward for reaching the target
            EndEpisode();  // End the current episode
        }

        // If the agent falls off the platform (y-position < 0), end the episode
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();  // End the episode if the agent falls
        }
    }

    // This method allows for manual control of the agent during testing
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Get the continuous actions for manual control (Horizontal and Vertical inputs from keyboard)
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");  // A/D or Left/Right arrow keys for horizontal movement
        continuousActionsOut[1] = Input.GetAxis("Vertical");  // W/S or Up/Down arrow keys for vertical movement
    }

    // Update is called once per frame (not used in this script, can be removed)
    void Update()
    {
    }
}
