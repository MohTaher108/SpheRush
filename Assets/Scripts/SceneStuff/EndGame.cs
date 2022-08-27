using UnityEngine;

public class EndGame : MonoBehaviour
{
    
    public void Menu()
    {
        AudioManager.instance.Play("LevelSelect");
        SceneFader.instance.FadeTo(SceneFader.menuSceneName);
    }

}
