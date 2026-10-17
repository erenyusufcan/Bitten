using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform WeaponHolder;
    public List<Weapon> weapons;
    private Weapon _currentWeapon;
    public GameDirector gameDirector;  
    public bool canShoot;   

    private void Awake()
    {
      InstantiateMachineGun();
      _currentWeapon.GetWeaponScript(this);
       gameDirector.uiManager.bulletUI.StartBulletUI(_currentWeapon);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectMachineGun();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) )
        {
           SelectShotGun();
        }
    }

    public void SelectMachineGun()
    {
        if (_currentWeapon.weaponType == WeaponType.MachineGun)
        {
            return;
        }
        InstantiateMachineGun();
        gameDirector.player.ChangeWeaponAnimation();
        _currentWeapon.GetWeaponScript(this);
        gameDirector.uiManager.ShowBulletUI();
        gameDirector.uiManager.bulletUI.StartBulletUI(_currentWeapon);
    }
    public void SelectShotGun()
    {
        print("SelectShotGun");
        if (_currentWeapon.weaponType == WeaponType.ShotGun)
        {
            return;
        }
        InstantiateShotGun();
        gameDirector.player.ChangeWeaponAnimation();
        _currentWeapon.GetWeaponScript(this);
        gameDirector.uiManager.HideBulletUI();
    }

    public void InstantiateMachineGun()
    {
        if(_currentWeapon != null)
        {
            _currentWeapon.DestroyBullets();
            Destroy(_currentWeapon.gameObject);
        }
        _currentWeapon = Instantiate(weapons[0], WeaponHolder);
        _currentWeapon.transform.localPosition = Vector3.zero;
    }
    public void InstantiateShotGun()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.DestroyBullets();
            Destroy(_currentWeapon.gameObject);
        }
        _currentWeapon = Instantiate(weapons[1], WeaponHolder);
        _currentWeapon.transform.localPosition = Vector3.zero;

    }

    public WeaponType getWeaponType()
    {
        return _currentWeapon.weaponType;
    }

    public void RestartWeaponManager()
    {
       canShoot = true; 
       _currentWeapon.currentBulletCount = _currentWeapon.startBulletCount;
    }
    public IEnumerator CanShootTrueCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    public void CanShootFalseFunc()
    {
       canShoot = false;
    }

   
}
