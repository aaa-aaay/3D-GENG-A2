using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    [SerializeField] public Animator animator;
    public enum AniState
    {
        IDLE,
        RUN,
        ATTACK,
        WALK,
        JUMP,
        ROLL
    }
    private AniState state = AniState.IDLE;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PlayAnimation(AniState.IDLE);
    }

    public void PlayAnimation(AniState aniState)
    {
        if (state == aniState) return;
        else state = aniState;

        switch (state)
        {
            case AniState.IDLE: animator.Play("Idle"); break;
            case AniState.WALK: animator.Play("Waking"); break;
            case AniState.RUN: animator.Play("Running"); break;
            case AniState.ATTACK: animator.Play("Punching"); break;
            case AniState.JUMP: animator.Play("Jump"); break;
            case AniState.ROLL: animator.Play("Rolling"); break;
        }
    }
}
