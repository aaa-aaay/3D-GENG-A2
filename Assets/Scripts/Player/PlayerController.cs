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

    [SerializeField] private GameObject cameraTarget;

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
           // _animator.SetBool("IsWalking", true);
            _animManager.PlayAnimation(AnimationManager.AniState.RUN);

            moveDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * moveDirection;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);


        }
        else if (_inputActions["Attack"].IsPressed())
        {
           // _animator.SetBool("IsAttack", true);
            _animManager.PlayAnimation(AnimationManager.AniState.ATTACK);
        }
        else if (_inputActions["Roll"].IsPressed())
        {
            _animManager.PlayAnimation(AnimationManager.AniState.ROLL);

        }
        else
        {
            _animManager.PlayAnimation(AnimationManager.AniState.IDLE);

            //_animator.SetBool("IsWalking", false);
            //_animator.SetBool("IsAttack", false);

        }
    }
    private void OnAnimatorMove()
    {
        Vector3 velocity = _animManager.animator.deltaPosition;
        _cc.Move(velocity);
    }
}
