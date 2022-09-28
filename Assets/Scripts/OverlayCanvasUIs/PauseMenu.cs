using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseUI;
    
    // Toggle the pause menu
    public void Toggle()
    {
        // Switch the menu's state
        pauseUI.SetActive(!pauseUI.activeSelf);

        // If we enabled pause menu, freeze game
        if(pauseUI.activeSelf)
        {
            Time.timeScale = 0f;
        } else // If we disabled pause menu, unfreeze game
        {
            Time.timeScale = 1f;
        }
    }

    // Restart the level
    public void Retry()
    {
        Toggle();
        SceneFader.instance.FadeTo(SceneManager.GetActiveScene().name);
    }

    // Go back to main menu
    public void Menu()
    {
        Toggle();
        SceneFader.instance.FadeTo(SceneFader.menuSceneName);
    }

}
