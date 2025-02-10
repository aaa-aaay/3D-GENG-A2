using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private float lookRadius = 10.0f;
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float attackCD = 1.0f;
    [SerializeField] private float chaseRadius = 15.0f;
    private bool seenPlayer;

    private float attackTimer = 0.0f;
    private Animator _animator;
    private Transform target;
    private NavMeshAgent agent;
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        float distanceFromPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceFromPlayer < lookRadius) {


            seenPlayer = true;
            if (distanceFromPlayer > attackRange) {
                if(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Z attack1")
                {
                    agent.speed = 3;
                    agent.SetDestination(target.position);
                    _animator.SetBool("IsWalking", true);
                }

            }

            if (distanceFromPlayer < attackRange && attackTimer > attackCD)
            {
                AudioManager.instance.PlaySFX("ZombieSound", transform.position);
                agent.speed = 0;
                attackTimer = 0;
                _animator.SetTrigger("Attack");
                _animator.SetBool("IsWalking", false);
            }

        }
        else if(seenPlayer && distanceFromPlayer < chaseRadius)
        {
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            seenPlayer = false;
            agent.speed = 0;
            agent.ResetPath();
        }


        

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
