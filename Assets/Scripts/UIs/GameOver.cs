using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public SceneFader sceneFader;

    // Restart Level
    public void Retry()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    // Go to the main menu
    public void Menu()
    {
        sceneFader.FadeTo(SceneFader.menuSceneName);
    }
    
}
