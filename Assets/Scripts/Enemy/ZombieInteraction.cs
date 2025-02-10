using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieInteraction : MonoBehaviour, Damagable
{
    [SerializeField]private int health = 100;
    [SerializeField] private healthBar healthBar;
    private Animator _animator;



    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>(); 
        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(health);
    }

    public void DesotryObject()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        AudioManager.instance.PlaySFX("HitSound", transform.position);
        healthBar.SetHealth(health);
        _animator.SetTrigger("Hit");

        if (health <= 0)
        {
            _animator.SetTrigger("Death");
        }
    }
}
