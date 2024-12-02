using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // Required for UI elements
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public List<List<Vector3>> trajectories; // List of trajectories (each trajectory is a list of waypoints)
    public float speed = 5.0f; // Speed of the NavMeshAgent
    public Button nextTrajectoryButton; // Button to start the next trajectory
    public Button loadSceneButton; // Button to load a new scene + switch robot

    public Button thumbsUpButton;

    public Button thumbsDownButton;

    public CameraFollow cameraFollowScript; // Reference to the camera follow script
    public bool swapped = false;
    private bool isExperimentEnded = false; // Track if experiment should end

    private NavMeshAgent agent; // NavMeshAgent variable
    private List<Vector3> currentTrajectory; // The currently active trajectory
    private int currentWaypointIndex = 0; // Index of the current waypoint in the trajectory
    private int currentTrajectoryIndex = 0; // Index of the current trajectory
    private bool isMoving = false; // Flag to control movement
    private Vector3 startPos = new Vector3(6, 0, -6);
    private int prefabIndex;

    public GameObject[] meshOptions;   // Array to hold child GameObjects with different meshes
    private int activeMeshIndex;

    public GameObject overlayCanvas;
    public Button nextButton; // Button inside the overlay Canvas to hide it
    private List<int> randomizedIndices; // Randomized order of trajectory indices

    void Start()
    {
        activeMeshIndex = Random.Range(0, 2);
        SetActiveMesh(activeMeshIndex);
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        agent.speed = speed; // Set the speed of the NavMeshAgent

        InitializeTrajectories(); // Initialize the trajectories
        StartTrajectory(randomizedIndices[currentTrajectoryIndex]); // Start the first trajectory

        nextTrajectoryButton.gameObject.SetActive(false); // Hide the button initially
        loadSceneButton.gameObject.SetActive(false);
        nextTrajectoryButton.onClick.AddListener(StartNextTrajectory); // Add listener for button click
        // loadSceneButton.onClick.AddListener(() => SceneManager.LoadSceneAsync(3));
        // SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneButton.onClick.AddListener(ShowOverlay);

        // Overlay Canvas setup
        if (overlayCanvas != null)
        {
            overlayCanvas.SetActive(false); // Ensure Canvas is hidden at the start
            nextButton.onClick.AddListener(HideOverlay); // Add listener for "Next" button
        }
    }
    // Method to set one mesh active and disable others
    void SetActiveMesh(int index)
    {
        Debug.Log(index);
        for (int i = 0; i < meshOptions.Length; i++)
        {
            meshOptions[i].SetActive(i == index);
        }
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

            thumbsDownButton.gameObject.SetActive(false);
            thumbsUpButton.gameObject.SetActive(false);
        }
    }


    // Start a specific trajectory by index
    public void StartTrajectory(int trajectoryIndex)
    {
        if (trajectoryIndex < 0 || trajectoryIndex >= trajectories.Count)
        {
            Debug.LogError("Invalid trajectory index!");
            return;
        }
        for (int i = 0; i < randomizedIndices.Count; i++) {
            Debug.Log(randomizedIndices[i]);
        }
        
        currentTrajectory = trajectories[trajectoryIndex];
        currentWaypointIndex = 0;
        isMoving = true;

        if (currentTrajectory.Count > 0)
        {
            nextTrajectoryButton.gameObject.SetActive(false);

            loadSceneButton.gameObject.SetActive(false);// Hide the button when movement starts
            thumbsDownButton.gameObject.SetActive(true);
            thumbsUpButton.gameObject.SetActive(true);
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
            int nextIndex = randomizedIndices[currentTrajectoryIndex];
            

            // Instantly teleport the robot to a fixed starting position 
            TeleportToStartingPosition(startPos, transform);

            // Start the new trajectory
            StartTrajectory(nextIndex);
        }
        else
        {
            Debug.Log("All trajectories completed!");
            nextTrajectoryButton.gameObject.SetActive(false);
            
            loadSceneButton.gameObject.SetActive(true);// Hide the button if no more trajectories

            thumbsDownButton.gameObject.SetActive(false);
            thumbsUpButton.gameObject.SetActive(false);

        }
    }

    // Teleport the robot to the starting position
    void TeleportToStartingPosition(Vector3 startingPosition, Transform newTransform)
    {
        agent.enabled = false; // Disable the NavMeshAgent to modify the transform directly
        newTransform.position = startingPosition; // Instantly set the robot's position
        agent.enabled = true; // Re-enable the NavMeshAgent for subsequent movement
        Debug.Log("Teleported to starting position: " + startingPosition);
    }
    // Move the NavMeshAgent along the current trajectory
    void MoveAlongTrajectory()
    {
        float distanceToWaypoint = Vector3.Distance(agent.transform.position, currentTrajectory[currentWaypointIndex]);
        // Debug.Log(distanceToWaypoint);
            // Check if the agent has reached the current waypoint
        if (!agent.pathPending && distanceToWaypoint < 0.1f)
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

    public void LoadNewRobot()
    {
        if (isExperimentEnded)
        {
            Debug.Log("Experiment Ended!");
            // Add your end experiment logic here
            SceneManager.LoadSceneAsync(2);
            // Application.Quit(); // Example: quit the application
            return;
        }
        // Increment the index and wrap around if needed
        activeMeshIndex = (activeMeshIndex + 1) % meshOptions.Length;
        SetActiveMesh(activeMeshIndex);
        
        
        agent.speed = speed;
        swapped = true;
        loadSceneButton.gameObject.SetActive(false);

        // Reset robot position 
        TeleportToStartingPosition(startPos, transform);

        // Reset trajectories 
        InitializeTrajectories();
        currentTrajectoryIndex = 0;

        StartTrajectory(randomizedIndices[currentTrajectoryIndex]); // Start trajectory

        // Update button for "End Experiment" functionality
        loadSceneButton.GetComponentInChildren<TextMeshProUGUI>().text = "End Study";
        loadSceneButton.onClick.RemoveAllListeners();
        loadSceneButton.onClick.AddListener(() => { isExperimentEnded = true; LoadNewRobot(); });
        loadSceneButton.gameObject.SetActive(false);
    }
    //for encapsulation lol
      public int GetCurrentTrajectoryIndex()
    {
        return randomizedIndices[currentTrajectoryIndex];
    }

    public int GetActiveMeshIndex()
    {
        return activeMeshIndex;
    }
   
    
    // Initialize static trajectories with predefined waypoints
    void InitializeTrajectories()
    {
        trajectories = new List<List<Vector3>>
        {
            new List<Vector3> // Trajectory 1
            {
                new Vector3(0, 0.05f, 0),
                new Vector3(1f, 0.05f, 2f),
                new Vector3(6f, 0.05f, 2),
                new Vector3(3f, 0.05f, 0f)
            },
            new List<Vector3> // Trajectory 2
            {
                new Vector3(0, 0.05f, 0),
                new Vector3(-5.55f, 0.05f, -5.76f),
                new Vector3(-5.83f, 0.05f, 0.33f),
                new Vector3(-3.99f, 0.05f, 5.35f)
            },
            new List<Vector3> // Trajectory 3
            {
                new Vector3(0, 0.05f, 0),
                new Vector3(1f,  0.05f, 2f),
                new Vector3(2f,  0.05f, 0),
                new Vector3(3.43f,  0.05f, 5.88f)
            },
            new List<Vector3> // Trajectory 4
            {
                new Vector3(0, 0.05f, 0),
                new Vector3(-5.55f, 0.05f, -5.76f),
                new Vector3(-5.83f, 0.05f, 0.33f),
                new Vector3(0.24f, 0.05f, 3.36f),
                new Vector3(-.85f, 0.05f, -0.34f)
            },
            new List<Vector3> // Trajectory 5
            {
                new Vector3(2.77f, 0.05f, -6f),
                new Vector3(6.25f, 0.05f, -2.16f),
                new Vector3(6.25f, 0.05f, 2.16f),
                new Vector3(-3.62f, 0.05f, 5.62f)
            }
        };

        // Create and shuffle the index list
        randomizedIndices = new List<int>();
        for (int i = 0; i < trajectories.Count; i++)
        {
            randomizedIndices.Add(i);
        }
        ShuffleList(randomizedIndices);
    }

    // Show the overlay Canvas
    void ShowOverlay()
    {
        overlayCanvas.SetActive(true);
        Time.timeScale = 0; // Optional: Pause the game
    }

    // Hide the overlay Canvas
    void HideOverlay()
    {
        overlayCanvas.SetActive(false);
        LoadNewRobot();
        Time.timeScale = 1; // Resume the game
    }

    void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}