using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.UI; // Required for UI elements

public class NavMeshAgentWithMeshes : MonoBehaviour
{
    public GameObject[] meshOptions;   // Array to hold child GameObjects with different meshes
    private int activeMeshIndex = -1;  // Tracks the currently active mesh index
    public Button loadSceneButton; // Button to load a new scene
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        // Get the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("No NavMeshAgent component found on this GameObject.");
            return;
        }

        // Check if mesh options are assigned
        if (meshOptions.Length == 0)
        {
            Debug.LogError("No mesh options assigned in the Inspector.");
            return;
        }

        // Randomly select a starting mesh
        activeMeshIndex = Random.Range(0, meshOptions.Length);
        SetActiveMesh(activeMeshIndex);
        loadSceneButton.onClick.AddListener(SwitchToNextMesh);
    }

    // Method to set one mesh active and disable others
    void SetActiveMesh(int index)
    {
        for (int i = 0; i < meshOptions.Length; i++)
        {
            meshOptions[i].SetActive(i == index);
        }
    }

    // Method to switch to the next mesh in the array
    public void SwitchToNextMesh()
    {
        // Increment the index and wrap around if needed
        activeMeshIndex = (activeMeshIndex + 1) % meshOptions.Length;
        SetActiveMesh(activeMeshIndex);
    }
}
