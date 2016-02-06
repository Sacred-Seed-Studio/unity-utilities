/**********************************************************************
 * Sacred Seed Studio
 * Android Share Intent
 * v0.1.0
 *
 * Allows you to share Text or Images using Android's share intent API.
 * In Player Settings you must set Write Access to External (SD Card).
 *
 * Based on
 * - http://www.daniel4d.com/blog/sharing-image-unity-android/
 * - https://www.youtube.com/watch?v=bEkmk2cxVLc
 *
 * Created: January 5 2016
 * Modified: February 5 2016
 *
 * The only rules:
 * 1. If you modify something, verify its documentation is still valid.
 * 2. Write some tests.
 * 3. Update the Modified date.
 *********************************************************************/

using System.Collections;
using UnityEngine;

public class AndroidShareIntent : MonoBehaviour
{
    AndroidJavaClass IntentClass;
    AndroidJavaClass UnityPlayer;

    AndroidJavaObject intentObject;
    AndroidJavaObject currentActivity;
    AndroidJavaObject shareChooser;

    bool isInitialized = false;
    bool screenshotProcessing = false;

    string androidClass = "android.content.Intent";

    public static AndroidShareIntent instance;

    #region Setup
    // Make sure we only have on instance of this class running and statically accessible
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) { Destroy(gameObject); }
    }

    // Setup the Android Java Classes/Objects
    void Initialize()
    {
        if (isInitialized) { return; }

        isInitialized = true;
        IntentClass = new AndroidJavaClass(androidClass);
        SetIntentObject();
        UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }
    #endregion

    #region Public API
    /// <summary>
    /// Share a textual message with installed Apps
    /// </summary>
    /// <param name="message">Text message to send</param>
    /// <param name="subject">Optional: Subject of message (used for email)</param>
    public void ShareText(string message, string subject = "$#!@empty")
    {
#if UNITY_ANDROID
        if (!isInitialized) Initialize();

        intentObject.Call<AndroidJavaObject>("setAction", IntentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("putExtra", IntentClass.GetStatic<string>("EXTRA_TEXT"), message);
        intentObject.Call<AndroidJavaObject>("putExtra", IntentClass.GetStatic<string>("EXTRA_SUBJECT"), (subject == "$#!@empty") ? "" : subject);
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        shareChooser = IntentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
        currentActivity.Call("startActivity", shareChooser);
        SetIntentObject();
#endif
    }

    /// <summary>
    /// Share a screenshot that is taken on next complete frame
    /// </summary>
    /// <param name="message">Text message to send</param>
    /// <param name="subject">Optional: Subject of message (used for email)</param>
    public void ShareScreenshot(string message, string subject = "$#!@empty")
    {
        if (!screenshotProcessing) { StartCoroutine(CreateAndShareScreenshot(message, subject)); }
        else { Debug.Log("Screenshot already going"); } //TODO maybe add more to this?
    }
    #endregion

    #region Helpers
    // Takes the screenshot and calls the Android API's
    IEnumerator CreateAndShareScreenshot(string message, string subject)
    {
#if UNITY_ANDROID
        if (!isInitialized) { Initialize(); }

        screenshotProcessing = true;
        // Finish rendering the frame, make a texture from the current screen, save the screenshot
        yield return new WaitForEndOfFrame();
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();
        byte[] dataToSave = screenTexture.EncodeToPNG();
        string destination = Path.Combine(Application.persistentDataPath, Application.productName + System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        File.WriteAllBytes(destination, dataToSave);

        // Android API Calls for share intent
        intentObject.Call<AndroidJavaObject>("setAction", IntentClass.GetStatic<string>("ACTION_SEND"));
        // Load Image
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
        intentObject.Call<AndroidJavaObject>("putExtra", IntentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        // Add Message/Subject
        intentObject.Call<AndroidJavaObject>("putExtra", IntentClass.GetStatic<string>("EXTRA_TEXT"), message);
        intentObject.Call<AndroidJavaObject>("putExtra", IntentClass.GetStatic<string>("EXTRA_SUBJECT"), (subject == "$#!@empty") ? "" : subject);
        intentObject.Call<AndroidJavaObject>("setType", "image/png");
        shareChooser = IntentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
        currentActivity.Call("startActivity", shareChooser);
        SetIntentObject();
#endif

        screenshotProcessing = false;
        yield return null;
    }

    // Resets the share intent so messages don't carry over
    void SetIntentObject()
    {
        intentObject = new AndroidJavaObject(androidClass);
    }
    #endregion
}
