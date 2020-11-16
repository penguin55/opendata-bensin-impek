using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class AttackEvent : MonoBehaviour
{
    [SerializeField] protected float delay_enter, delay_prepare, delay_animation, delay_active, delay_exit;
    [SerializeField] protected Animator anim;

    protected UnityAction onCompleteAction;

    public virtual void ExecutePattern(UnityAction onComplete)
    {
        onCompleteAction = onComplete;
        DOVirtual.DelayedCall(delay_enter, OnPrepare_Attack);
    }

    protected virtual void OnPrepare_Attack()
    {
        //anim.gameObject.SetActive(true);
        DOTween.Sequence()
            .AppendCallback(() => { anim.gameObject.SetActive(true); })
            .AppendCallback(() => { TWAudioController.PlaySFX("BOSS_SFX", "boss_attack_telegraph"); })
            .AppendInterval(delay_animation)
            .AppendCallback(() => anim.gameObject.SetActive(false))
            .AppendInterval(delay_prepare)
            .AppendCallback(() => OnEnter_Attack());

        //DOVirtual.DelayedCall(delay_prepare, () =>
        //{
        //    anim.gameObject.SetActive(false);
        //    OnEnter_Attack();
        //});
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
