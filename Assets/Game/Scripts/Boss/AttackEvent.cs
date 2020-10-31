using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class AttackEvent : MonoBehaviour
{
    [SerializeField] protected float delay_enter, delay_active, delay_exit;

    protected UnityAction onCompleteAction;

    public virtual void ExecutePattern(UnityAction onComplete)
    {
        onCompleteAction = onComplete;
        DOVirtual.DelayedCall(delay_enter, OnEnter_Attack);
    }

    protected virtual void OnEnter_Attack()
    {
        DOVirtual.DelayedCall(delay_active, Attack);
    }

    protected virtual void Attack()
    {
        DOVirtual.DelayedCall(delay_exit, OnExit_Attack);
    }

    protected virtual void OnExit_Attack()
    {
        onCompleteAction.Invoke();
    }
}
