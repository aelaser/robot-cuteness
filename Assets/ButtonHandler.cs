using UnityEngine;
using System.Runtime.InteropServices;

public class ButtonHandler : MonoBehaviour
{
    // Declare the JavaScript function for WebGL interop
   [DllImport("__Internal")]
    private static extern void storeButtonClickData(string argument, string robot, int iteration);

    public PlayerMovement playerMovement; //use it to get the traj index

    // public Button thumbsUpButton; // Reference to the ThumbsUp Button
    // public Button thumbsDownButton; // Reference to the ThumbsDown Button

    // void Start()
    // {
    //     // Initialize the button interactable state
    //     UpdateButtonStates();
    // }


    public void OnThumbsUpButtonClick()
    {
        int robotType = playerMovement.GetActiveMeshIndex(); // Assume you want to store robot type as well
        int traj_idx = playerMovement.GetCurrentTrajectoryIndex();
        Debug.Log("traj index is: " + traj_idx);
        string robot;  // Declare robot as a string
        if (robotType == 1)
            robot = "Control";
        else 
            robot = "Cute";
        Debug.Log("robot index is: " + robotType);
        Debug.Log("Thumbs Up clicked!");
        #if UNITY_WEBGL
        //store button click data, but also store it according to the trajectory index which is a global variable
        storeButtonClickData("ThumbsUp", robot, traj_idx);
        #endif
    }

    public void OnThumbsDownButtonClick()
    {
        int robotType = playerMovement.GetActiveMeshIndex();
        int traj_idx = playerMovement.GetCurrentTrajectoryIndex();
        string robot;  // Declare robot as a string
        if (robotType == 1)
            robot = "Control";
        else 
            robot = "Cute";
        Debug.Log("traj index is: " + traj_idx);
        Debug.Log("robot index is: " + robotType);
        Debug.Log("Thumbs Down clicked!");

        #if UNITY_WEBGL
        storeButtonClickData("ThumbsDown", robot, traj_idx);
        #endif
    }

    
}
