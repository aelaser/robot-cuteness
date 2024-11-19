// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using UnityEngine.SceneManagement;
// using System.Text.RegularExpressions;
// using System.Runtime.InteropServices; // For calling JS functions

// public class MainMenu : MonoBehaviour
// {
//     public TMP_InputField emailInputField;  // TextMeshPro Input Field
//     public TMP_Text errorMessage;           // TextMeshPro Text for displaying error messages
//     public Button nextButton;               // Button that triggers the scene change

//     // Declare the JavaScript function for WebGL interop
//     [DllImport("__Internal")]
//     private static extern void storeEmailData(string email);

//     void Start()
//     {
//         nextButton.onClick.AddListener(ValidateEmail);
//         errorMessage.text = ""; 
//     }

//     public void ValidateEmail()
//     {
//         if (IsValidEmail(emailInputField.text))
//         {
//             string email = emailInputField.text;
//             SendEmailToFb(email); // Send valid email to Firebase
//             errorMessage.text = ""; // Clear any previous error messages
//             PlayGame(); // Load the next scene since the email is valid
//         }
//         else
//         {
//             errorMessage.text = "Please enter a valid email address"; // Show error message
//         }
//     }

//     private void SendEmailToFb(string email)
//     {
//         Debug.Log("Valid Email: " + email); // Log the valid email
//         #if UNITY_WEBGL
//         storeEmailData(email); // Call the JS function to store the email in Firebase
//         #endif
//     }

//     public void PlayGame()
//     {
//         SceneManager.LoadSceneAsync(1); // Load the next scene asynchronously
//     }

//     bool IsValidEmail(string email)
//     {
//         // Use a more inclusive regex pattern to validate the email format
//         return Regex.IsMatch(email,
//             @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
//     }
// }


using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices; // For calling JS functions
using System; // For generating GUID

public class MainMenu : MonoBehaviour
{
    public Button nextButton;               // Button that triggers the scene change
    public TMP_Text statusMessage;          // Optional TextMeshPro Text for showing status messages

    public string uniqueId;

    // Declare the JavaScript function for WebGL interop
    [DllImport("__Internal")]
    private static extern void storeEmailData(string uniqueId);

    void Start()
    {
        nextButton.onClick.AddListener(AssignUniqueIdAndProceed);
        if (statusMessage != null)
            statusMessage.text = ""; // Clear any previous status messages
    }

    public void AssignUniqueIdAndProceed()
    {
        // Generate a unique ID (using GUID)
        uniqueId = Guid.NewGuid().ToString("N"); // GUID without hyphens
        uniqueId = uniqueId.Substring(0, 8); // Take the first 8 characters (adjust
        Debug.Log("Generated Unique ID: " + uniqueId);

        PlayerPrefs.SetString("UniqueId", uniqueId); // Store unique ID in PlayerPrefs
        PlayerPrefs.Save(); // Save the PlayerPrefs data

        SendUniqueIdToFirebase(uniqueId); // Send the ID to Firebase
        PlayGame(); // Load the next scene
    }

    private void SendUniqueIdToFirebase(string uniqueId)
    {
        #if UNITY_WEBGL
        storeEmailData(uniqueId); // Call the JS function to store the unique ID in Firebase
        #endif
        Debug.Log("Unique ID sent to Firebase: " + uniqueId);
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1); // Load the next scene asynchronously
    }
}
