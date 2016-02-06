/**********************************************************************
 * Sacred Seed Studio
 * Title Controller
 * v0.1.0
 *
 * Fades splash screen(s) in and out and then fades the menu canvas in.
 *
 * Created: February 5 2016
 * Modified: February 5 2016
 *********************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [Header("Canvases")]
    public GameObject SplashCanvas;
    public GameObject MenuCanvas;
    [Header("Splash Variables")]
    public float incrementDelay = 0.0005f;
    public float fullSplashTime = 0.5f; // Seconds to show splash image full opacity for
    [Header("Images")]
    public Image[] splashImages;
    [Header("Skip Spash Config")]
    public bool allowSplashSkipping = true;
    public string skipSplashButton = "Submit";

    CanvasGroup menuCanvasGroup;

    Color white = Color.white;
    Color transparent = Color.white;

    bool skipSplash = false;

    void Start()
    {
        Debug.Assert(SplashCanvas != null && MenuCanvas != null, "Canvases not added");
        Debug.Assert(splashImages.Length > 0, "Splash logo(s) not added");
        transparent.a = 0f;
        menuCanvasGroup = MenuCanvas.GetComponent<CanvasGroup>();
        menuCanvasGroup.alpha = 0f;
        foreach (Image i in splashImages) { i.color = transparent; }
        ShowCanvases(true, false);
        StartCoroutine("Splash");
    }

    void Update()
    {
        if (allowSplashSkipping && Input.GetButtonDown(skipSplashButton))
        {
            skipSplash = true;
        }
    }

    void ShowCanvases(bool splash, bool menu)
    {
        SplashCanvas.SetActive(splash);
        MenuCanvas.SetActive(menu);
    }

    IEnumerator Splash()
    {
        float increment;
        float percentage;

        // Fade each splash image in/out
        foreach (Image i in splashImages)
        {
            increment = 0.01f;
            percentage = 0f;
            while (i.color != white && !skipSplash)
            {
                i.color = Color.Lerp(transparent, white, percentage);
                percentage += increment;
                yield return new WaitForSeconds(incrementDelay);
            }

            if (skipSplash) { break; }
            yield return new WaitForSeconds(fullSplashTime);

            percentage = 0f;
            increment = 0.02f;
            while (i.color != transparent && !skipSplash)
            {
                i.color = Color.Lerp(white, transparent, percentage);
                percentage += increment;
                yield return new WaitForSeconds(incrementDelay);
            }
        }

        // Open menu
        ShowCanvases(false, true);

        // Fade menu in
        increment = 0.01f;
        percentage = 0f;
        while (menuCanvasGroup.alpha != 1f)
        {
            menuCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, percentage);
            percentage += increment;
            yield return new WaitForSeconds(incrementDelay);
        }
    }
}
