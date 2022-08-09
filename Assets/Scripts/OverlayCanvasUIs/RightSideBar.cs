using UnityEngine;
using UnityEngine.UI;

public class RightSideBar : MonoBehaviour
{

    public Sprite playButtonSprite;
    public Sprite pauseButtonSprite;
    private bool isPaused;

    // Check if game was ever played, to disable the pathCheckObject
    private bool gameStart;

    public Image image;

    public WaveSpawner waveSpawner;

    public GameObject HowToPlayUI;

    public GameObject pathCheckObject;

    void Start()
    {
        isPaused = true;
        waveSpawner.enabled = false;
        image.sprite = playButtonSprite;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Toggle();
    }

    // Toggles the pause/play
    public void Toggle()
    {
        // Disable path check if game starts
        if(!gameStart)
        {
            pathCheckObject.SetActive(false);
        }

        isPaused = !isPaused;

        if(isPaused)
        {
            waveSpawner.enabled = false;
            image.sprite = playButtonSprite;
        } else
        {
            waveSpawner.enabled = true;
            image.sprite = pauseButtonSprite;
        }
    }

    public void HowToPlay()
    {
        HowToPlayUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PathCheck()
    {
        waveSpawner.PathCheck();
        pathCheckObject.SetActive(false);
    }
}
