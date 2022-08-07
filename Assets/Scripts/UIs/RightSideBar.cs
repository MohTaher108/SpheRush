using UnityEngine;
using UnityEngine.UI;

public class RightSideBar : MonoBehaviour
{

    public Sprite playButtonSprite;
    public Sprite pauseButtonSprite;
    private bool isPaused;

    public Image image;

    public WaveSpawner waveSpawner;

    public GameObject HowToPlayUI;

    private Button pauseButton;

    void Start()
    {
        isPaused = true;
        waveSpawner.enabled = false;
        image.sprite = playButtonSprite;
        // Get the pause button from the Buttons gameObject
        pauseButton = this.transform.Find("Buttons").Find("Play/Pause").gameObject.GetComponent<Button>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Toggle();
    }

    // Toggles the pause/play
    public void Toggle()
    {
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
}
