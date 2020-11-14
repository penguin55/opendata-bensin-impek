using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class AttackEvent : MonoBehaviour
{
    [SerializeField] protected float delay_enter, delay_prepare, delay_active, delay_exit;
    [SerializeField] protected Animator anim;

    protected UnityAction onCompleteAction;

    public virtual void ExecutePattern(UnityAction onComplete)
    {
        onCompleteAction = onComplete;
        DOVirtual.DelayedCall(delay_enter, OnPrepare_Attack);
    }

    protected virtual void OnPrepare_Attack()
    {
        anim.gameObject.SetActive(true);
        DOVirtual.DelayedCall(delay_prepare, () =>
        {
            anim.gameObject.SetActive(false);
            OnEnter_Attack();
        });
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
