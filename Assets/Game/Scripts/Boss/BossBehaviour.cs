using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossBehaviour : MonoBehaviour
{
    protected int health;
    protected AttackPattern[] patterns;

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

    public UnityAction attackEvent; 
}
