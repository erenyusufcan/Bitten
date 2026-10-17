using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
    
{
    public List<Enemy> inExplodeRangeEnemies;  
    private GameDirector _gameDirector;

    public void StartGrenade(GameDirector gameDirector)
    {
        _gameDirector = gameDirector;
    }   
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall"))
        {
            Explode();
        }
    }

   

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>(); 
            if(enemy != null && !inExplodeRangeEnemies.Contains(enemy))
            {
                inExplodeRangeEnemies.Add(enemy);
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null && inExplodeRangeEnemies.Contains(enemy))
            {
               inExplodeRangeEnemies.Remove(enemy);
            }
        }
    }
    private void Explode()
    {
        
        foreach (var enemy in inExplodeRangeEnemies)
        {
            if(enemy == null)
            {
                continue;
            }
            int explodeDamageDistance = Mathf.RoundToInt(Vector3.Distance(enemy.transform.position, transform.position));
            if(explodeDamageDistance <= 1)
            {
                explodeDamageDistance = 1;
            }
            else if(explodeDamageDistance >= 4)
            {
                explodeDamageDistance = 4;
            }
                enemy.TakeDamage(((4 / explodeDamageDistance)+2)+(((_gameDirector.levelManager.currentlevel - 1) / _gameDirector.levelManager.levels.Count)*2), enemy.transform.position + Vector3.up);
            print("Distance: " + explodeDamageDistance);
        }
        Destroy(gameObject);
        FXManager.Instance.PlayGrenadeExplosion(transform.position);
        AudioManager.Instance.PlayGrenadeExplosionAS();
    }
}
