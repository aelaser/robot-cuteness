using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // Required for UI elements

public class PlayerMovement : MonoBehaviour
{
    public List<List<Vector3>> trajectories; // List of trajectories (each trajectory is a list of waypoints)
    public float speed = 5.0f; // Speed of the NavMeshAgent
    public Button nextTrajectoryButton; // Button to start the next trajectory
    public Button loadSceneButton; // Button to load a new scene
    public GameObject newAgentPrefab;

    private NavMeshAgent agent; // NavMeshAgent variable
    private List<Vector3> currentTrajectory; // The currently active trajectory
    private int currentWaypointIndex = 0; // Index of the current waypoint in the trajectory
    private int currentTrajectoryIndex = 0; // Index of the current trajectory
    private bool isMoving = false; // Flag to control movement

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        agent.speed = speed; // Set the speed of the NavMeshAgent

        InitializeTrajectories(); // Initialize the trajectories
        StartTrajectory(currentTrajectoryIndex); // Start the first trajectory

        nextTrajectoryButton.gameObject.SetActive(false); // Hide the button initially
        loadSceneButton.gameObject.SetActive(false);
        nextTrajectoryButton.onClick.AddListener(StartNextTrajectory); // Add listener for button click
        loadSceneButton.onClick.AddListener(LoadNewScene);
    }

    void Update()
    {
        if (isMoving && currentTrajectory != null && currentWaypointIndex < currentTrajectory.Count)
        {
            MoveAlongTrajectory();
        }
        else if (isMoving)
        {
            isMoving = false;
            Debug.Log("Trajectory completed!");

            // Show the button when the trajectory is completed
            nextTrajectoryButton.gameObject.SetActive(true);
            loadSceneButton.gameObject.SetActive(true);
        }
    }

    // Initialize static trajectories with predefined waypoints
    void InitializeTrajectories()
    {
        trajectories = new List<List<Vector3>>
        {
            new List<Vector3> // Trajectory 1
            {
                new Vector3(0, 0, 0),
                new Vector3(1f, 0, 2f),
                new Vector3(2f, 0, 0),
                new Vector3(3f, 0, 1f)
            },
            new List<Vector3> // Trajectory 2
            {
                new Vector3(0, 0, 0),
                new Vector3(-5f, 0, -5f),
                new Vector3(-10f, 0, 0),
                new Vector3(-15f, 0, -5f)
            }
        };
    }

    // Start a specific trajectory by index
    public void StartTrajectory(int trajectoryIndex)
    {
        if (trajectoryIndex < 0 || trajectoryIndex >= trajectories.Count)
        {
            Debug.LogError("Invalid trajectory index!");
            return;
        }

        currentTrajectory = trajectories[trajectoryIndex];
        currentWaypointIndex = 0;
        isMoving = true;

        if (currentTrajectory.Count > 0)
        {
            nextTrajectoryButton.gameObject.SetActive(false);

            loadSceneButton.gameObject.SetActive(false);// Hide the button when movement starts
            MoveToNextWaypoint(); // Start moving to the first waypoint
        }
    }

    // Start the next trajectory
    void StartNextTrajectory()
    {
        if (currentTrajectoryIndex + 1 < trajectories.Count)
        {
            // Increment the trajectory index
            currentTrajectoryIndex++;

            // Instantly teleport the robot to a fixed starting position 
            TeleportToStartingPosition(new Vector3(3, 0, -4));

            // Start the new trajectory
            StartTrajectory(currentTrajectoryIndex);
        }
        else
        {
            Debug.Log("All trajectories completed!");
            nextTrajectoryButton.gameObject.SetActive(false);

            loadSceneButton.gameObject.SetActive(true);// Hide the button if no more trajectories
        }
    }

    // Teleport the robot to the starting position
    void TeleportToStartingPosition(Vector3 startingPosition)
    {
        agent.enabled = false; // Disable the NavMeshAgent to modify the transform directly
        transform.position = startingPosition; // Instantly set the robot's position
        agent.enabled = true; // Re-enable the NavMeshAgent for subsequent movement
        Debug.Log("Teleported to starting position: " + startingPosition);
    }
    // Move the NavMeshAgent along the current trajectory
    void MoveAlongTrajectory()
    {
        // Check if the agent has reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex < currentTrajectory.Count)
            {
                MoveToNextWaypoint();
            }
        }
    }

    // Set the agent's destination to the next waypoint
    void MoveToNextWaypoint()
    {
        Vector3 nextWaypoint = currentTrajectory[currentWaypointIndex];
        agent.SetDestination(nextWaypoint);
        Debug.Log("Moving to waypoint: " + nextWaypoint);
    }

    void LoadNewScene()
    {
        if (newAgentPrefab != null)
        {
            // Destroy the existing agent
            Destroy(agent.gameObject);

            // Instantiate the new agent prefab at the agent's current position and rotation
            GameObject newAgentInstance = Instantiate(newAgentPrefab, transform.position, transform.rotation);
            //GameObject.Find("DeliveryRobotBody_01_non_cute");//
            agent = newAgentInstance.GetComponent<NavMeshAgent>();
            agent.speed = speed;
            loadSceneButton.gameObject.SetActive(false);

            // Reset robot position 
            TeleportToStartingPosition(new Vector3(3, 0, -4));

            // Reset trajectories 
            currentTrajectoryIndex = 0;


            StartTrajectory(currentTrajectoryIndex); // Start trajectory
        }
    }

    void OnDrawGizmos()
    {
        if (trajectories != null)
        {
            Gizmos.color = Color.green;

            foreach (var trajectory in trajectories)
            {
                for (int i = 0; i < trajectory.Count; i++)
                {
                    // Draw spheres at waypoints
                    Gizmos.DrawSphere(trajectory[i], 0.3f);

                    // Draw lines between waypoints
                    if (i < trajectory.Count - 1)
                    {
                        Gizmos.DrawLine(trajectory[i], trajectory[i + 1]);
                    }
                }
            }
        }

        // Draw the current trajectory in a different color (e.g., red)
        if (currentTrajectory != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < currentTrajectory.Count; i++)
            {
                Gizmos.DrawSphere(currentTrajectory[i], 0.3f);

                if (i < currentTrajectory.Count - 1)
                {
                    Gizmos.DrawLine(currentTrajectory[i], currentTrajectory[i + 1]);
                }
            }
        }
    }
}