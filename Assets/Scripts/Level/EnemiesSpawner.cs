using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private PlayerValues enemyCount;
    [SerializeField] private int enemiesToSpawn;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private int enemySpawnHeight = 7;
    [SerializeField] private Transform _groundParent;
    

    void Start()
    {
        enemyCount.maxValue = enemiesToSpawn;
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
        enemyCount.currentValue = enemiesToSpawn;
        
    }

    private void Update()
    {
        if(enemyCount.currentValue < enemyCount.maxValue)
        {
            SpawnEnemy();
            enemyCount.AddValue(1);

        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(_enemy);

        SpawnObject.SpawnObjectAtRandomPlace(enemy, enemySpawnHeight, _groundParent);
    }



}
