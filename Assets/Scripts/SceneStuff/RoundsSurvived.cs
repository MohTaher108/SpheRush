using UnityEngine;
using System.Collections;
using TMPro;

public class RoundsSurvived : MonoBehaviour
{

    public TextMeshProUGUI roundsText;

    // Display number of rounds the player has survived
    void OnEnable()
    {
        StartCoroutine(AnimateText());
    }

    // A coroutine that animates the text going from 0 to the # of rounds survived
    IEnumerator AnimateText()
    {
        roundsText.text = "0";
        int round = 0;

        // Wait for the canvas to finish animating
        yield return new WaitForSeconds(.7f);

        // Animate the numbers from 0 to # of rounds survived, and wait 0.05 seconds in between each iteration so the user can see the numbers go up
        while(round < PlayerStats.Rounds)
        {
            round++;
            roundsText.text = round.ToString();

            yield return new WaitForSeconds(.05f);
        }
    }

}
