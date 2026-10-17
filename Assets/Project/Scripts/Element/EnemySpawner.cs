using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Enemy> enemies;    
    private LevelManager _levelManager;

    public void StartEnemySpawner(LevelManager levelManager)
    {
        _levelManager = levelManager;
       // var state = UnityEngine.Random.state;
        //UnityEngine.Random.InitState(_levelManager.currentlevel);
        SpawnEnemy();
       // UnityEngine.Random.state = state;
    }

    private void SpawnEnemy()
    {
        int enemyCount = (_levelManager.currentlevel - 1) / _levelManager.levels.Count;

        for (int i = 0; i < enemyCount; i++)
        {
           var enemyPrefab = Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Count)]);
           enemyPrefab.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-5f, 5f), 0, UnityEngine.Random.Range(-11f, 11f));
           enemyPrefab.transform.SetParent(GetComponentInParent<Level>().transform);
           enemyPrefab.StartEnemy(_levelManager.gameDirector.player);
        }
    }
}
