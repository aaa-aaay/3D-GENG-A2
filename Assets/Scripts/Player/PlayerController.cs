using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;

    //private Animator _animator;
    private CharacterController _cc;
    private AnimationManager _animManager;

    private bool isAttacking, isjumping, isRolling;

    void Start()
    {
       // _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _animManager = GetComponent<AnimationManager>();
        _inputActions = _playerInput.actions;
    }
    void Update()
    {

        Vector2 input = _inputActions["Movement"].ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);

        if (moveDirection.magnitude > 0)
        {
            _animManager.PlayAnimation(AnimationManager.AniState.RUN);

            moveDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * moveDirection;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);

        }
        else if (_inputActions["Attack"].WasPressedThisFrame())
        {
            _animManager.PlayAnimation(AnimationManager.AniState.ATTACK);
            isAttacking = true;
            Invoke("AttackComplete", _animManager.animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        else if (_inputActions["Roll"].IsPressed())
        {
            _animManager.PlayAnimation(AnimationManager.AniState.ROLL);
            isRolling = true;

        }
        else if(!isAttacking && !isRolling)
        {

            _animManager.PlayAnimation(AnimationManager.AniState.IDLE);
        }
    }
    private void OnAnimatorMove()
    {
        Vector3 velocity = _animManager.animator.deltaPosition;
        _cc.Move(velocity);
    }

    void AttackComplete()
    {
        isAttacking = false;
    }
}
