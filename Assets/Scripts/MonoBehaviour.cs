using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow, typically your player or robot
    public Vector3 offset = new Vector3(0, 3, -5); // Offset to position the camera in front of the player
    public float followSpeed = 5.0f; // Speed at which the camera follows the target

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position with offset in front of the player
            Vector3 desiredPosition = target.position + target.forward * offset.z + target.up * offset.y;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // Keep the camera facing the target
            transform.LookAt(target.position + Vector3.up * 1.5f); // Adjust height if needed
        }
    }
}
