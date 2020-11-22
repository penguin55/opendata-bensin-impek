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
    [SerializeField] private float timeToScale, attackTime;
    [SerializeField] private float beginning, end;

    [SerializeField] private ParticleSystem fires;

    [SerializeField] private Animator attack;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        laserParent.SetActive(true);

        laserParent.transform.DOScale(Vector2.one * end, timeToScale);
        ActivateDamageArea(true);
        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        ActivateDamageEffect(true);
        TWAudioController.PlaySFX("SFX_BOSS", "laser");

        DOVirtual.DelayedCall(1, () => CameraShake.instance.Shake(1, 1, 2)).SetLoops(-1).SetId("ShakeLaser");
        DOVirtual.DelayedCall(attackTime, () => {
            DOTween.Kill("ShakeLaser", true);
            ActivateDamageEffect(false);
            base.Attack();
        } );
    }

    protected override void OnExit_Attack()
    {
        laserParent.transform.DOScale(Vector2.one * beginning, timeToScale).OnComplete(()=> {
            ActivateDamageArea(false);
            laserParent.SetActive(false);
            base.OnExit_Attack();
        });
        
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
            fires.Play();
            damageArea.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            fires.Stop();
            damageArea.GetComponent<Collider2D>().enabled = false;
        }
        
    }
}
