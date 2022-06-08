using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool GameIsOver;

    public GameObject gameOverUI;

    void Start()
    {
        GameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If game has already ended, do nothing
        if(GameIsOver)
            return;

        if(Input.GetKeyDown("e"))
        {
            EndGame();
        }

        // If player lives finishes, end the game
        if(PlayerStats.Lives <= 0) {
            EndGame();
        }
    }

    // End the game
    void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }

}
