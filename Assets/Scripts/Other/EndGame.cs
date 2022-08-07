using UnityEngine;

public class EndGame : MonoBehaviour
{

    public SceneFader sceneFader;

    public void Menu()
    {
        sceneFader.FadeTo(SceneFader.menuSceneName);
    }

}
