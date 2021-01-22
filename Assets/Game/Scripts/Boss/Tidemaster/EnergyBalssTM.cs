using DG.Tweening;
using TomWill;
using UnityEngine;

public class EnergyBalssTM : DamageArea
{
    [SerializeField] private float projectileTimeToMove;
    private Collider2D collider;  
    private GameObject projectile;
    private Animator anim;
    [SerializeField] private AnimationClip explodeClip;
    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;

    private bool deactiveMissileDashed;

    public void Launch(GameObject projectile, float timeToLaunch)
    {
        ParticleSystem smoke = projectile.transform.GetChild(2).GetComponent<ParticleSystem>();
        smoke.Play();
        deactiveMissileDashed = false;
        anim = projectile.GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        this.projectile = projectile;
        projectile.GetComponent<Collider2D>().enabled = true;
        TWAudioController.PlaySFX("SFX_BOSS", "energyball_launch");
        DOVirtual.DelayedCall(timeToLaunch, OnEnter_State);
    }

    protected override void OnEnter_State()
    {

        base.OnEnter_State();
        projectile.transform.DOMove(transform.position, projectileTimeToMove).SetEase(Ease.InSine).OnComplete(() =>
        {
            DOTween.Kill("Alert" + transform.GetInstanceID());
            collider.enabled = true;
            OnExit_State();
        });
    }

    protected override void OnExit_State()
    {
        base.OnExit_State();

        anim.SetTrigger("Explode");

        DOVirtual.DelayedCall(0.2f, () =>
        {
            collider.enabled = false;
        });
        //projectile.GetComponent<SpriteRenderer>().enabled = false;
        //ParticleSystem particle = projectile.transform.GetChild(0).GetComponent<ParticleSystem>();
        //particle.Play();
        TWAudioController.PlaySFX("SFX_BOSS", "energyball_impact");
        CameraShake.instance.Shake(duration, strength, vibrato);
        DOVirtual.DelayedCall(explodeClip.length, () =>
        {
            Destroy(projectile);
        });

    }
}
