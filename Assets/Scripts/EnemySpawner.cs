using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] Enemies;
    public Transform Boss;
    public Vector3 SpawnPosition = new Vector3(0, 0, -1);

    public bool Working { get; private set; }

    public bool HasActiveEnemies
    {
        get { return (activeEnemies.Count > 0); }
    }

    private int generatedCount;
    private float elapsedTime;
    private float nextSpawnTime;
    private WaveParameters parameters;
    private List<Transform> activeEnemies;

    void Start()
    {
        elapsedTime = 0;
        nextSpawnTime = 0;
        parameters = null;
        activeEnemies = new List<Transform>();
    }

    void Update()
    {
        if ((parameters != null) && (generatedCount < parameters.EnemiesToGenerate))
        {
            Working = true;
            elapsedTime += Time.deltaTime;
            if (elapsedTime > nextSpawnTime)
            {
                elapsedTime = 0;
                SpawnNext();
                nextSpawnTime = Random.Range(parameters.MinSpawInterval, parameters.MaxSpawnInterval);
                generatedCount++;
                Debug.Log(string.Format("EnemySpawner: {0}/{1} enemigos generados, siguiente en {2}", 
                    generatedCount, parameters.EnemiesToGenerate, nextSpawnTime));
            }
        }
        else
        {
            Working = false;
        }
    }

    void SpawnNext()
    {     
        int typeIndex = 2;
        int typeRange = Random.Range(0, 100);
        if (typeRange > (parameters.EnemyWeights[0] + parameters.EnemyWeights[1]))
        {
            typeIndex = 1;
        }
        else if (typeRange > (parameters.EnemyWeights[0]))
        {
            typeIndex = 0;
        }

        Transform enemy = (Transform)Instantiate(Enemies[typeIndex], SpawnPosition, Quaternion.identity);
        EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
        enemyBehavior.Untrack = new UntrackEnemy(RemoveActiveEnemy);
        activeEnemies.Add(enemy);
    }

    public void StartWave(WaveParameters parameters)
    {
        this.parameters = parameters;
        generatedCount = 0;
        activeEnemies.Clear();
    }

    public void RemoveActiveEnemy(Transform enemy)
    {
        activeEnemies.Remove(enemy);
    }
}
