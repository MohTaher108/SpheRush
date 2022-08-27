using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static bool GameIsOver;

    public GameObject overlayCanvas;
    private GameObject gameOverUI;
    private GameObject completeLevelUI;
    private PauseMenu pauseMenu;
    [HideInInspector]
    public RightSideBar rightSideBar;

    public int levelNumber;

    void Start()
    {
        GameIsOver = false;

        gameOverUI = overlayCanvas.transform.Find("GameOver").gameObject;
        completeLevelUI = overlayCanvas.transform.Find("CompleteLevel").gameObject;        
        pauseMenu = overlayCanvas.transform.Find("PauseMenu").gameObject.GetComponent<PauseMenu>();
        rightSideBar = overlayCanvas.transform.Find("RightSideBar").gameObject.GetComponent<RightSideBar>();

        // Initilaize the levelNumber in completeLevel so we know whether the level has been previously completed or not
        CompleteLevel CompleteLevelScript = completeLevelUI.GetComponent<CompleteLevel>();
        CompleteLevelScript.levelNumber = this.levelNumber;
    }

    void Update()
    {
        if(GameIsOver)
            return;

        if(PlayerStats.Lives <= 0) {
            EndGame();
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            pauseMenu.Toggle();

        if(Input.GetKeyDown(KeyCode.E))
        {
            EndGame();
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            WinLevel();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale = 5f;
        }

        if(Input.GetKeyDown(KeyCode.Semicolon))
        {
            Time.timeScale = 2f;
        }

        if(Input.GetKeyDown(KeyCode.Quote))
        {
            Time.timeScale = 1f;
        }
    }

    void EndGame()
    {
        AudioManager.instance.Play("GameOver");
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        AudioManager.instance.Play("LevelComplete");
        GameIsOver = true;
        completeLevelUI.SetActive(true);
    }
    
}
