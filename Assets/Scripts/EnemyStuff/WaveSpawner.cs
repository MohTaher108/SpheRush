using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

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

    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();

    void Start()
    {
        begunNextWave = false;
        currentWave = null;
        
        // Load the waveData for the level
        TextAsset waveDataFile = Resources.Load<TextAsset>("LevelWaves/" + SceneManager.GetActiveScene().name);
        waveData = waveDataFile.text.Split(new string[] { "\n" }, StringSplitOptions.None);
        GameStats.WaveCount = waveData.Length;

        enemyPrefabs["Fast"] = Resources.Load<GameObject>("Enemies/Types/Enemy_Fast");
        enemyPrefabs["Simple"] = Resources.Load<GameObject>("Enemies/Types/Enemy_Simple");
        enemyPrefabs["Tough"] = Resources.Load<GameObject>("Enemies/Types/Enemy_Tough");
        enemyPrefabs["Fake"] = Resources.Load<GameObject>("Enemies/Types/Enemy_Fake");
        enemyPrefabs["Witch"] = Resources.Load<GameObject>("Enemies/Types/Enemy_Witch");
        enemyPrefabs["Skeleton"] = Resources.Load<GameObject>("Enemies/Types/Enemy_Skeleton");
    }

    void Update()
    {
        if(doPathCheck)
        {
            gameManager.rightSideBar.doPathCheck = false;
            gameManager.rightSideBar.pathCheckEnemy = SpawnEnemy(enemyPrefabs["Fake"]);
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

            AudioManager.instance.Play("WaveComplete");
        }

        if(!isWavePaused)
        {
            StartCoroutine(SpawnWave());
            begunNextWave = true;
        }
    }

    IEnumerator SpawnWave()
    {
        currentWave = new Wave(waveData, enemyPrefabs);
        
        while(currentWave.count > 0)
        {
            SpawnEnemy(currentWave.enemyPrefab);
            currentWave.count--;
            yield return new WaitForSeconds(1f / currentWave.rate); // Wait 1/rate time between spawning each enemy
        }
    }

    GameObject SpawnEnemy(GameObject enemy)
    {
        GameStats.EnemiesAlive++;
        return Instantiate(enemy, enemySpawnPoint.position, enemySpawnPoint.rotation);
    }
}
