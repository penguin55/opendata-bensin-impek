using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public static BossBehaviour Instance;

    [SerializeField] protected int health;
    [SerializeField] protected AttackPattern[] patterns;

    protected AttackEvent currentAttackEvent;

    protected virtual void Preparation()
    {

    }

    protected virtual void Final()
    {

    }

    protected virtual void Die()
    {

    }
}

[System.Serializable] 
public class AttackPattern
{
    public string attackName;

    public AttackEvent attackEvent; 
}
