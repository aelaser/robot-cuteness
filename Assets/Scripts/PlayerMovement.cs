using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public Transform target; // Target to move towards
    public float speed = 5.0f; // Speed of the NavMeshAgent

    private NavMeshAgent agent; // NavMeshAgent variable

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        agent.speed = speed; // Set the speed of the NavMeshAgent
    }

    void Update()
    {
        if (target != null)
        {
            agent.destination = target.position; // Set the destination of the NavMeshAgent to the target's position
            Debug.Log("Target Position: " + target.position + ", Agent Position: " + agent.transform.position);
        }
        else
        {
            Debug.Log("No target assigned.");
        }
    }

}
