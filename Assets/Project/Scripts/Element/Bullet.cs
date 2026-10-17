using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _bulletSpeed;
    private Vector3 _bulletDirection;
    private Vector3 _weaponPosition;
    public float bulletDestroyDistance;
    private Weapon _weapon;

    public void StartBullet(Weapon weapon)
    {
        _weapon = weapon;
    }


    private void Update()
    {
        transform.position += _bulletDirection * Time.deltaTime * _bulletSpeed;
        DestroyBullet();
    }
    public void BulletMove(Vector3 WeaponDirection,Vector3 WeaponPosition)
    {
        _bulletDirection = WeaponDirection;
        _weaponPosition = WeaponPosition;
    }
    public void DestroyBullet()
    {
        if ((transform.position - _weaponPosition).magnitude > bulletDestroyDistance)
        {
            AddBackToPool();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            FXManager.Instance.PlayImpactParticle(transform.position);
            AddBackToPool();
        }

        if (other.CompareTag("Enemy"))
        {
            FXManager.Instance.PlayZombieImpactParticle(transform.position,transform.forward);
            AddBackToPool();
            print(transform.position);
            other.GetComponent<Enemy>().TakeDamage(1,transform.position);

        }
    }

    public void AddBackToPool()
    {
        if(_weapon == null)
        {
            Destroy(gameObject);
            return;
        }
        _weapon.activeBullets.Remove(this);
        _weapon.bullets.Add(this);
        gameObject.SetActive(false);
        transform.position = _weapon.bulletSpawnPoint.position;
    }
}
