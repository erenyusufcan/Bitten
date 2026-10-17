using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public Grenade grenadePrefab;
    public LayerMask grenadeLayerMask;
    public bool grenadeThrowing= false;
    private PlayerMovement _playerMovement;
    private float timer;
    public Transform leftHandTransform;
    public GrenadeUI grenadeUI;
    public GameDirector gameDirector;
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        timer = 3;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && timer > 3) 
        {
            ThrowGrenade();
        }
        if(gameDirector.gameState == GameState.Playing)
        {
            grenadeUI.SetFillBar(timer / 3);
        }
    }

    private void ThrowGrenade()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hit,20,grenadeLayerMask))
        {
            var distance = (hit.point - transform.position).magnitude;
            timer = 0;
            //print(distance);   
            if (distance < 4.5f)
            {
                StartCoroutine(ThrowGrenadeAnimationCoroutine(distance/7,hit.point));
            }
            else if(distance >= 4.5f)
            {
                StartCoroutine(ThrowGrenadeAnimationCoroutine(distance / 14, hit.point));
            }


        }

    }
    private IEnumerator ThrowGrenadeAnimationCoroutine(float delay,Vector3 hit)
    {
        gameDirector.weaponManager.CanShootFalseFunc();
        grenadeThrowing = true;
        _playerMovement.SetUpperBodyLayerWeightTo1();
        _playerMovement.ChangeAnimation("ThrowGrenade");
        yield return new WaitForSeconds(delay );
        
        gameDirector.weaponManager.StartCoroutine(gameDirector.weaponManager.CanShootTrueCoroutine(delay*1.3f));
        var grenade = Instantiate(grenadePrefab);
        grenade.StartGrenade(gameDirector);
        grenade.transform.position = leftHandTransform.position;
        grenade.transform.DOMoveX(hit.x, delay).SetEase(Ease.Linear);
        grenade.transform.DOMoveZ(hit.z, delay).SetEase(Ease.Linear);
        grenade.transform.DOMoveY(transform.position.y + 2.5f, delay / 2).SetEase(Ease.OutQuad);
        grenade.transform.DOMoveY(hit.y, delay / 2).SetDelay(delay / 2).SetEase(Ease.InQuad);

  
        yield return new WaitForSeconds(delay * 2);
        _playerMovement.SetUpperBodyLayerWeightTo0();
        grenadeThrowing = false;
    }
}

