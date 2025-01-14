using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;

    private Animator _animator;
    private CharacterController _cc;
    // private AnimationManager _animManager;

    float velocity;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.5f;
    InputAction attackAction, sprintAction, dodgeAction;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = _playerInput.actions;
        attackAction = _inputActions["Attack"];
        sprintAction = _inputActions["Sprint"];
        dodgeAction = _inputActions["Roll"];

    }
    void Update()
    {
        Movements();

        if (attackAction.WasPressedThisFrame()) _animator.SetTrigger("Punch");
        if (dodgeAction.WasPressedThisFrame()) _animator.SetTrigger("Roll");
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


        if (sprintAction.IsPressed() && velocity < 5.0)
        {
            velocity += Time.deltaTime * acceleration;
        }
        else if (!sprintAction.IsPressed() && velocity > 0)
        {
            velocity -= Time.deltaTime * deceleration;
            _animator.SetBool("IsRunning", false);
        }

        _animator.SetFloat("Velocity", velocity);
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = _animator.deltaPosition;
        _cc.Move(velocity);
    }
}
