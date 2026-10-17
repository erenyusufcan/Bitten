using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public float LevelTime; 
    private List<Enemy> _enemies = new List<Enemy>();   
    public void StartLevel(Player player)
    {
        foreach (var e in GetComponentsInChildren<Enemy>())
        {
            e.StartEnemy(player);
            _enemies.Add(e);
        }
        foreach (var s in GetComponentsInChildren<EnemySpawner>())
        {
            s.StartEnemySpawner(player.gameDirector.levelManager);
        }
    }

    public bool IsLevelCompleted()
    {
        if(_enemies.Count == 0)
        {
           return true;
        }
        return false;
    }

    public void StopLevel()
    {
        foreach (var e in GetComponentsInChildren<Enemy>())
        {
            e.SetPlayerDeath();
        }  
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }
}
