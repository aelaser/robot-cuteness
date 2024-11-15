using UnityEngine;
using System.Runtime.InteropServices;

public class ButtonHandler : MonoBehaviour
{
    // Declare the JavaScript function for WebGL interop
   [DllImport("__Internal")]
private static extern void storeButtonClickData(string argument);

    public void OnThumbsUpButtonClick()
    {
        Debug.Log("Thumbs Up clicked!");
        #if UNITY_WEBGL
        storeButtonClickData("ThumbsUp");
        #endif
    }

    public void OnThumbsDownButtonClick()
    {
        Debug.Log("Thumbs Down clicked!");
        #if UNITY_WEBGL
        storeButtonClickData("ThumbsDown");
        #endif
    }
}
