using System.Collections.Generic;
using System.Linq;
using TomWill;
using UnityEngine;
using DG.Tweening;

public class Chariot : BossBehaviour
{
    enum State_Chariot
    {
        PREPARATION,
        FINAL,
        DIE,
        IDLE,
        MACHINEGUN,
        MISSILE,
        POISONZONE
    }

    private State_Chariot currentState;
    [SerializeField] private State_Chariot [] stateSequences;
    private int stateIndex;

    private void Start()
    {
        Instance = this;
        stateIndex = 0;
        currentState = State_Chariot.PREPARATION;

        Sprite = GetComponent<SpriteRenderer>();
        Init();
        
        UpdateState();
        InGameUI.instance.UpdateHpBos(health);
    }


    private void UpdateState()
    {
        switch (currentState)
        {
            case State_Chariot.PREPARATION:
                Preparation();
                break;
            case State_Chariot.IDLE:
                Idle();
                break;
            case State_Chariot.MISSILE:
                Missile();
                break;
            case State_Chariot.POISONZONE:
                PoisonZone();
                break;
            case State_Chariot.MACHINEGUN:
                MachineGun();
                break;
            case State_Chariot.FINAL:
                Final();
                break;
            case State_Chariot.DIE:
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

    protected override void Preparation()
    {
        base.Preparation();
        NextState();
    }

    protected override void Die()
    {
        base.Die();
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

    #region PoisonZone
    private void OnEnterPoisonZone()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Laser").attackEvent;
    }

    private void PoisonZone()
    {
        OnEnterPoisonZone();
        currentAttackEvent.ExecutePattern(OnExitPoisonZone);
    }

    private void OnExitPoisonZone()
    {
        NextState();
    }
    #endregion

    protected override void Final()
    {
        base.Final();

        stateIndex = 1;
        currentState = stateSequences[stateIndex];
        UpdateState();
    }
}
