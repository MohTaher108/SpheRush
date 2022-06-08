using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Money
    public static int Money;
    public int startMoney = 400;

    // Lives
    public static int Lives;
    public int startLives = 20;

    public static int Rounds;

    void Start()
    {
        // Initialize money and lives count
        Money = startMoney;
        Lives = startLives;
        Rounds = 0;
    }
}
