using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirdMask : BossBehaviour
{
    enum State_BirdMask
    {
        PREPARATION,
        FINAL,
        DIE,
        IDLE,
        MACHINEGUN,
        MISSILE,
        LASER,
        SWIPE_TO_RIGHT,
        SWIPE_TO_LEFT
    }

    private State_BirdMask currentState;
    [SerializeField] private State_BirdMask [] stateSequences;
    private int stateIndex;

    private void Start()
    {
        Instance = this;
        stateIndex = 0;
        currentState = State_BirdMask.MISSILE;

        UpdateState();
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
            case State_BirdMask.MISSILE:
                Missile();
                break;
            case State_BirdMask.LASER:
                Laser();
                break;
            case State_BirdMask.MACHINEGUN:
                MachineGun();
                break;
            case State_BirdMask.SWIPE_TO_RIGHT:
                Swipe(true);
                break;
            case State_BirdMask.SWIPE_TO_LEFT:
                Swipe(false);
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
        currentAttackEvent = patterns.First(e => e.attackName == "Missile").attackEvent;
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

    #region MISSILE
    private void OnEnterMissile()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Missile").attackEvent;
    }

    private void Missile()
    {
        OnEnterMissile();

        currentAttackEvent.ExecutePattern(OnExitMissile);
    }

    private void OnExitMissile()
    {
        NextState();
    }
    #endregion

    #region MACHINEGUN
    private void OnEnterMachineGun()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Machinegun").attackEvent;
    }

    private void MachineGun()
    {
        OnEnterMachineGun();
        currentAttackEvent.ExecutePattern(OnExitMachineGun);
    }

    private void OnExitMachineGun()
    {
        NextState();
    }
    #endregion

    #region SWIPE
    private void OnEnterSwipe()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Swipe").attackEvent;
    }

    private void Swipe(bool moveRight)
    {
        OnEnterSwipe();
        if (!moveRight) ((BirdMask_Swipe)currentAttackEvent).MoveToLeft();

        currentAttackEvent.ExecutePattern(OnExitSwipe);
    }

    private void OnExitSwipe()
    {
        NextState();
    }
    #endregion

    #region LASER
    private void OnEnterLaser()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Laser").attackEvent;
    }

    private void Laser()
    {
        OnEnterLaser();

        currentAttackEvent.ExecutePattern(OnExitLaser);
    }

    private void OnExitLaser()
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
