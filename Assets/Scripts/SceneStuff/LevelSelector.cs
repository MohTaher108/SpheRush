using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    
    public Button[] levelButtons;

    public TextMeshProUGUI StarsCount;

    // A secret string that will store inputs, and if the correct input is put in then it opens a secret level
    private int secretLevelKey;

    // Lock all the buttons the player hasn't unlocked
    void Start() 
    {
        int levelReached = PlayerPrefs.GetInt("StarsCount", 0) + 1;

        for (int i = levelReached; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }

        StarsCount.text = "" + (levelReached - 1);

        // If user has all the stars, initiliaze secret level's key to see if the user can find it
        if(PlayerPrefs.GetInt("StarsCount", 0) >= levelButtons.Length)
        {
            secretLevelKey = 0;
        } else // Else disable the Update() function
        {
            this.enabled = false;
        }
    }

    // Secret Level code functionality
    void Update()
    {
        // If the user inputted any of the first 5 letters of "secret" at the correct time then increment the key counter
        if((Input.GetKeyDown(KeyCode.S) && secretLevelKey == 0) || (Input.GetKeyDown(KeyCode.E) && secretLevelKey == 1) || (Input.GetKeyDown(KeyCode.C) && secretLevelKey == 2) 
            || (Input.GetKeyDown(KeyCode.R) && secretLevelKey == 3) || (Input.GetKeyDown(KeyCode.E) && secretLevelKey == 4))
        {
            secretLevelKey++;
            AudioManager.instance.Play("CorrectSecretCodeInput");
        } else if((Input.GetKeyDown(KeyCode.T) && secretLevelKey == 5)) // If the user finished the word, load the secret level
        {
            SceneFader.instance.FadeTo(SceneFader.secretLevelSceneName);
        } else if(Input.anyKeyDown && !Input.GetMouseButtonDown(0)) // If the user hits the wrong button, reset their progress (ignore mouse clicks)
        {
            AudioManager.instance.Play("IncorrectSecretCodeInput");
            secretLevelKey = 0;
        }
    }

    public void SelectLevel(string levelName)
    {
        SceneFader.instance.FadeTo(levelName);
    }

    public void Menu()
    {
        SceneFader.instance.FadeTo(SceneFader.menuSceneName);
    }

}
