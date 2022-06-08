using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    
    // Update is called once per frame
    void Update()
    {
        // Update the money text display
        moneyText.text = "$" + PlayerStats.Money.ToString();
    }
}
