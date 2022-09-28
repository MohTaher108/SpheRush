using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    // Restart Level
    public void Retry()
    {
        SceneFader.instance.FadeTo(SceneManager.GetActiveScene().name);
    }

    // Go to the main menu
    public void Menu()
    {
        SceneFader.instance.FadeTo(SceneFader.menuSceneName);
    }
    
}
