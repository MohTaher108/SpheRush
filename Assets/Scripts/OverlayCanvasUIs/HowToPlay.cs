using UnityEngine;

public class HowToPlay : MonoBehaviour
{

    public GameObject HowToPlayUI;

    public void Close()
    {
        AudioManager.instance.Play("HowToPlayClosed");
        HowToPlayUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Activate()
    {
        AudioManager.instance.Play("HowToPlayOpened");
        HowToPlayUI.SetActive(true);
        Time.timeScale = 0f;
    }
    
}
