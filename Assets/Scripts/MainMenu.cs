using UnityEngine;
using UnityEngine.UI;
using TMPro; // This is needed for TextMeshPro components
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField emailInputField;  // TextMeshPro Input Field
    public TMP_Text errorMessage;           // TextMeshPro Text for displaying error messages
    public Button nextButton;               // The button that triggers the scene change

    void Start()
    {
        // Clear any existing listeners and add ValidateEmail
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(ValidateEmail);
        errorMessage.text = ""; // Initialize the error message to be empty
    }

    public void ValidateEmail()
    {
        // Check if the entered email is valid
        if (IsValidEmail(emailInputField.text))
        {
            errorMessage.text = ""; // Clear any previous error messages
            PlayGame(); // Load the next scene since the email is valid
        }
        else
        {
            errorMessage.text = "Please enter a valid email address"; 
        }
    }

    public void PlayGame()
    {
        Debug.Log("Attempting to load next scene...");

        SceneManager.LoadSceneAsync(1); // Load the next scene once next is clicked
    }

    private bool IsValidEmail(string email)
    {
        // Regex pattern to validate the email format
        return Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
    }
}
