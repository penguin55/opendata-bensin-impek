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
        FINAL
    }

}

[System.Serializable] 
public class AttackPattern
{
    public string attackName;

    public UnityAction attackEvent; 
}
