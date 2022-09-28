using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public HowToPlay HowToPlayUIScript;

    public void Play()
    {
        SceneFader.instance.FadeTo(SceneFader.levelSelectSceneName);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        HowToPlayUIScript.Activate();
    }

    public void Reset()
    {
        PlayerPrefs.SetInt("StarsCount", 0);
    }

}
