using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    // Restart Level
    public void Retry()
    {
        AudioManager.instance.Play("LevelSelect");
        SceneFader.instance.FadeTo(SceneManager.GetActiveScene().name);
    }

    // Go to the main menu
    public void Menu()
    {
        AudioManager.instance.Play("LevelSelect");
        SceneFader.instance.FadeTo(SceneFader.menuSceneName);
    }
    
}
