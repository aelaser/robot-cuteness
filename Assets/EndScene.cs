
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices; // For calling JS functions
using System; // For generating GUID


public class EndScene : MonoBehaviour
{
    public TMP_Text uniqueIdText; // Reference to a TextMeshPro Text component to display the ID
    public Button copyButton; 

    public string uniqueId;

    // Declare the JavaScript function for WebGL interop
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string uniqueId);


    void Start()
    {
        uniqueId = PlayerPrefs.GetString("UniqueId", "ID Not Found"); // Retrieve the stored unique ID

        // Display the unique ID in the TextMeshPro component
        if (uniqueIdText != null)
        {
            uniqueIdText.text =  uniqueId;
        }
        copyButton.onClick.AddListener(CopyUniqueIdToClipboard);
    }

    // Method to select and copy the unique ID to clipboard
    public void CopyUniqueIdToClipboard()
    {
        // Call the JavaScript function to copy the ID to clipboard
        #if UNITY_WEBGL
        CopyToClipboard(uniqueId); // This will invoke the JS function
        #endif
        Debug.Log("Unique ID copied to clipboard: " + uniqueId);
    }
}

