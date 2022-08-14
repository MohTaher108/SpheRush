using UnityEngine;
using TMPro;

public class WaveCountUI : MonoBehaviour
{

    public TextMeshProUGUI waveCountText;
    
    void Update()
    {
        int curRound = (PlayerStats.Rounds + 1);
        if(curRound > GameStats.WaveCount)
            curRound = GameStats.WaveCount;
        // Update the money text display
        waveCountText.text = curRound.ToString() + "/" + GameStats.WaveCount.ToString();
    }

}
