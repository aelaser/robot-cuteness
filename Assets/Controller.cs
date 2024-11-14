using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public GameObject robotPrefab1;
    public GameObject robotPrefab2;

    private GameObject activeRobot;       // Holds the active robot instance
    private NavMeshAgent navMeshAgent;    // NavMeshAgent for controlling movement

    public Transform targetDestination;   // Assign a target in the Inspector or dynamically set it

    void Start()
    {
        // Randomly choose between the two prefabs
        GameObject selectedPrefab = Random.Range(0, 2) == 0 ? robotPrefab1 : robotPrefab2;

        // Instantiate the chosen prefab as a child of this GameObject
        activeRobot = Instantiate(selectedPrefab, transform.position, transform.rotation, transform);

        // Add a NavMeshAgent component if the prefab doesn't already have one
        navMeshAgent = activeRobot.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            navMeshAgent = activeRobot.AddComponent<NavMeshAgent>();
        }
    }


    void Update()
    {
        // Check if the agent has reached its destination
        if (navMeshAgent != null && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            // You can add behavior here for when the robot reaches its destination
            Debug.Log("Robot has reached its destination.");
        }
    }
}
