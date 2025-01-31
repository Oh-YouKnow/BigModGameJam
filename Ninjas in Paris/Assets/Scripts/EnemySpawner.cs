using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public WeightedRandomList<Transform> enemyPool;
    public int maxEnemies = 10;
    public float secondsBetweenSpawns = 2;
    public int currentEnemyCount;
    float _elapsedTime;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentEnemyCount = CountEnemy();
        Debug.Log("Counted");
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= secondsBetweenSpawns && currentEnemyCount < maxEnemies)
        {
            _elapsedTime = 0;
            Vector3 spawnPos = RandomSpawnPosition();
            Transform enemy = enemyPool.GetRandom();
            Instantiate(enemy, spawnPos, Quaternion.identity);
            currentEnemyCount++;
        }
    }

    private int CountEnemy()
    {
        int count;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        count = enemies.Length;
        return count;
    }

    // This function finds a random position to spawn an enemy offscreen.
    private Vector3 RandomSpawnPosition()
    {
        // First finds a point on a vertical line on the player, then randomly decides
        // whether to place the point left or right offscreen.
        
        Vector3 randPos = new Vector3(0, 0, Random.Range(-10, 5));
        Vector3 playerPos = player.transform.position;
        randPos += playerPos;
        int leftOrRight = Random.Range(-1, 1);
        if (leftOrRight < 0)
        {
            randPos.x -= 25f;
        }
        else
        {
            randPos.x += 25f;
        }
        return randPos;
    }
}
