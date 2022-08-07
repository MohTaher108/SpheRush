using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public SceneFader sceneFader;

    public GameObject HowToPlayUI;

    public void Play()
    {
        sceneFader.FadeTo(SceneFader.levelSelectSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        HowToPlayUI.SetActive(true);
    }

    public void Reset()
    {
        PlayerPrefs.SetInt("StarsCount", 0);
    }

}
