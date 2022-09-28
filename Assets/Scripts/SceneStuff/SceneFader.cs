using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    // Singleton
    public static SceneFader instance;

    public const string menuSceneName = "MainMenu";
    public const string levelSelectSceneName = "LevelSelect";
    public const string secretLevelSceneName = "SecretLevel";
    
    // Reference to the black image that hides the game
    public Image fadeImage;
    // Create an animation curve that allows a customized fade in/out
    public AnimationCurve curve;

    void Awake()
    {
        // Print an error if a second BuildManager is ever made accidentally
        if(instance != null)
        {
            Debug.LogError("More than one SceneFader in scene!");
            return;
        }

        instance = this;
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    // Fade in the scene
    IEnumerator FadeIn()
    {
        // Go from the end of the curve to the beginning
        float currentTime = 1f;
        while(currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            float newAlpha = curve.Evaluate(currentTime);
            fadeImage.color = new Color(0f, 0f, 0f, newAlpha); // Change the fadeImage's alpha (can't change the alpha alone so gotta generate a new color)
            yield return 0; // Skip to next frame
        }
    }

    // Fade to a different scene (using scene's name)
    public void FadeTo(string scene)
    {
        AudioManager.instance.Play("LevelSelect");
        StartCoroutine(FadeOut(scene));
    }

    // Fade to a different scene (using buildIndex)
    public void FadeTo(int scene)
    {
        AudioManager.instance.Play("LevelSelect");
        StartCoroutine(FadeOut(scene));
    }

    // Fade out the scene and load the next scene (using scene's name)
    IEnumerator FadeOut(string scene)
    {
        // Go from the beginning of the curve to the end
        float currentTime = 0f;
        while(currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            float newAlpha = curve.Evaluate(currentTime);
            fadeImage.color = new Color(0f, 0f, 0f, newAlpha); // Change the fadeImage's alpha (can't change the alpha alone so gotta generate a new color)
            yield return 0; // Skip to next frame
        }

        SceneManager.LoadScene(scene);
    }


    // Fade out the scene and load the next scene (using buildIndex)
    IEnumerator FadeOut(int scene)
    {
        // Go from the beginning of the curve to the end
        float currentTime = 0f;
        while(currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            float newAlpha = curve.Evaluate(currentTime);
            fadeImage.color = new Color(0f, 0f, 0f, newAlpha); // Change the fadeImage's alpha (can't change the alpha alone so gotta generate a new color)
            yield return 0; // Skip to next frame
        }

        SceneManager.LoadScene(scene);
    }
}
