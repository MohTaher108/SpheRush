using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{

    // Reference to the main menu scene name for loading it
    public const string menuSceneName = "MainMenu";
    // Reference to the level select scene name for loading it
    public const string levelSelectSceneName = "LevelSelect";
    
    // Reference to the black image that hides the game
    public Image img;
    // Create an animation curve that allows a customized fade in/out
    public AnimationCurve curve;

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
            img.color = new Color(0f, 0f, 0f, newAlpha); // Change the img's alpha (can't change the alpha alone so gotta generate a new color)
            yield return 0; // Skip to next frame
        }
    }

    // Fade to a different scene (using scene's name)
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    // Fade to a different scene (using buildIndex)
    public void FadeTo(int scene)
    {
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
            img.color = new Color(0f, 0f, 0f, newAlpha); // Change the img's alpha (can't change the alpha alone so gotta generate a new color)
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
            img.color = new Color(0f, 0f, 0f, newAlpha); // Change the img's alpha (can't change the alpha alone so gotta generate a new color)
            yield return 0; // Skip to next frame
        }

        SceneManager.LoadScene(scene);
    }
}
