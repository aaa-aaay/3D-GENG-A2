using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private SphereCollider _collider;
    [SerializeField]
    private CinemachineImpulseSource source;

    // Start is called before the first frame update
    void Start()
    {
        _collider.enabled = false;
    }

    private void Update()
    {
        LayerMask layer = LayerMask.GetMask("Target");
        if (_collider.enabled)
        {
            Collider[] hitColliders = Physics.OverlapSphere(_collider.transform.position, _collider.radius, layer);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                _collider.enabled = false;
                Debug.Log(hitColliders[i].gameObject.name);

                source.GenerateImpulse(Camera.main.transform.forward);
            }
        }
    }

    public void CheckHit()
    {
        Debug.Log("Shit has been called???");
        _collider.enabled = true;
    }
    public void DisableCollider()
    {
        _collider.enabled = false;
    }
}
