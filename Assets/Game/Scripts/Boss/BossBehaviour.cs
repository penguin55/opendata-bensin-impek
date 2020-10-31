using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected AttackPattern[] patterns;

    protected AttackEvent currentAttackEvent;

    protected enum BossState
    {
        PREPARATION,
        FINAL,
        DIE
    }

    protected void Preparation()
    {

    }

    protected void Final()
    {

    }

    protected void Die()
    {

    }
}

[System.Serializable] 
public class AttackPattern
{
    public string attackName;

    public AttackEvent attackEvent; 
}
