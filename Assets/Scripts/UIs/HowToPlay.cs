using UnityEngine;

public class HowToPlay : MonoBehaviour
{

    public GameObject HowToPlayUI;

    public void Close()
    {
        HowToPlayUI.SetActive(false);
        Time.timeScale = 1f;
    }
    
}
