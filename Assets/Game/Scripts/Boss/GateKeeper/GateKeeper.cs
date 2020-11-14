using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateKeeper : BossBehaviour
{
    enum State_BirdMask
    {
        PREPARATION,
        FINAL,
        DIE,
        IDLE,
        FLAMETHOWER,
        CANNON,
        ROUND_FLAME,
        CLOCKWISE,
        COUNTER_CLOCKWISE
    }

    private State_BirdMask currentState;
    [SerializeField] private State_BirdMask[] stateSequences;
    private int stateIndex;

    private void Start()
    {
        Instance = this;
        stateIndex = 0;
        currentState = State_BirdMask.CANNON;

        Sprite = GetComponent<SpriteRenderer>();
        DefaultMaterial = Sprite.material;

        UpdateState();
        InGameUI.instance.UpdateHpBos(health);
    }


    private void UpdateState()
    {
        switch (currentState)
        {
            case State_BirdMask.PREPARATION:
                Preparation();
                break;
            case State_BirdMask.IDLE:
                Idle();
                break;
            case State_BirdMask.CANNON:
                Cannon();
                break;
            case State_BirdMask.ROUND_FLAME:
                RoundFlame();
                break;
            case State_BirdMask.CLOCKWISE:
                FlameThower(true);
                break;
            case State_BirdMask.COUNTER_CLOCKWISE:
                FlameThower(false);
                break;
            case State_BirdMask.FINAL:
                Final();
                break;
            case State_BirdMask.DIE:
                Die();
                break;
        }
    }



    private void NextState()
    {
        if (!GameVariables.GAME_OVER)
        {
            stateIndex++;
            currentState = stateSequences[stateIndex];
            UpdateState();
        }
    }

    #region IDLE
    private void OnEnterIdle()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Cannon").attackEvent;
    }

    private void Idle()
    {
        OnEnterIdle();
        currentAttackEvent.ExecutePattern(OnExitIdle);
    }

    private void OnExitIdle()
    {
        NextState();
    }
    #endregion

    #region CANNON
    private void OnEnterCannon()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Cannon").attackEvent;
    }

    private void Cannon()
    {
        OnEnterCannon();

        currentAttackEvent.ExecutePattern(OnExitCannon);
    }

    private void OnExitCannon()
    {
        NextState();
    }
    #endregion

    #region ROUND_FLAME
    private void OnEnterRoundFlame()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Round_Flame").attackEvent;
    }

    private void RoundFlame()
    {
        OnEnterRoundFlame();
        currentAttackEvent.ExecutePattern(OnExitRoundFlame);
    }

    private void OnExitRoundFlame()
    {
        NextState();
    }
    #endregion

    #region FLAMETHOWER
    private void OnEnterFlameThower()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "FlameThower").attackEvent;
    }

    private void FlameThower(bool clockwise)
    {
        OnEnterFlameThower();
        if (!clockwise) ((BirdMask_Swipe)currentAttackEvent).MoveToLeft();

        currentAttackEvent.ExecutePattern(OnExitFlameThower);
    }

    private void OnExitFlameThower()
    {
        NextState();
    }
    #endregion


    protected override void Final()
    {
        base.Final();

        stateIndex = 0;
        currentState = stateSequences[stateIndex];
        UpdateState();
    }
}
