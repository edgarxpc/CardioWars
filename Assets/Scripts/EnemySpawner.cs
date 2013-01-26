using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] Enemies;
    public Vector3 SpawnPosition = new Vector3(0, 0, -1);
    public float MinSpawnInterval = 5;
    public float MaxSpawnInterval = 10;    

    private float elapsedTime;
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = 0;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > nextSpawnTime)
        {
            elapsedTime = 0;
            nextSpawnTime = Random.Range(MinSpawnInterval, MaxSpawnInterval);
            Spawn(Enemies[Random.Range(1, Enemies.Length)]);
        }
    }

    void Spawn(Transform enemy)
    {
        Debug.Log(string.Format("EnemySpawner: Instantiating enemy type: {0}", enemy.name));
        Instantiate(enemy, SpawnPosition, Quaternion.identity);
    }
}
