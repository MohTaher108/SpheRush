using UnityEngine;
using UnityEngine.UI;

public class RightSideBar : MonoBehaviour
{
    [HideInInspector]
    public bool isPaused;
    [HideInInspector]
    public bool doPathCheck; // If this value is set to true, the wave spawner will detect it and call PathCheck()
    private bool isStarted; 

    [Header("Unity Stuff")]
    public Sprite playButtonSprite;
    public Sprite pauseButtonSprite;
    public Image playSprite;

    public GameObject HowToPlayUI;

    public GameObject pathCheckObject;

    void Start()
    {
        isPaused = true;
        doPathCheck = false;
        isStarted = false;
        playSprite.sprite = playButtonSprite;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Toggle();
    }

    // Toggles the pause/play
    public void Toggle()
    {
        if(!isStarted)
        {
            isStarted = true;
            pathCheckObject.SetActive(false);
        }

        isPaused = !isPaused;

        if(isPaused)
            playSprite.sprite = playButtonSprite;
        else
            playSprite.sprite = pauseButtonSprite;
    }

    public void HowToPlay()
    {
        HowToPlayUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PathCheck()
    {
        doPathCheck = true;
        pathCheckObject.SetActive(false);
    }
}
