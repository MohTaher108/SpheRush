using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Wave
{

    // Wave's type of enemy, number of enemies, and rate of spawning enemies
    public GameObject enemyPrefab;
    public int count;
    public float rate;
    public int moneyGained;

    // Parse Wave Info
    public Wave(String[] waveData, Dictionary<string, GameObject> enemyPrefabs)
    {
        string curNumString; // A temporary string to extract numbers out of the wave data
        string curWaveData = waveData[PlayerStats.Rounds];
        int curWaveDataIndex = 0;
        char curChar;

        // Read in wave money gained
        curNumString = "";
        curChar = curWaveData[curWaveDataIndex++];
        while(curChar != ';')
        {
            curNumString += curChar;
            curChar = curWaveData[curWaveDataIndex++];
        }
        moneyGained = Int32.Parse(curNumString);

        // Read the enemy spawning information
        for(; curWaveDataIndex < curWaveData.Length; curWaveDataIndex++)
        {
            // Read in the enemy type
            curNumString = "";
            curChar = curWaveData[curWaveDataIndex++];
            while(curChar != ',')
            {
                curNumString += curChar;
                curChar = curWaveData[curWaveDataIndex++];
            }
            enemyPrefab = enemyPrefabs[curNumString];

            // Read in the enemy count
            curNumString = "";
            curChar = curWaveData[curWaveDataIndex++];
            while(curChar != ',')
            {
                curNumString += curChar;
                curChar = curWaveData[curWaveDataIndex++];
            }
            count = Int32.Parse(curNumString);
            
            // Read in the enemy spawn rate
            curNumString = "";
            curChar = curWaveData[curWaveDataIndex++];
            while(curChar != ';')
            {
                curNumString += curChar;
                curChar = curWaveData[curWaveDataIndex++];
            }
            rate = float.Parse(curNumString);
        }
    }

}
