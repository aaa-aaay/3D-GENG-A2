using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField]
    private CinemachineImpulseSource source;
    [SerializeField] public string sfxName;

    [SerializeField]
    private int damage = 40;

    // Start is called before the first frame update
    void Start()
    {
        _collider.enabled = false;
    }

    private void Update()
    {
        if (_collider.enabled)
        {
            BoxCollider boxCollider = _collider as BoxCollider;
            if (boxCollider != null)
            {
                Vector3 worldCenter = boxCollider.transform.TransformPoint(boxCollider.center);
                Vector3 worldSize = Vector3.Scale(boxCollider.size, boxCollider.transform.lossyScale);
                Quaternion worldRotation = boxCollider.transform.rotation;

                Collider[] hitColliders = Physics.OverlapBox(worldCenter, worldSize * 0.5f, worldRotation);

                Debug.Log("Found objects: " + hitColliders.Length);
                foreach (Collider hitObject in hitColliders)
                {

                    Damagable damagable = hitObject.GetComponent<Damagable>();

                    if (damagable == null) continue; // Continue instead of return
                    if (hitObject.gameObject.layer == gameObject.layer) continue;

                    Debug.Log(hitObject.gameObject.name);

                    damagable.TakeDamage(damage);
                    _collider.enabled = false;
                    if(source != null)
                    source.GenerateImpulse(Camera.main.transform.forward);
                }
            }
        }
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }
    public void DisableCollider()
    {
        _collider.enabled = false;
    }
    public void PlayAttackSound()
    {
        AudioManager.instance.PlaySFX(sfxName, _collider.gameObject.transform.position);
    }


    private void OnDrawGizmos()   
    {
        BoxCollider boxCollider = _collider as BoxCollider;
        if (boxCollider == null) return;

        Gizmos.color = Color.red;

        Vector3 worldCenter = boxCollider.transform.TransformPoint(boxCollider.center);
        Vector3 worldSize = Vector3.Scale(boxCollider.size, boxCollider.transform.lossyScale);
        Quaternion worldRotation = boxCollider.transform.rotation;

        // Draw the box wireframe with correct position, size, and rotation
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(worldCenter, worldRotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, worldSize);
    }
}
