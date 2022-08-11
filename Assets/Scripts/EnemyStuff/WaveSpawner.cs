using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.IO;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public TextAsset waveDataFile;
    private string[] waveData;
    private static Wave currentWave;

    private static int EnemiesAlive;

    [Header("Waves timer")]
    public float timeBetweenWaves = 2f;
    private float countdown;

    [Header("Unity Stuff")]
    public Transform enemySpawnPoint;
    public TextMeshProUGUI waveCountdownText;
    public GameManager gameManager;

    public GameObject Enemy_Fast;
    public GameObject Enemy_Simple;
    public GameObject Enemy_Tough;
    public GameObject Enemy_Fake;

    void Start()
    {
        countdown = 0;
        EnemiesAlive = 0;
        // Open wave data .txt file
        waveData = waveDataFile.text.Split(new string[] { "\n" }, StringSplitOptions.None);
    }

    void Update()
    {
        // Wait till wave is completed
        if(EnemiesAlive > 0)
        {
            return;
        }

        // Has the player finished all levels? (include lives check, because player wins otherwise when level ends as last enemy reaches the end)
        if(PlayerStats.Rounds == waveData.Length && PlayerStats.Lives > 0)
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
        currentWave = new Wave(waveData, Enemy_Simple, Enemy_Fast, Enemy_Tough);
        
        for (int i = 0; i < currentWave.count; i++)
        {
            SpawnEnemy(currentWave.enemyPrefab);
            EnemiesAlive++;
            yield return new WaitForSeconds(1f / currentWave.rate); // Wait 1/rate time between spawning each enemy
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, enemySpawnPoint.position, enemySpawnPoint.rotation);
    }

    public void PathCheck()
    {
        Instantiate(Enemy_Fake, enemySpawnPoint.position, enemySpawnPoint.rotation);
    }

    // Report that an enemy died and give the player money if they finished the wave
    public static void enemyDied()
    {
        EnemiesAlive--;

        if(EnemiesAlive <= 0)
        {
            PlayerStats.Rounds++;
            PlayerStats.Money += currentWave.moneyGained;
        }
    }

}
