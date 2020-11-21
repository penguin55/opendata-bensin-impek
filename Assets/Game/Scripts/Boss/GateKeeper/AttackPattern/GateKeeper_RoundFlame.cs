using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class GateKeeper_RoundFlame : AttackEvent
{
    [SerializeField] private GameObject laserParent;
    [SerializeField] private Transform damageArea;
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float timeToMove;
    [SerializeField] private Vector2 beginning, end;

    [SerializeField] private ParticleSystem fires;

    [SerializeField] private Animator attack;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        laserParent.SetActive(true);
        laserParent.transform.localScale = Vector2.Lerp(laserParent.transform.localScale, end, timeToMove * Time.deltaTime);
        ActivateDamageArea(true);
        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        ActivateDamageEffect();
        TWAudioController.PlaySFX("SFX_BOSS", "laser");

        DOVirtual.DelayedCall(1, () => CameraShake.instance.Shake(1, 1, 2)).SetLoops(-1).SetId("ShakeLaser");

        DOTween.Sequence()
            .Append(damageArea.DOMove(target.position + Vector3.up * offset.y, timeToMove))
            .OnComplete(() =>
            {
                DOTween.Kill("ShakeLaser");
                base.Attack();
            });
    }

    protected override void OnExit_Attack()
    {

        ActivateDamageArea(false);
        laserParent.transform.localScale = Vector2.Lerp(laserParent.transform.localScale,beginning, timeToMove * Time.deltaTime);
        laserParent.SetActive(false);
        base.OnExit_Attack();
    }

    private void ActivateDamageArea(bool active)
    {
            damageArea.gameObject.SetActive(active);
            damageArea.GetComponent<Collider2D>().enabled = false;
     }
    

    private void ActivateDamageEffect()
    {

        fires.Play();
        damageArea.GetComponent<Collider2D>().enabled = true;
        
    }
}
