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

    [System.Serializable]
    public struct DataGerbong
    {
        public string name;
        public GameObject gerbong;
        public Animator anim;
        public bool dead;
    }

    public ChariotTweening animationTweening;
    [SerializeField] private List<DataGerbong> dataGerbong;
    private DataGerbong activeGerbong;

    private State_Chariot currentState;
    [SerializeField] private State_Chariot [] stateSequences;
    private int stateIndex;

    private void Start()
    {
        Instance = this;
        stateIndex = 0;
        currentState = State_Chariot.PREPARATION;
        activeGerbong = dataGerbong[0];

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
        currentAttackEvent = patterns.First(e => e.attackName == "StrafingMachinegun").attackEvent;
        (currentAttackEvent as Chariot_Machinegun).InitialitationPattern(activeGerbong);
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