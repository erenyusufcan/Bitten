using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float startHealth;
    private float _currentHealth;
    public bool isDeath;
    public bool changeWeapon;

    public HealthBar healthBar;
    public SpriteRenderer shadow;
    public List<Light> playerLights;
    public Transform playerChestBone;
    public Transform leftHand;
    public GameObject potionPrefab;

    private PlayerMovement _playerMovement;
    private FXManager _fxManager;
    public GameDirector gameDirector;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();  
        _fxManager = gameDirector.fxManager;
    }
    private void Update()
    {
        if(transform.position.y < -10f && gameDirector.gameState == GameState.Playing)
        {
            gameDirector.LevelLose(0);
           
        }
        if(transform.position.y < -20f && gameDirector.gameState == GameState.Lose)
        {
            gameObject.SetActive(false);    
        }
    }


    public void RestartPlayer()
    {
        _playerMovement.RigidBodyConstrainOff();
        _playerMovement.currentStamina = 5;
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(0,0,-10f);
        healthBar.HideHealthBar();
        TurnOnLights();
        shadow.enabled = true;
        _currentHealth = startHealth;
        isDeath = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Potion"))
        {
            gameDirector.LevelWin();
            other.gameObject.SetActive(false);  
            DrinkPotion();
            _fxManager.PlayPotionParticle(other.transform.position);  
            _playerMovement.ChangeAnimation("Drink");
        }
        if (other.gameObject.CompareTag("Collectable"))
        {
            print("Collectable");
            AudioManager.Instance.PlayCollectableAS();  
            Destroy(other.gameObject);
            gameDirector.timerManager.CollectableCollected();

        }
    }
    public Vector3 GetDirection()
    {
        return _playerMovement.GetDirection();
    }
    public void DrinkPotion()
    {
        var potion = Instantiate(potionPrefab,leftHand); 
        potion.transform.localPosition = new Vector3(0,.2f,0);  
        Destroy(potion, 4.3f);    
    }

    public void TakeDamage(float damageAmaount)
    {
        if(isDeath)
        {
            return;
        }
        gameDirector.uiManager.getHitUI.ShowGetHitEffect();
        AudioManager.Instance.PlayImpactAS();

        _currentHealth -= damageAmaount;
        float fillAmount = (float)_currentHealth / startHealth;
        healthBar.ReduceBar(fillAmount);

        if (_currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        isDeath = true; 
        _playerMovement.ChangeAnimation("Die");
        healthBar.HideHealthBar();
        TurnOffLights();
        shadow.enabled = false;
        gameDirector.LevelLose(3);
    }

    private void TurnOffLights()
    {
         foreach (var light in playerLights)
           {
             light.enabled = false;
           }   
    }
    public void TurnOnLights()
    {
         foreach (var light in playerLights)
           {
             light.enabled = true;
           }
    }

    public void TimeFailAnim()
    {
        _playerMovement.ChangeAnimation("TimeFail");    
    }
    public void ChangeWeaponAnimation()
    {
       StartCoroutine(ChangeWeaponAnimCoroutine());
    }
    private IEnumerator ChangeWeaponAnimCoroutine()
    {
        AudioManager.Instance.PlayChangeWeaponAS(); 
        changeWeapon = true;
        gameDirector.weaponManager.canShoot = false;
        _playerMovement.ChangeAnimation("ChangeWeapon");
        _playerMovement.SetUpperBodyLayerWeightTo1();
        yield return new WaitForSeconds(.75f);
        _playerMovement.SetUpperBodyLayerWeightTo0();
        gameDirector.weaponManager.canShoot = true;
        changeWeapon = false;
    }
}
