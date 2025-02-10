using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, Damagable
{
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;

    private Animator _animator;
    private CharacterController _cc;
    // private AnimationManager _animManager;

    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.5f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private int health = 200;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private BlockHandler _blockHandler;
    [SerializeField] private healthBar healthbar;
    //[SerializeField] private ClimbCheck climbAimerBottom;
    //[SerializeField] private ClimbCheck climbAimerTop;

    private Vector3 moveDirection;
    private bool isJumping = false;
    private bool isClimbing = false;
    private bool isDead = false;
    private bool isCrouch = false;
    private bool running1 = false;


    InputAction attackAction, sprintAction, dodgeAction, jumpAction, blockAction, restartAction, crouchAction;

    private float movementVelocity;
    private Vector3 jumpVelocity;
    private float gravity = -9.81f;
    private bool _isAttack = false;
    private int _attackStep = 0;

    private List<IEnumerator> _attackQueue = new List<IEnumerator>();
    private string[] _attackNames = { "Attack1", "Attack2", "Attack3" };


    void Start()
    {
        _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = _playerInput.actions;
        attackAction = _inputActions["Attack"];
        sprintAction = _inputActions["Sprint"];
        dodgeAction = _inputActions["Roll"];
        jumpAction = _inputActions["Jump"];
        blockAction = _inputActions["Block"];
        restartAction = _inputActions["Restart"];
        crouchAction = _inputActions["Crouch"];

        healthbar.SetMaxHealth(health);
        healthbar.SetHealth(health);


    }
    void Update()
    {

        if (restartAction.WasPressedThisFrame())
        {
            SceneManager.LoadScene(0);
        }

        Movements();

        if (attackAction.WasPressedThisFrame())
        {
            if (_attackQueue.Count < 3)
            {
                _attackQueue.Add(PerformAttack());
            }
            if (_attackQueue.Count == 1)
            {
                StartCombo();

            }
        }


        if (dodgeAction.WasPressedThisFrame()) _animator.SetTrigger("Roll");




        if (blockAction.IsInProgress())
        {
            _animator.SetBool("Isblocking", true);
        }
        else
        {
            _animator.SetBool("Isblocking", false);
        }



        if (crouchAction.WasPressedThisFrame()) { 
            isCrouch = !isCrouch;
             _animator.SetBool("IsCrouching", isCrouch);
            if (isCrouch) {

                _cc.height = 1.5f;
           
            
            }
            else
            {
                _cc.height = 1.8f;
            }



        }
    }

    private void Movements()
    {
        Vector2 input = _inputActions["Movement"].ReadValue<Vector2>();


        if (_cc.isGrounded)
        {
            Vector3 moveDirection = new Vector3(input.x, 0, input.y);
            if (moveDirection.magnitude > 0)
            {
                _animator.SetBool("IsWalking", true);
                moveDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * moveDirection;
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);

            }
            else
            {
                _animator.SetBool("IsWalking", false);
            }
        }


        //Jumping
        if (_cc.isGrounded && jumpVelocity.y < 0) {
            jumpVelocity.y = -2f;
            isJumping = false;
        }

        if (!isJumping)
        {
            moveDirection = new Vector3(input.x, 0, input.y);
            if (moveDirection.magnitude > 0)
            {
                moveDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * moveDirection;
            }
        }

        if (jumpAction.WasPressedThisFrame() && _cc.isGrounded)
        {

            jumpVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            _animator.SetTrigger("Jump");
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f); // Increase speed for smoother rotatio

            isJumping = true;

        }


        jumpVelocity.y += gravity * Time.deltaTime;




        if (sprintAction.IsPressed() && movementVelocity < 5.0)
        {
            movementVelocity += Time.deltaTime * acceleration;
        }
        else if (!sprintAction.IsPressed() && movementVelocity > 0)
        {
            movementVelocity -= Time.deltaTime * deceleration;
            _animator.SetBool("IsRunning", false);
        }

        _animator.SetFloat("Velocity", movementVelocity);
    }

    private void StartCombo()
    {
        _isAttack = true;
        _animator.SetBool("IsAttack", _isAttack);
        StartCoroutine(_attackQueue[0]);
    }

    private IEnumerator PerformAttack()
    {

        _attackStep++;
        _animator.SetInteger("AttackStep", _attackStep);

        while (_attackStep > 0 &&  !IsCurrentAnimationReadyForNextStep(_attackNames[_attackStep - 1]))
        {
            yield return null;

        }
        if(_attackStep >= _attackQueue.Count)
        {
            ResetCombo();
            yield break;
        }
        else
        {
            StartCoroutine(_attackQueue[_attackStep]);
        }

    }

    private bool IsCurrentAnimationReadyForNextStep(string name)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 0.7f && stateInfo.IsName(name);

    }
    private void ResetCombo()
    {
        _isAttack = false;
        _attackStep = 0;
        _animator.SetInteger("AttackStep", _attackStep);
        _animator.SetBool("IsAttack", false);
        _attackQueue.Clear();
    }


    private void OnAnimatorMove()
    {
        Vector3 velocity = _animator.deltaPosition;

        if (isJumping)
        {
            velocity += moveDirection * Time.deltaTime * 5f; // Adjust speed if needed
        }

        _cc.Move(velocity + (jumpVelocity * Time.deltaTime));
    }

    public void TakeDamage(int amount)  
    {
        health -= amount;
        AudioManager.instance.PlaySFX("HitSound",transform.position);
        healthbar.SetHealth(health);
        _animator.SetTrigger("OnHit");
        ResetCombo();

        if (health <= 0 && isDead == false)
        {
            _animator.SetTrigger("OnDead");
            isDead = true;
        }

    }

    public void DesotryObject()
    {
    }

    public void ResetPlayer()
    {
        SceneManager.LoadScene(0);
    }

    public void StartBlocking()
    {
        _blockHandler.EnableColliderBlocking();

    }
    public void StopBlocking()
    {
        _blockHandler.DisableColliderBlocking();
    }

    public void PlayRunningSound()
    {
        if(running1) AudioManager.instance.PlaySFX("RunningSFX1", transform.position);
        if(!running1) AudioManager.instance.PlaySFX("RunningSFX2", transform.position);
        running1 = !running1;
    }
    public void PlaySheildSound()
    {
        AudioManager.instance.PlaySFX("ShieldSFX", transform.position);
    }

}
