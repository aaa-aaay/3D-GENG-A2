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

    void Start()
    {
        _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = _playerInput.actions;
    }
    void Update()
    {

        Vector2 input = _inputActions["Movement"].ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);

        if (moveDirection.magnitude > 0)
        {
            _animator.SetBool("IsWalking", true);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else if (_inputActions["Attack"].IsPressed())
        {
            _animator.SetBool("IsAttack", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsAttack", false);

        }

    }
    private void OnAnimatorMove()
    {
        Vector3 velocity = _animator.deltaPosition;
        _cc.Move(velocity);
    }
}
