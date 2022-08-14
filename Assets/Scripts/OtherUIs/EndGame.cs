using UnityEngine;

public class EndGame : MonoBehaviour
{
    
    public void Menu()
    {
        SceneFader.instance.FadeTo(SceneFader.menuSceneName);
    }

}
