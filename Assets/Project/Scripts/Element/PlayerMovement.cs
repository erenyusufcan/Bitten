using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    public float MachineGunWalkSpeed;
    public float ShotGunWalkSpeed;
    public float RunSpeed;
    public float JumpSpeed;
    public float FallSpeed;
    public float movementspeed;
    private bool _isJumping;  

    public LayerMask JumpLayers;
    public LayerMask LookAtLayers;
    public SpaceKeyDecide spaceKeyDecide;   
    public Camera mainCamera;
    private Animator _animator;
    private Player _player;
    private GrenadeThrower _grenadeThrower;
    private Vector3 _direction;
    public float startStamina;
    public float currentStamina;  
    public StaminaBar staminaBar;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _grenadeThrower = GetComponent<GrenadeThrower>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        currentStamina = startStamina;  
        staminaBar.HideStaminaBar();
    }
    private void Update()
    {
        if(_player.gameDirector.gameState != GameState.Playing || _player.isDeath)
        {
            MovePlayer(Vector3.zero, 0);
            staminaBar.HideStaminaBar();
            return;
        }
        
        //Set Movement Direction
        _direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            _direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _direction += Vector3.back;
        }
        


        //Set Movement Speed

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(currentStamina > 0)
            {
                if(_player.gameDirector.weaponManager.getWeaponType() == WeaponType.ShotGun)
                {
                    movementspeed = RunSpeed * 3 / 5;
                }
                else
                {
                    movementspeed = RunSpeed;
                }
                if(_direction.magnitude > 0)
                {
                    DecreaseStamina();
                }
                else
                {
                    IncreaseStamina();
                }
            }
            else
            {
                if(_player.gameDirector.weaponManager.getWeaponType() == WeaponType.ShotGun)
                {
                    movementspeed = ShotGunWalkSpeed;
                }
                else
                {
                    movementspeed = MachineGunWalkSpeed;
                }
                movementspeed = MachineGunWalkSpeed;
            }
          
        }
        else
        {
            IncreaseStamina();
            if (_player.gameDirector.weaponManager.getWeaponType() == WeaponType.ShotGun)
            {
                movementspeed = ShotGunWalkSpeed;
            }
            else
            {
                movementspeed = MachineGunWalkSpeed;
            }
        }

        staminaBar.transform.position = transform.position + Vector3.up + Vector3.right * 5f; 
        print(currentStamina);

        //Jump
        _isJumping = !CheckIfIsGrounded();
        if (Input.GetKeyDown(KeyCode.Space) && CheckIfIsGrounded())
        {
            if(spaceKeyDecide == SpaceKeyDecide.Jump)
            {
                Jump();
            }
            else if(!_isDashing && _canDash)
            {
                StartCoroutine(Dash()); 
            }

        }

        //Player Move Function
        if (!_isDashing)
        {
            MovePlayer(_direction, movementspeed);
        }
        //Player Looking MousePosition
        PlayerLookAtMouse();

        SetWalkDirection(Vector3.SignedAngle(transform.forward, _direction, Vector3.up));
        dashPS.transform.position = transform.position + Vector3.up;

    }
    public Vector3 GetDirection()
    {
        return _direction;
    }
    private void DecreaseStamina()
    {
        currentStamina -= Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, startStamina);
        print(currentStamina);
        float staminaPercent = currentStamina / startStamina;
        staminaBar.ChangeBarFillAmount(staminaPercent);
        if (currentStamina <= .1f)
        {
            staminaBar.HideStaminaBar();
        }
    }

    private void IncreaseStamina()
    {
        currentStamina += Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, startStamina);
        float staminaPercent = currentStamina / startStamina;
        staminaBar.ChangeBarFillAmount(staminaPercent); 

        if (currentStamina >= startStamina - .2f)
        {
            staminaBar.HideStaminaBar();
        }
    }

    private bool _isDashing;
    private bool _canDash = true;
    public float dashForce;
    public float dashDuration;
    public ParticleSystem dashPS;
    IEnumerator Dash()
    {
        _isDashing = true;
        _canDash = false;
        _rb.AddForce(_direction * dashForce);
        dashPS.transform.LookAt(dashPS.transform.position - _direction);
        dashPS.Play();
        AudioManager.Instance.PlayDashAS(); 
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
        yield return new WaitForSeconds(.75f);  
        _canDash = true;
    }

    void SetWalkDirection(float angel)
    {
        _animator.SetFloat("WalkDirection", angel);
    }


    private void PlayerLookAtMouse()
    {
        Debug.DrawRay(mainCamera.transform.position, mainCamera.ScreenPointToRay(Input.mousePosition).direction * 50);
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.ScreenPointToRay(Input.mousePosition).direction, out var hit, 50, LookAtLayers))
        {
            var lookPos = hit.point;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }
    }

    //IsGrounded Controller
    private bool CheckIfIsGrounded()
    {

        if (Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, out RaycastHit hit, 2f, JumpLayers))
        {
            _player.shadow.transform.position = hit.point + Vector3.up * .01f;
        }

        if (Physics.Raycast(transform.position + Vector3.up * .2f, Vector3.down, .35f, JumpLayers))
        {
            return true;
        }
        Debug.DrawRay(transform.position + Vector3.up* .2f, Vector3.down*.35f);
        return false;
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * JumpSpeed);
        _isJumping = true;
        ChangeAnimation("Jump");
    }

    public void MovePlayer(Vector3 dir ,float movementSpeed)
    {
        Vector3 HorizontalVelocity = dir.normalized * movementSpeed;
        Vector3 VerticalVelocity = new Vector3(0, _rb.velocity.y, 0);

        if (VerticalVelocity.y > 0)
        {
            VerticalVelocity.y -= (FallSpeed/2) * Time.deltaTime;
        }
        if (VerticalVelocity.y < 0)
        {
            VerticalVelocity.y -= FallSpeed * Time.deltaTime;
        }

        _rb.velocity = HorizontalVelocity + VerticalVelocity;
        if (!_isJumping && _player.changeWeapon == false && _grenadeThrower.grenadeThrowing==false)
        {
            if (dir.magnitude > 0 )
            {
                ChangeAnimation("Run");
            }
            else
            {

                if (_player.gameDirector.gameState == GameState.Playing)
                {
                    ChangeAnimation("Idle");
                }
                    
            }
        }
           
    }
    public void RigidBodyConstrainOff()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void ChangeAnimation(string animation)
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("Run", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Die", false);
        _animator.SetBool("Drink", false);
        _animator.SetBool("TimeFail", false);
        _animator.SetBool("ChangeWeapon", false);
        _animator.SetBool("ThrowGrenade", false);
        _animator.SetBool(animation, true);
    }
    public void SetUpperBodyLayerWeightTo1()
    {
        _animator.SetLayerWeight(1, 1);
    }
    public void SetUpperBodyLayerWeightTo0()
    {
         _animator.SetLayerWeight(1, 0);
    }
}

public enum SpaceKeyDecide
{
    Jump,
    Dash
}
