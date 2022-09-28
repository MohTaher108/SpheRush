using UnityEngine;
using UnityEngine.UI;

public class RightSideBar : MonoBehaviour
{
    [HideInInspector]
    public bool isPaused;
    [HideInInspector]
    public bool doPathCheck; // If this value is set to true, the wave spawner will detect it and call PathCheck()
    private bool isStarted; 
    [HideInInspector]
    public GameObject pathCheckEnemy;

    [Header("Unity Stuff")]
    public Sprite playButtonSprite;
    public Sprite pauseButtonSprite;
    public Image playSprite;

    public HowToPlay HowToPlayUIScript;

    public GameObject pathCheckButton;

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
            if(pathCheckEnemy != null)
            {
                Destroy(pathCheckEnemy);
                GameStats.EnemiesAlive--;
            }

            isStarted = true;
            pathCheckButton.SetActive(false);
        }

        isPaused = !isPaused;

        if(isPaused)
            playSprite.sprite = playButtonSprite;
        else
            playSprite.sprite = pauseButtonSprite;
    }

    public void HowToPlay()
    {
        HowToPlayUIScript.Activate();
    }

    public void PathCheck()
    {
        doPathCheck = true;
        pathCheckButton.SetActive(false);
    }
}
