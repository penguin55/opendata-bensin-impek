using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TomWill;
using UnityEngine;
using DG.Tweening;

public class Tidemaster : BossBehaviour
{

    enum State_Tidemaster
    {
        PREPARATION,
        FINAL,
        DIE,
        IDLE,
        TORPEDO_SHIELD,
        MISSILE,
        ENERGY_BALLS
    }

    private State_Tidemaster currentState;
    [SerializeField] private State_Tidemaster[] stateSequences;
    private int stateIndex;

    private void Start()
    {
        Instance = this;
        stateIndex = 0;
        currentState = State_Tidemaster.PREPARATION;

        Sprite = GetComponent<SpriteRenderer>();
        Init();
        UpdateState();
        InGameUI.instance.UpdateHpBos(health);
    }


    private void UpdateState()
    {
        switch (currentState)
        {
            case State_Tidemaster.PREPARATION:
                Preparation();
                break;
            case State_Tidemaster.IDLE:
                Idle();
                break;
            case State_Tidemaster.MISSILE:
                Missile();
                break;
            case State_Tidemaster.ENERGY_BALLS:
                Energy_Balls();
                break;
            case State_Tidemaster.TORPEDO_SHIELD:
                Torpedo_Shield();
                break;
            case State_Tidemaster.FINAL:
                Final();
                break;
            case State_Tidemaster.DIE:
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
    private void OnEnterTorpedo_Shield()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Torpedo").attackEvent;
    }

    private void Torpedo_Shield()
    {
        OnEnterTorpedo_Shield();
        currentAttackEvent.ExecutePattern(OnExitTorpedo_Shield);
    }

    private void OnExitTorpedo_Shield()
    {
        NextState();
    }
    #endregion

    #region LASER
    private void OnEnterEnergy_Balls()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Energy Balls").attackEvent;
    }

    private void Energy_Balls()
    {
        OnEnterEnergy_Balls();

        currentAttackEvent.ExecutePattern(OnExitEnergy_Balls);
    }

    private void OnExitEnergy_Balls()
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
