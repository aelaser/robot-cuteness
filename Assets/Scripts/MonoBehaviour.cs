using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow, typically your player or robot
    public Vector3 offset = new Vector3(0, 3, -5); // Additional offset for fine-tuning
    public float followSpeed = 5.0f; // Speed at which the camera follows the target
    private Vector3 initialPositionRelativeToTarget; // Initial position difference between camera and target

    void Start()
    {
        if (target != null)
        {
            // Store the initial position relative to the target
            initialPositionRelativeToTarget = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position based on the initial setup and additional offset
            Vector3 desiredPosition = target.position + initialPositionRelativeToTarget + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // Keep the camera facing the target
            transform.LookAt(target.position + Vector3.up * 1.5f); // Adjust height if needed
        }
    }
}
