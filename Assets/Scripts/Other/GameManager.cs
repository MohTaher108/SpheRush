using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static bool GameIsOver;

    public GameObject overlayCanvas;
    private GameObject gameOverUI;
    private GameObject completeLevelUI;
    private PauseMenu pauseMenu;

    public int levelNumber;

    void Start()
    {
        GameIsOver = false;

        gameOverUI = overlayCanvas.transform.Find("GameOver").gameObject;
        completeLevelUI = overlayCanvas.transform.Find("CompleteLevel").gameObject;        
        pauseMenu = overlayCanvas.transform.Find("PauseMenu").gameObject.GetComponent<PauseMenu>();

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

        // if(Input.GetKeyDown(KeyCode.L))
        // {
        //     Time.timeScale = 5f;
        // }

        // if(Input.GetKeyDown(KeyCode.Semicolon))
        // {
        //     Time.timeScale = 2f;
        // }

        // if(Input.GetKeyDown(KeyCode.Quote))
        // {
        //     Time.timeScale = 1f;
        // }
    }

    void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        GameIsOver = true;
        completeLevelUI.SetActive(true);
    }
    
}
