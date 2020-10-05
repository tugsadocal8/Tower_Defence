using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class LevelManager
{
    public int loadLevel;
    public EnemyEnum enemyEnum;
    public float enemyOffTime;
    public float speed;
    public float health;
    public float monetaryValue;
    public LevelData LevelData;
    public int CurrentLevelIndex;
    public int CurrentWaveIndex;

    public void CreateWave()
    {

        for (int i = 0; i < LevelData.Levels[CurrentLevelIndex].Waves[CurrentWaveIndex].Enemies.Count; i++)
        {
            enemyEnum = (EnemyEnum)LevelData.Levels[CurrentLevelIndex].Waves[CurrentWaveIndex].Enemies[i].EnemyId;
            enemyOffTime = LevelData.Levels[CurrentLevelIndex].Waves[CurrentWaveIndex].Enemies[i].Offtime;
            speed = LevelData.Levels[CurrentLevelIndex].Waves[CurrentWaveIndex].Enemies[i].Speed;
            health = LevelData.Levels[CurrentLevelIndex].Waves[CurrentWaveIndex].Enemies[i].Health;
            monetaryValue = LevelData.Levels[CurrentLevelIndex].Waves[CurrentWaveIndex].Enemies[i].MonetaryValue;
            GameManager.Instance.SpawnEnemy(enemyEnum,enemyOffTime,speed, health, monetaryValue);

        }
    }

    public void LoadLevelData()
    {
        string json = File.ReadAllText(Application.dataPath + "/Data/LevelData.json");
        LevelData = JsonConvert.DeserializeObject<LevelData>(json);
    }


    public void SetWave(int index)
    {
        CurrentWaveIndex = index;
    }

    public void SetLevel(int index)
    {
        CurrentLevelIndex = index;
    }
}
