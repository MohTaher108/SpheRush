using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.IO;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    private string[] waveData;
    private static Wave currentWave;
    private bool isWavePaused { get { return gameManager.rightSideBar.isPaused; } }
    private bool doPathCheck { get { return gameManager.rightSideBar.doPathCheck; } }
    private bool begunNextWave;

    [Header("Unity Stuff")]
    public Transform enemySpawnPoint;
    public GameManager gameManager;

    [HideInInspector]
    public GameObject Enemy_Fast;
    [HideInInspector]
    public GameObject Enemy_Simple;
    [HideInInspector]
    public GameObject Enemy_Tough;
    [HideInInspector]
    public GameObject Enemy_Fake;

    void Start()
    {
        begunNextWave = false;
        currentWave = null;
        
        // Load the waveData for the level
        TextAsset waveDataFile = Resources.Load<TextAsset>("LevelWaves/" + SceneManager.GetActiveScene().name);
        waveData = waveDataFile.text.Split(new string[] { "\n" }, StringSplitOptions.None);
        GameStats.WaveCount = waveData.Length;

        Enemy_Fast = Resources.Load<GameObject>("Enemies/Types/Enemy_Fast");
        Enemy_Simple = Resources.Load<GameObject>("Enemies/Types/Enemy_Simple");
        Enemy_Tough = Resources.Load<GameObject>("Enemies/Types/Enemy_Tough");
        Enemy_Fake = Resources.Load<GameObject>("Enemies/Types/Enemy_Fake");
    }

    void Update()
    {
        if(doPathCheck)
        {
            gameManager.rightSideBar.doPathCheck = false;
            PathCheck();
        }

        // Wait till wave is completed
        if(GameStats.EnemiesAlive > 0 || PlayerStats.Lives <= 0 || (currentWave != null && currentWave.count > 0))
            return;

        if(begunNextWave)
        {
            begunNextWave = false;
            PlayerStats.Money += currentWave.moneyGained;
            PlayerStats.Rounds++;

            if(PlayerStats.Rounds == waveData.Length)
            {
                gameManager.WinLevel();
                this.enabled = false;
                return;
            }
        }

        if(!isWavePaused)
        {
            StartCoroutine(SpawnWave());
            begunNextWave = true;
        }
    }

    IEnumerator SpawnWave()
    {
        currentWave = new Wave(waveData, this);
        
        while(currentWave.count > 0)
        {
            SpawnEnemy(currentWave.enemyPrefab);
            GameStats.EnemiesAlive++;
            currentWave.count--;
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

}
