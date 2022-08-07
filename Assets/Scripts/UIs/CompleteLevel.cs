using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    public SceneFader sceneFader;

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
        sceneFader.FadeTo(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Menu()
    {
        sceneFader.FadeTo(SceneFader.menuSceneName);
    }

    public void levelSelect()
    {
        sceneFader.FadeTo(SceneFader.levelSelectSceneName);
    }
    
}
