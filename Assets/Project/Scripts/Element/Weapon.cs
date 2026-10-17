using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon : MonoBehaviour
{
    private WeaponManager _weaponManager;
    public Bullet bullet;
    public Transform bulletSpawnPoint;
    public ParticleSystem shootEffect;
    public Light shootLight;
    public float timeBetweenBullets;
    private float _bulletTimer;
    public int startBulletCount;
    public int currentBulletCount;
    public float standingBulletSpeed;
    public float movingBulletSpeed;
    private float _currentBulletSpeed;
    public List<Bullet> bullets;
    public List<Bullet> activeBullets;
    public WeaponType weaponType;

    private void Awake()
    {
        InstantiateBullets();
    }

    private void InstantiateBullets()
    {
        for (int i = 0; i < 20; i++)
        {
            var bulletPrefab = Instantiate(bullet);
            bulletPrefab.transform.position = bulletSpawnPoint.position;
            bulletPrefab.gameObject.SetActive(false);
            bullets.Add(bulletPrefab);
        }
       
    }
    public void DestroyBullets()
    {
       for(int i = 0; i < bullets.Count; i++)
        {
            Destroy(bullets[i].gameObject);
        }
        bullets.Clear();       
    }

    private void Start()
    {
        _bulletTimer = timeBetweenBullets;
        currentBulletCount = startBulletCount;
    }
    private void Update()
    {
       
        if(weaponType == WeaponType.MachineGun)
        {
            if (!_weaponManager.canShoot || GameDirector.instance.gameState != GameState.Playing)
            {
                return;
            }
            if (_weaponManager.gameDirector.player.GetDirection().magnitude > 0)
            {
                timeBetweenBullets = movingBulletSpeed;
            }
            else
            {
                timeBetweenBullets = standingBulletSpeed;
            }

            _bulletTimer += Time.deltaTime;
            if (Input.GetMouseButton(0) && _bulletTimer > timeBetweenBullets && !EventSystem.current.IsPointerOverGameObject())
            {

                if(currentBulletCount <= 0)
                {
                    _weaponManager.canShoot = false;
                    StartCoroutine(ReloadCoroutine());
                    return;
                }
                currentBulletCount--;
                print(currentBulletCount);
                MachineGunShoot();
            }
        }
        else if(weaponType == WeaponType.ShotGun)
        {
            if(!_weaponManager.canShoot || GameDirector.instance.gameState != GameState.Playing)
            {
                return;
            }
            _bulletTimer += Time.deltaTime;
            if (Input.GetMouseButton(0) && _bulletTimer > timeBetweenBullets && !EventSystem.current.IsPointerOverGameObject())
            {
                ShotGunShoot();
            }
        }

    }

    private IEnumerator ReloadCoroutine()
    {
        AudioManager.Instance.PlayReloadAS();
        yield return new WaitForSeconds(2.5f);
        currentBulletCount = startBulletCount;
        _weaponManager.canShoot = true;
    }

    public void MachineGunShoot()
    { 
        var spawnedBullet = bullets[0];
        spawnedBullet.gameObject.SetActive(true);   
        spawnedBullet.transform.position = bulletSpawnPoint.position;
        AudioManager.Instance.PlayMachineGunShootAS();
        spawnedBullet.BulletMove(transform.forward,transform.position);
        spawnedBullet.StartBullet(this);
        _bulletTimer = 0;  
        shootEffect.Play();
        shootLight.DOKill();
        shootLight.intensity = 0;
        shootLight.DOIntensity(75,.075f).SetLoops(2,LoopType.Yoyo);
        bullets.Remove(spawnedBullet);
        activeBullets.Add(spawnedBullet);
    }
    public void ShotGunShoot()
    {
        for(int i =  0; i < 15; i++)
        {
           var spawnedBullet = bullets[0];
           spawnedBullet.gameObject.SetActive(true);
           spawnedBullet.transform.position = bulletSpawnPoint.position;
           AudioManager.Instance.PlayShotGunShootAS();
            spawnedBullet.StartBullet(this);
            float bulletRandomXpos = UnityEngine.Random.Range(-0.15f, 0.15f);
           float bulletRandomYpos = UnityEngine.Random.Range(-0.15f, 0.15f);
           Vector3 bulletRandomDirection = new Vector3(bulletRandomXpos,bulletRandomYpos,0);
           spawnedBullet.BulletMove((transform.forward + bulletRandomDirection).normalized,transform.position);
           bullets.Remove(spawnedBullet);
           activeBullets.Add(spawnedBullet);
        }
        _bulletTimer = 0;
        shootEffect.Play();
        shootLight.DOKill();
        shootLight.intensity = 0;
        shootLight.DOIntensity(75, .075f).SetLoops(2, LoopType.Yoyo);
    }

    public void GetWeaponScript(WeaponManager weaponManager)
    {
        _weaponManager = weaponManager;
    }
}

public enum WeaponType
{
    MachineGun,
    ShotGun
}

