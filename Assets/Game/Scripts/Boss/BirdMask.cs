using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMask : BossBehaviour
{
    enum State_BirdMask
    {
        IDLE,
        MACHINEGUN,
        MISSILE,
        SWIPE
    }

    private System.Enum currentState;
    private System.Enum [] stateSequences;
    private int stateIndex;

    private void UpdateState()
    {
        switch (currentState)
        {
            case BossState.PREPARATION:
                break;
            case State_BirdMask.IDLE:
                Idle();
                break;
            case State_BirdMask.MISSILE:
                Missile();
                break;
            case State_BirdMask.MACHINEGUN:
                MachineGun();
                break;
            case State_BirdMask.SWIPE:
                Swipe();
                break;
            case BossState.FINAL:
                stateIndex = 0;
                break;
        }
    }



    private void NextState()
    {
        stateIndex++;
        currentState = stateSequences[stateIndex];
    }

    private void OnEnterIdle()
    {

    }

    private void OnExitIdle()
    {

    }


    private void Idle()
    {
        OnEnterIdle();
        OnExitIdle();
    }

    private void OnEnterMissile()
    {

    }

    private void OnExitMissile()
    {

    }

    private void Missile()
    {
        OnEnterMissile();
        OnExitMissile();
    }
    private void OnEnterMachineGun()
    {

    }

    private void OnExitMachineGun()
    {

    }

    private void MachineGun()
    {
        OnEnterMachineGun();
        OnExitMachineGun();
    }

    private void OnEnterSwipe()
    {

    }

    private void OnExitSwipe()
    {

    }

    private void Swipe()
    {
        OnEnterSwipe();
        OnExitSwipe();
    }

}



