using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected UnityAction onCompleteAction;

    protected virtual void OnEnter_State()
    {

    }

    protected virtual void OnExit_State()
    {

    }
}
