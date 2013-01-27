using UnityEngine;
using System.Collections;

public class LevelBehavior : MonoBehaviour
{
    private WaveParameters[] waves =
    {
        new WaveParameters() { EnemiesToGenerate=10, Bosses=0, MinSpawInterval=1f, MaxSpawnInterval=3f, EnemyWeights = new int[] {50, 30, 20} }, 
        new WaveParameters() { EnemiesToGenerate=12, Bosses=0, MinSpawInterval=1f, MaxSpawnInterval=3f, EnemyWeights = new int[] {40, 40, 20} },
        new WaveParameters() { EnemiesToGenerate=13, Bosses=0, MinSpawInterval=1f, MaxSpawnInterval=2f, EnemyWeights = new int[] {30, 40, 30} },
        new WaveParameters() { EnemiesToGenerate=15, Bosses=0, MinSpawInterval=1f, MaxSpawnInterval=2f, EnemyWeights = new int[] {20, 40, 40} },
        new WaveParameters() { EnemiesToGenerate=20, Bosses=0, MinSpawInterval=1f, MaxSpawnInterval=1f, EnemyWeights = new int[] {10, 50, 40} },
        new WaveParameters() { EnemiesToGenerate=0, Bosses=1, MinSpawInterval=1f, MaxSpawnInterval=1f, EnemyWeights = new int[] {0, 0, 0} },
    };

    private EnemySpawner spawner;

    void Start()
    {
        spawner = GetComponent<EnemySpawner>();
    }

    void Update()
    {
        switch (GameState.CurrentState)
        { 
            case LevelState.WaveStarting:
                Debug.Log(string.Format("LevelBehavior: Comenzando wave {0}", GameState.WaveNumber));
                spawner.StartWave(waves[GameState.WaveNumber - 1]);
                GameState.CurrentState = LevelState.WaveStarted;
                break;

            case LevelState.WaveStarted:
                if (!(spawner.Working || spawner.HasActiveEnemies))
                {
                    if (GameState.WaveNumber == waves.Length)
                    {
                        GameState.CurrentState = LevelState.PlayerWon;
                    }
                    else
                    {
                        GameState.WaveNumber++;
                        GameState.CurrentState = LevelState.WaveStarting;
                    }
                }
                break;

            case LevelState.PlayerWon:
				Application.LoadLevel(3); //Gameover good.
                break;

            case LevelState.HeartWon:
                Application.LoadLevel(2); //Gameover bad.
                break;
        }
    }
}

public class WaveParameters
{
    public int EnemiesToGenerate;
    public int Bosses;
    public float MinSpawInterval;
    public float MaxSpawnInterval;
    public int[] EnemyWeights;    
}

public enum LevelState
{
    WaveStarting,
    WaveStarted,
    PlayerWon,
    HeartWon,
}
