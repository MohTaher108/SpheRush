using UnityEngine;
using TMPro;

public class LivesUI : MonoBehaviour
{
    public TextMeshProUGUI livesText;

    // Update is called once per frame
    void Update()
    {
        // Update the lives text display
        if(PlayerStats.Lives == 1)
            livesText.text = PlayerStats.Lives + "LIFE";
        else
            livesText.text = PlayerStats.Lives + " LIVES";
    }
}
