using UnityEngine;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    private int waveIndex;

    private static int EnemiesAlive;
    public Transform enemySpawnPoint;

    public float timeBetweenWaves = 2f;
    private float countdown;
    private static Wave currentWave;

    public TextMeshProUGUI waveCountdownText;

    public GameManager gameManager;


    void Start()
    {
        countdown = 0;
        EnemiesAlive = 0;
        waveIndex = 0;
    }

    void Update()
    {
        // Wait till wave is completed
        if(EnemiesAlive > 0)
        {
            return;
        }

        // Has the player finished all levels? (include lives check, because player wins otherwise when level ends as last enemy reaches the end)
        if(waveIndex == waves.Length && PlayerStats.Lives > 0)
        {
            gameManager.WinLevel();
            this.enabled = false;
        }

        // Decrement countdown, disallow it from becoming negative, and update the countdown text overlay
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        waveCountdownText.text = string.Format("{0:00.00}", countdown);

        // Spawn a wave when countdown hits 0
        if(countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        currentWave = waves[waveIndex];
        EnemiesAlive = currentWave.count;
        for (int i = 0; i < currentWave.count; i++)
        {
            SpawnEnemy(currentWave.enemy);
            yield return new WaitForSeconds(1f / currentWave.rate); // Wait 1/rate time between spawning each enemy
        }

        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, enemySpawnPoint.position, enemySpawnPoint.rotation);
    }

    // Report that an enemy died and give the player money if they finished the wave
    public static void enemyDied()
    {
        EnemiesAlive--;

        if(EnemiesAlive <= 0)
        {
            PlayerStats.Money += currentWave.moneyGained;
        }
    }

}
