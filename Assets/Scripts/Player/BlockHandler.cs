using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour, Damagable
{
    [SerializeField] Collider _collider;
    public bool attackBlocked;
    [SerializeField] private Animator _animator;


    private void Start()
    {
        _collider.enabled = false;
    }

    public void DesotryObject()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Attack has been blocked");
        if(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "BlockConnect") 
        _animator.SetBool("BlockConnect",true);
        AudioManager.instance.PlaySFX("BlockSound", transform.position);
        StartCoroutine(BlockFinish());
    }

    public void EnableColliderBlocking()
    {
        _collider.enabled = true;
    }
    public void DisableColliderBlocking()
    {

        _collider.enabled = false; 
    }

    IEnumerator BlockFinish()
    {
        yield return new WaitForSeconds(0.4f);
        _animator.SetBool("BlockConnect", false);
    }
}
