using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackEvent : MonoBehaviour
{
    [SerializeField] protected float delay_enter, delay_exit;
    [SerializeField] protected float fireRate;

    protected UnityAction onCompleteAction;

    public virtual void ExecutePattern(UnityAction onComplete)
    {
        onCompleteAction = onComplete;
    }
}
