using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField emailInputField;  // TextMeshPro Input Field
    public TMP_Text errorMessage;           // TextMeshPro Text for displaying error messages
    public Button nextButton;               // Button that triggers the scene change

    void Start()
    {
        nextButton.onClick.AddListener(ValidateEmail);
        errorMessage.text = ""; 
    }

    public void ValidateEmail()
    {
        if (IsValidEmail(emailInputField.text))
        {
            PrintValidEmail(); // Print the email only if it's valid
            errorMessage.text = ""; // Clear any previous error messages
            PlayGame(); // Load the next scene since the email is valid
        }
        else
        {
            errorMessage.text = "Please enter a valid email address"; // Show error message
        }
    }

    private void PrintValidEmail()
    {
        Debug.Log("Valid Email: " + emailInputField.text); // Only log valid emails
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1); // Load the next scene asynchronously
    }

    bool IsValidEmail(string email)
    {
        // Use a more inclusive regex pattern to validate the email format
        return Regex.IsMatch(email,
            @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
    }
}
