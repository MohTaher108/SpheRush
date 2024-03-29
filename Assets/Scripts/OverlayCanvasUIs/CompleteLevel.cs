using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    
    public bool isSecretLevel = false;

    [HideInInspector]
    public int levelNumber;

    // If the level hasn't been completed before, then increment StarsCount
    void Start()
    {
        int StarsCount = PlayerPrefs.GetInt("StarsCount", 0);
        if(StarsCount < levelNumber)
        {
            PlayerPrefs.SetInt("StarsCount", (StarsCount + 1));
        }
    }

    public void Continue()
    {
        if(isSecretLevel)
        {
            SceneFader.instance.FadeTo(SceneFader.levelSelectSceneName);
            return;
        }

        SceneFader.instance.FadeTo(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Menu()
    {
        SceneFader.instance.FadeTo(SceneFader.menuSceneName);
    }
    
}
