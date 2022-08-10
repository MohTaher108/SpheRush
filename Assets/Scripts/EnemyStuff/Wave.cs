using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class Wave
{

    // Wave's type of enemy, number of enemies, and rate of spawning enemies
    public GameObject enemyPrefab;
    public int count;
    public float rate;
    public int moneyGained;

    // Parse Wave Info
    public Wave(StreamReader sr, GameObject Enemy_Simple, GameObject Enemy_Fast, GameObject Enemy_Tough)
    {
        // Read in money gained
        string str = sr.ReadLine();
        moneyGained = Int32.Parse(str);

        char curChar;
        string curNumString, waveData = sr.ReadLine();
        for(int index = 0; index < waveData.Length; index++)
        {
            // Read in the enemy type
            curChar = waveData[index++];
            if(curChar == 'S')
                enemyPrefab = Enemy_Simple;
            else if(curChar == 'F')
                enemyPrefab = Enemy_Fast;
            else if(curChar == 'T')
                enemyPrefab = Enemy_Tough;
            index++; // Skip comma

            // Read in the enemy count
            curNumString = "";
            curChar = waveData[index++];
            while(curChar != ',')
            {
                curNumString += curChar;
                curChar = waveData[index++];
            }
            count = Int32.Parse(curNumString);
            
            // Read in the enemy spawn rate
            curNumString = "";
            curChar = waveData[index++];
            while(curChar != ';')
            {
                curNumString += curChar;
                curChar = waveData[index++];
            }
            rate = float.Parse(curNumString);
        }
    }

}
