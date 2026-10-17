using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float enemyStartHealth;
    private float _currentHealth;
    public HealthBar healthBar;
    private Rigidbody _rb;
    private Animator _animator;
    private HitFlash _hitFlash;

    public float speed;
    public float playerFollowDistance;
    public float attackDistance;

    private bool HitActionState;
    public Collectable collectable;

    private Player _player;
    public LayerMask PlayerCanSeeLayerMask;
    public LayerMask AttackTouchLayerMask;
    private Vector3 _playerLastSeenPos;
    private bool _hasSeenPlayer;
    private NavMeshAgent _navMeshAgent;
    private CapsuleCollider _capsuleCollider;
    public SpriteRenderer shadow;
    public Light pointLight;
    public Transform zombieChestBone;
    private float timer;
    private bool __zombieSoundEffect;   

    private bool _ýsAttackInProgress;
    private bool _canAttack;
    private bool _biteOnce;
    public ActionState actionState;
    public AnimatonsState animatonsState;
    private AnimatonsState _stateBeforeGetHit;
    public EnemyType enemyType;
    private Coroutine _attackCoroutine;
    public float attackRate; 
    public EnemySpell enemySpellPrefab; 
    public Transform spellSpawnPoint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _hitFlash = GetComponent<HitFlash>();   
    }

    public void StartEnemy(Player player)
    {
        healthBar.HideHealthBar();
        shadow.enabled = true;
        _currentHealth = enemyStartHealth;
        _player = player;
        actionState = ActionState.Standing;
        _hasSeenPlayer = false;
    }

    private void Update()
    {
  
        Debug.DrawRay(transform.position + Vector3.up * .75f, transform.forward * 1.75f, Color.blue);
        Debug.DrawRay(transform.position + Vector3.up * .75f + Vector3.left * .3f, transform.forward * 1.75f, Color.blue);
        Debug.DrawRay(transform.position + Vector3.up * .75f + Vector3.right * .3f, transform.forward * 1.75f, Color.blue);
        if (actionState == ActionState.Die || _player.gameDirector.gameState != GameState.Playing)
        {
            return;
        }

        HitActionTimer();

       // print(timer);

        // Decide Action State
        if (GetPlayerDistanceMagnitude() < playerFollowDistance &&
            GetPlayerDistanceMagnitude() > attackDistance &&
            !_ýsAttackInProgress)
        {
            if (IfEnemyCanSeePlayer())
            {
                actionState = ActionState.WalkTowardsPlayer;
                _canAttack = true;
            }
            else
            {
                if (_hasSeenPlayer)
                {
                    actionState = ActionState.WalkPlayerLastSeenPos;
                    _canAttack = false;
                }
                else
                {
                    if (!HitActionState)
                    {
                        actionState = ActionState.Standing;
                        _canAttack = false;
                    }
                   
                }
            }
        }
        else if (GetPlayerDistanceMagnitude() > playerFollowDistance)
        {
            if (_hasSeenPlayer && (transform.position - _playerLastSeenPos).magnitude > 0.5f)
            {
                actionState = ActionState.WalkPlayerLastSeenPos;
                _canAttack = false;
            }
            else
            {
                if (!HitActionState)
                {
                    actionState = ActionState.Standing;
                    _canAttack = false;
                }
            }
        }
        else if (GetPlayerDistanceMagnitude() < attackDistance && _canAttack)
        {
            actionState = ActionState.Attack;
        }

        // Action State
        if (actionState == ActionState.WalkTowardsPlayer)
        {

            WalkTowardsPlayer();
        }
        else if (actionState == ActionState.WalkPlayerLastSeenPos)
        {

            WalkPlayerLastSeenPos();
        }
        else if (actionState == ActionState.Standing)
        {
            _navMeshAgent.isStopped = true;
            ChangeAnimaton(AnimatonsState.Idle);
        }   
        else if (actionState == ActionState.Attack)
        {
            Attack();     
        }

    }

    private void WalkTowardsPlayer()
    {
        if(animatonsState != AnimatonsState.GetHit)
        {
            if(!__zombieSoundEffect)
            {
                AudioManager.Instance.PlayZombieSoundEffectAS();
                __zombieSoundEffect = true;
            }
            _navMeshAgent.SetDestination(_player.transform.position);
            _navMeshAgent.isStopped = false;
            ChangeAnimaton(AnimatonsState.Walk);
        }
       
    }

    private void WalkPlayerLastSeenPos()
    {
        if (animatonsState != AnimatonsState.GetHit)
        {
            _navMeshAgent.SetDestination(_playerLastSeenPos);
            _navMeshAgent.isStopped = false;
            ChangeAnimaton(AnimatonsState.Walk);
        }
      
    }

    private void Attack()
    {
        if (!_ýsAttackInProgress )
        {
            _ýsAttackInProgress = true;
            _navMeshAgent.isStopped = true;
            ChangeAnimaton(AnimatonsState.Attack, true);
            _attackCoroutine = StartCoroutine(AttackCoroutine(attackRate));
        }
    }

    IEnumerator AttackCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if(enemyType == EnemyType.Basic || enemyType == EnemyType.Though)
        {
            if (GetPlayerDistanceMagnitude() < attackDistance && IsAttackTouchPlayer())
            {
                _player.TakeDamage(1);
            }

            yield return new WaitForSeconds(0.3f);

            transform.DOKill();
            transform.DOLookAt(_player.transform.position, 0.25f);

            yield return new WaitForSeconds(0.25f);
        }
        else if(enemyType == EnemyType.Ranged)
        {
            var spawnedEnemySpell = Instantiate(enemySpellPrefab);
            spawnedEnemySpell.transform.position = spellSpawnPoint.position;
            spawnedEnemySpell.EnemySpellStart(_player,(_player.transform.position-transform.position).normalized);
            spawnedEnemySpell.transform.SetParent(GetComponentInParent<Level>().transform);
            AudioManager.Instance.PlaySpellCastAS();
            Destroy(spawnedEnemySpell.gameObject, 5f);
            transform.DOKill();
            transform.DOLookAt(_player.transform.position, 0.25f);
        }
 
            _ýsAttackInProgress = false;

    }

    public void SetPlayerDeath()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
        if (!_biteOnce)
        {
            transform.LookAt(_player.playerChestBone.transform.position);
            ChangeAnimaton(AnimatonsState.BitingStart);
            _biteOnce = true;
        }
        _navMeshAgent.isStopped = true;
        healthBar.HideHealthBar();
        
    }

    private void ChangeAnimaton(AnimatonsState animaton, bool forcePlayAnimations=false)
    {
        if (animaton == AnimatonsState.Walk && (animatonsState != AnimatonsState.Walk || forcePlayAnimations))
        {
            _animator.CrossFade("Walk", .1f);
            animatonsState = AnimatonsState.Walk;
        }
        else if (animaton == AnimatonsState.Idle && (animatonsState != AnimatonsState.Idle || forcePlayAnimations))
        {
            _animator.SetTrigger("Idle");
            animatonsState = AnimatonsState.Idle;
        }
        else if (animaton == AnimatonsState.Attack && (animatonsState != AnimatonsState.Attack || forcePlayAnimations))
        {
            _animator.SetTrigger("Attack");
            animatonsState = AnimatonsState.Attack;
        }
        else if (animaton == AnimatonsState.GetHit && (animatonsState != AnimatonsState.GetHit || forcePlayAnimations))
        {
            _animator.SetTrigger("GetHit");
            animatonsState = AnimatonsState.GetHit;
            StartCoroutine(UpperBodyMaskCoroutine(.3f));
        }
        else if (animaton == AnimatonsState.BitingStart && (animatonsState != AnimatonsState.BitingStart || forcePlayAnimations))
        {
            _animator.SetTrigger("BitingStart");
            animatonsState = AnimatonsState.BitingStart;
         //   StartCoroutine(UpperBodyMaskCoroutine(.3f));
        }
        else if (animaton == AnimatonsState.Die && (animatonsState != AnimatonsState.Die || forcePlayAnimations))
        {
            _animator.CrossFade("Die", .1f);
            animatonsState = AnimatonsState.Die;
        }
    }
    IEnumerator UpperBodyMaskCoroutine(float delay)
    {
        _animator.SetLayerWeight(1, 1);
        yield return new WaitForSeconds(delay);
        _animator.SetLayerWeight(1, 0);
    }

    private bool IfEnemyCanSeePlayer()
    {
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, _player.transform.position - transform.position, Color.red);

        if (Physics.Raycast(transform.position + Vector3.up * 1.5f,
            _player.transform.position - transform.position,
            playerFollowDistance,
            PlayerCanSeeLayerMask))
        {
            return false;
        }

        _playerLastSeenPos = _player.transform.position;
        _hasSeenPlayer = true;
        return true;
    }

    private bool IsAttackTouchPlayer()
    {

        if(Physics.Raycast(transform.position + Vector3.up * .75f, transform.forward, 1.75f, AttackTouchLayerMask) ||
           Physics.Raycast(transform.position + Vector3.up * .75f + Vector3.left * .3f, transform.forward, 1.75f, AttackTouchLayerMask) ||
           Physics.Raycast(transform.position + Vector3.up * .75f + Vector3.right * .3f, transform.forward, 1.75f, AttackTouchLayerMask))
        {
            return true;
        }  
        return false;

    }

    public void TakeDamage(int damageAmaount,Vector3 hitPos)
    {
        HitActionState = true;
        timer = 0f;
        _currentHealth -= damageAmaount;
        AudioManager.Instance.PlayImpactAS();
        _hitFlash.PlayHitFlash();
        FXManager.Instance.PlayDamageText(hitPos,damageAmaount);
        StartCoroutine(PlayGetHitCoroutine());
        float fillAmount = (float)_currentHealth / enemyStartHealth;
        healthBar.ReduceBar(fillAmount);
        if(actionState == ActionState.Standing)
        {
            actionState = ActionState.WalkTowardsPlayer;
        }

        if (_currentHealth <= 0)
        {
            KillEnemy();
        }
    }

    void HitActionTimer()
    {
        if (HitActionState)
        {
            timer += Time.deltaTime;

            if (timer > 3f)
            {
                HitActionState = false;
                timer = 0f;
            }
        }
    }
  
    IEnumerator PlayGetHitCoroutine()
    {
        CancelAttack();
        if (animatonsState != AnimatonsState.GetHit)
        {
            _stateBeforeGetHit = animatonsState;
        }
        ChangeAnimaton(AnimatonsState.GetHit);
        _navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(.25f);
        ChangeAnimaton(_stateBeforeGetHit);


    }   

    private void KillEnemy()
    {
        CancelAttack();
        actionState = ActionState.Die;
        _stateBeforeGetHit = AnimatonsState.Die;
        _navMeshAgent.isStopped = true;
        _capsuleCollider.enabled = false;
        healthBar.HideHealthBar();
        shadow.enabled = false;
        pointLight.enabled = false;
        GetComponentInParent<Level>().RemoveEnemyFromList(this);
        ChangeAnimaton(AnimatonsState.Die);

        Invoke(nameof(PlayZombieDeathEffect), 2f);
        transform.DOKill();
        transform.DOMoveY(transform.position.y - 1, 1f).SetDelay(2.2f);  
        transform.DOScale(0, 1f).SetDelay(2.2f).OnComplete(() => Destroy(gameObject));
        Invoke(nameof(InstantiateCollectable), 2f);
    }
    public void InstantiateCollectable()
    {
        var spawnedCollectable = Instantiate(collectable,GetComponentInParent<Level>().transform);
        spawnedCollectable.transform.position = transform.position + Vector3.up * .2f;
    }
    private void PlayZombieDeathEffect()
    {
        FXManager.Instance.PlayZombieDeath(zombieChestBone.transform.position);
    }
    private void CancelAttack()
    {
        _ýsAttackInProgress = false;
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
    }

    public float GetPlayerDistanceMagnitude()
    {
        return (_player.transform.position - transform.position).magnitude;
    }
}

public enum ActionState
{
    Standing,
    WalkTowardsPlayer,
    WalkPlayerLastSeenPos,
    Attack,
    Die,
}

public enum AnimatonsState
{
    Idle,
    Walk,
    Attack,
    GetHit, 
    BitingStart,
    BitingLoop,
    Die
}

public enum EnemyType
{
    Basic,
    Though,
    Ranged
}