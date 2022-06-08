using UnityEngine;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    // Prefab of enemy object
    public Transform enemyPrefab;

    // Spawnpoint of enemies
    public Transform spawnPoint;

    // Time between each wave of enemies and a variable to count down till the next wave
    public float timeBetweenWaves = 5.5f;
    private float countdown = 2f;

    // Countdown text overlay
    public TextMeshProUGUI waveCountdownText;

    // Number of waves that have occurred
    private int waveIndex = 0;

    // Time between each enemy that gets spawned, so they don't overlap
    private float timeBetweenEnemies = 0.5f;
    
    void Update()
    {
        // Spawn a wave when countdown hits 0
        if(countdown <= 0f)
        {
            // Instead of saying SpawnWave(), we use StartCoroutine() which allows us to implement a pause between each enemy spawn
            StartCoroutine(SpawnWave());
            // Reset countdown
            countdown = timeBetweenWaves;
        }

        // Decrement countdown
        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        // Update countdown text overlay
        waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    // Spawn a wave of enemies
    IEnumerator SpawnWave()
    {
        // Increment which wave we're on
        waveIndex++;
        PlayerStats.Rounds++;

        // Spawn a waveNumber amount of enemies
        for (int i = 0; i < waveIndex; i++)
        {
            // Spawn the enemy then wait timeBetweenEnemies seconds
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    // Spawn an enemy
    void SpawnEnemy()
    {
        // Instantiate an enemy
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
