using UnityEngine;
using System.Collections;
 
public class CameraFade : MonoBehaviour
{    
    // ---------------------------------------- 
    // 	PUBLIC FIELDS
    // ----------------------------------------
 
    // Alpha start value
    public float startAlpha = 1;
 
    // Texture used for fading
    public Texture2D fadeTexture;
 
    // Default time a fade takes in seconds
    public float fadeDuration = 2;
 
    // Depth of the gui element
    public int guiDepth = -1000;
 
    // Fade into scene at start
    public bool fadeIntoScene = true;
 
    // ---------------------------------------- 
    // 	PRIVATE FIELDS
    // ----------------------------------------
 
    // Current alpha of the texture
    private float currentAlpha = 1;
 
    // Current duration of the fade
    private float currentDuration;
 
    // Direction of the fade
    private int fadeDirection = -1;
 
    // Fade alpha to
    private float targetAlpha = 0;
 
    // Alpha difference
    private float alphaDifference = 0;
 
    // Style for background tiling
    private GUIStyle backgroundStyle = new GUIStyle();
    private Texture2D dummyTex;
 
    // Color object for alpha setting
    Color alphaColor = new Color();
 
    // ---------------------------------------- 
    // 	FADE METHODS
    // ----------------------------------------
 
    public void FadeIn(float duration, float to)
    {
        // Set fade duration
        currentDuration = duration;
        // Set target alpha
        targetAlpha = to;
        // Difference
        alphaDifference = Mathf.Clamp01(currentAlpha - targetAlpha);
        // Set direction to Fade in
        fadeDirection = -1;
    }
 
    public void FadeIn()
    {
        FadeIn(fadeDuration, 0);
    }
 
    public void FadeIn(float duration)
    {
        FadeIn(duration, 0);
    }
 
    public void FadeOut(float duration, float to)
    {
        // Set fade duration
        currentDuration = duration;
        // Set target alpha
        targetAlpha = to;
        // Difference
        alphaDifference = Mathf.Clamp01(targetAlpha - currentAlpha);
        // Set direction to fade out
        fadeDirection = 1;
    }
 
    public void FadeOut()
    {
        FadeOut(fadeDuration, 1);
    }    
 
    public void FadeOut(float duration)
    {
        FadeOut(duration, 1);
    }
 
    // ---------------------------------------- 
    // 	STATIC FADING FOR MAIN CAMERA
    // ----------------------------------------
 
    public static void FadeInMain(float duration, float to)
    {
        GetInstance().FadeIn(duration, to);
    }
 
    public static void FadeInMain()
    {
        GetInstance().FadeIn();
    }
 
    public static void FadeInMain(float duration)
    {
        GetInstance().FadeIn(duration);
    }
 
    public static void FadeOutMain(float duration, float to)
    {
        GetInstance().FadeOut(duration, to);
    }
 
    public static void FadeOutMain()
    {
        GetInstance().FadeOut();
    }
 
    public static void FadeOutMain(float duration)
    {
        GetInstance().FadeOut(duration);
    }
 
    // Get script fom Camera
    public static CameraFade GetInstance()
    {
    	// Get Script
        CameraFade fader = (CameraFade)Camera.main.GetComponent("CameraFade");
        // Check if script exists
        if (fader == null) 
        {
            Debug.LogError("No CameraFade attached to the main camera.");
        }    
        return fader;
	}
 
    // ---------------------------------------- 
    // 	SCENE FADEIN
    // ----------------------------------------
 
    public void Start()
    {
    	Debug.Log("Starting CameraFade");
 
        dummyTex = new Texture2D(1,1);
        dummyTex.SetPixel(0,0,Color.clear);
        backgroundStyle.normal.background = fadeTexture;
        currentAlpha = startAlpha;
        if (fadeIntoScene)
        {
            FadeIn();
        }
    }
 
    // ---------------------------------------- 
    // 	FADING METHOD
    // ----------------------------------------
 
    public void OnGUI()
    {   
        // Fade alpha if active
        if ((fadeDirection == -1 && currentAlpha > targetAlpha) ||
            (fadeDirection == 1 && currentAlpha < targetAlpha))
        {
            // Advance fade by fraction of full fade time
            currentAlpha += (fadeDirection * alphaDifference) * (Time.deltaTime / currentDuration);
            // Clamp to 0-1
            currentAlpha = Mathf.Clamp01(currentAlpha);
        }
 
        // Draw only if not transculent
        if (currentAlpha > 0)
        {
            // Draw texture at depth
            alphaColor.a = currentAlpha;
            GUI.color = alphaColor;
            GUI.depth = guiDepth;
            GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTex, backgroundStyle);
        }
    }
}
