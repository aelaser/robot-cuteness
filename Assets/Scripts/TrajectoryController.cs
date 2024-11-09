// using System.Collections.Generic;
// using UnityEngine;

// public class TrajectoryController : MonoBehaviour
// {
//     public Transform robot; // The robot to move
//     public List<List<Vector3>> trajectories; // All trajectories (list of waypoint lists)
//     public float speed = 2.0f;

//     private List<Vector3> currentTrajectory; // The trajectory the robot is following
//     private int currentWaypointIndex = 0;
//     private bool isMoving = false;

//     void Start()
//     {
//         // Initialize trajectories with static values
//         InitializeTrajectories();
//         OnDrawGizmos();
//         // Start the first trajectory as an example
//         StartTrajectory(0); // Index 0 for the first trajectory
//     }

//     void InitializeTrajectories()
//     {
//         trajectories = new List<List<Vector3>>
//         {
//             new List<Vector3> // Trajectory 1
//             {
//                 new Vector3(0, 0, 0),
//                 new Vector3(1, 0, 1),
//                 new Vector3(2, 0, 2),
//                 new Vector3(3, 0, 3)
//             },
//             new List<Vector3> // Trajectory 2
//             {
//                 new Vector3(0, 0, 0),
//                 new Vector3(-1, 0, -1),
//                 new Vector3(-2, 0, -2),
//                 new Vector3(-3, 0, -3)
//             }
//         };
//     }

//     public void StartTrajectory(int trajectoryIndex)
//     {
//         if (trajectoryIndex < trajectories.Count)
//         {
//             currentTrajectory = trajectories[trajectoryIndex];
//             currentWaypointIndex = 0;
//             isMoving = true;
//             robot.position = currentTrajectory[0]; // Move to the starting point
//         }
//         else
//         {
//             Debug.LogWarning("Invalid trajectory index");
//         }
//     }

//     void Update()
//     {
//         if (isMoving && currentTrajectory != null)
//         {
//             MoveAlongTrajectory();
//         }
//     }

//     void MoveAlongTrajectory()
//     {
//         if (currentWaypointIndex < currentTrajectory.Count)
//         {
//             Vector3 target = currentTrajectory[currentWaypointIndex];
//             robot.position = Vector3.MoveTowards(robot.position, target, speed * Time.deltaTime);

//             if (Vector3.Distance(robot.position, target) < 0.1f)
//             {
//                 currentWaypointIndex++;
//             }
//         }
//         else
//         {
//             isMoving = false; // Trajectory completed
//             Debug.Log("Trajectory Completed");
//         }
//     }

//     void OnDrawGizmos()
//     {   
//         if (currentTrajectory != null)
//         {
//             Gizmos.color = Color.red;
//             foreach (Vector3 point in currentTrajectory)
//             {
//                 Gizmos.DrawSphere(point, 0.2f); // Draw a sphere at each waypoint
//             }
//         }
//     }

// }
