using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    private Player _player;
    private Vector3 _direction;
    public float speed;
    public void EnemySpellStart(Player player,Vector3 dir)
    {
        _player = player;
        _direction = dir;
    }

    private void Update()
    {
        transform.position += _direction * Time.deltaTime * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            FXManager.Instance.PlayEnemySpellImpact(transform.position);
            _player.TakeDamage(2);
            AudioManager.Instance.PlaySpellImpactAS();  
            Destroy(gameObject);
        }
        if(other.CompareTag("Wall"))
        {
            FXManager.Instance.PlayEnemySpellImpact(transform.position);
            Destroy(gameObject);
        }
    }
}
