using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class GateKeeper_FlameThower : AttackEvent
{
    [SerializeField] private GameObject flameThowerParent;
    [SerializeField] private Transform damageArea;
    [SerializeField] private bool clockwise;
    [SerializeField] private float timeToRotate;
    [SerializeField] private float attackTime;

    [SerializeField] private ParticleSystem fires;

    [SerializeField] private Animator attack;


    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    public void Clockwise(bool active)
    {
        clockwise = active;
    }

    protected override void OnEnter_Attack()
    {
        flameThowerParent.SetActive(true);
        //ActivateDamageArea(true);
        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        ActivateDamageArea(true);
        ActivateDamageEffect(true);
        TWAudioController.PlaySFX("SFX_BOSS", "laser");

        //DOVirtual.DelayedCall(1, () => CameraShake.instance.Shake(1, 1, 2)).SetLoops(-1).SetId("ShakeLaser");

        //if (clockwise)
        //{
        //    flameThowerParent.transform.DORotate(new Vector3(0, 0, -360), timeToRotate, RotateMode.FastBeyond360);
        //}
        //else
        //{
        //    flameThowerParent.transform.DORotate(new Vector3(0, 0, 360), timeToRotate, RotateMode.FastBeyond360);
        //}

        DOTween.Sequence()
            .AppendCallback(() => { if (clockwise) flameThowerParent.transform.DORotate(new Vector3(0, 0, -360), timeToRotate, RotateMode.FastBeyond360).SetRelative(); })
            .AppendCallback(() => { if (!clockwise) flameThowerParent.transform.DORotate(new Vector3(0, 0, 360), timeToRotate, RotateMode.FastBeyond360).SetRelative(); })
            .AppendCallback(() => { CameraShake.instance.Shake(1, 1, 2); }).SetLoops(-1).SetId("ShakeLaser")
            .AppendInterval(attackTime)
            .AppendCallback(() =>
            {
                DOTween.Kill("ShakeLaser", true);
                ActivateDamageArea(false);
                base.Attack();
            });
    }

    protected override void OnExit_Attack()
    {
        flameThowerParent.SetActive(false);
        base.OnExit_Attack();
    }

    private void ActivateDamageArea(bool active)
    {
        damageArea.gameObject.SetActive(active);
        damageArea.GetComponent<Collider2D>().enabled = false;
    }


    private void ActivateDamageEffect(bool active)
    {
        if (active)
        {
            damageArea.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            damageArea.GetComponent<Collider2D>().enabled = false;
        }
    }
}
