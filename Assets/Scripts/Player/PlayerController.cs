using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;

    private Animator _animator;
    private CharacterController _cc;
    // private AnimationManager _animManager;


    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.5f;
    [SerializeField] private float jumpHeight = 2.0f;


    InputAction attackAction, sprintAction, dodgeAction, jumpAction, blockAction;

    private float movementVelocity;
    private Vector3 jumpVelocity;
    private float gravity = -9.81f;

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

    }
    void Update()
    {
        Movements();

        if (attackAction.WasPressedThisFrame()) _animator.SetTrigger("Punch");
        if (dodgeAction.WasPressedThisFrame()) _animator.SetTrigger("Roll");
        if (blockAction.IsInProgress())
        {
            _animator.SetBool("Isblocking", true);
        }
        else
        {
            _animator.SetBool("Isblocking", false);

        }
    }

    private void Movements()
    {
        Vector2 input = _inputActions["Movement"].ReadValue<Vector2>();
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

        //Jumping
        if (_cc.isGrounded && jumpVelocity.y < 0) {
            jumpVelocity.y = -2f;
        }
        jumpVelocity.y += gravity * Time.deltaTime;

        if (jumpAction.WasPressedThisFrame() && _cc.isGrounded) {
            jumpVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _animator.SetBool("Jump", true);
        }



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

    private void OnAnimatorMove()
    {
        Vector3 velocity = _animator.deltaPosition;
        _cc.Move(velocity + (jumpVelocity * Time.deltaTime));
    }
}
