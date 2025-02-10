using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCamera : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;
    private int _cameraIndex = 0;
    InputAction switchCamAction;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CinemachineFreeLook _freeLookCamera;
    private GameObject currentLockOnUI;
    [SerializeField] private GameObject lockOnPrefab;


    [SerializeField] private float lockOnRadius;
    public LayerMask enemyLayer;
    public Transform currentTarget;
    private Transform player;

    private CinemachineBrain _cinemachineBrain;


    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = _playerInput.actions;
        switchCamAction = _inputActions["SwitchCamera"];

        _cameraIndex = 0;
        _freeLookCamera.gameObject.SetActive(true);
        _virtualCamera.gameObject.SetActive(false);

        _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void Update()
    {
        SwitchCameraType();

        if (_cameraIndex == 1 && currentTarget != null)
        {
            _virtualCamera.transform.LookAt(currentTarget);
            _virtualCamera.transform.position = transform.position + new Vector3(0, 2, -5);
        }
        if(_cameraIndex ==1  && currentTarget == null)
        {
            LockOnEnemy();
            SetLockOnIndicator(currentTarget);
        }
    }

    private void SwitchCameraType()
    {
        if (switchCamAction.WasPressedThisFrame())
        {

            _cinemachineBrain.m_DefaultBlend.m_Time = 0.5f;
            if (_cameraIndex == 0 )
            {
                if (LockOnEnemy())
                {
                    _cameraIndex = 1;
                    _freeLookCamera.gameObject.SetActive(false);
                    _virtualCamera.gameObject.SetActive(true);
                    _virtualCamera.transform.LookAt(currentTarget);
                    SetLockOnIndicator(currentTarget);
                }



            }
            else if (_cameraIndex == 1)
            {
                _cameraIndex = 0;
                _freeLookCamera.gameObject.SetActive(true);
                _virtualCamera.gameObject.SetActive(false);
                currentTarget = null;

                // Remove UI indicator when exiting lock-on mode
                if (currentLockOnUI != null)
                {
                    Destroy(currentLockOnUI);
                    currentLockOnUI = null;
                }

            }
        }
    }
    private bool LockOnEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRadius, enemyLayer);
        if (enemies.Length > 0)
        {
            // Sort by closest enemy
            currentTarget = enemies.Select(e => e.transform)
                                   .OrderBy(t => Vector3.Distance(transform.position, t.position))
                                   .FirstOrDefault();
        }


        if (currentTarget != null)
        {
            return true;
        }
        else
        {
            currentTarget = null; 
            return false;
        }
    }

    private void SetLockOnIndicator(Transform enemy)
    {
        if (currentLockOnUI != null)
        {
            Destroy(currentLockOnUI);
        }
        Transform uiPosition = enemy.Find("LockOnUIPosition");

        if (uiPosition != null)
        {
            currentLockOnUI = Instantiate(lockOnPrefab, uiPosition.position, Quaternion.identity);
            currentLockOnUI.transform.SetParent(uiPosition);
        }
    }
}
