﻿using DG.Tweening;
using TomWill;
using UnityEngine;

public class MissileBM : DamageArea
{
    [SerializeField] private float projectileTimeToMove;
    private Collider2D collider;
    private SpriteRenderer alertProjectileSprite;
    private GameObject projectile;

    [SerializeField] private SpriteRenderer sign;
    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;

    private bool activeMissile;
    private bool deactiveMissileDashed;

    public void Launch(GameObject projectile, float timeToLaunch, bool activeMissile = true)
    {
        ParticleSystem smoke = projectile.transform.GetChild(2).GetComponent<ParticleSystem>();
        smoke.Play();
        deactiveMissileDashed = false;
        collider = GetComponent<Collider2D>();
        alertProjectileSprite = GetComponent<SpriteRenderer>();

        collider.enabled = false;
        alertProjectileSprite.enabled = true;

        this.projectile = projectile;
        this.activeMissile = activeMissile;
        if (activeMissile) projectile.GetComponent<Collider2D>().enabled = false;
        else projectile.GetComponent<Collider2D>().enabled = true;

        alertProjectileSprite.DOFade(0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetId("Alert" + transform.GetInstanceID());
        TWAudioController.PlaySFX("SFX_BOSS", "rocket_launch");
        DOVirtual.DelayedCall(timeToLaunch, OnEnter_State);
    }

    protected override void OnEnter_State()
    {
        base.OnEnter_State();
        projectile.transform.DOMove(transform.position, projectileTimeToMove).SetEase(Ease.InSine).OnComplete(() =>
        {
            DOTween.Kill("Alert" + transform.GetInstanceID());
            alertProjectileSprite.DOFade(1f, 0f);
            collider.enabled = activeMissile;

            if (!activeMissile)
            {
                ParticleSystem smoke = projectile.transform.GetChild(2).GetComponent<ParticleSystem>();
                smoke.Stop();
                projectile.GetComponent<Animator>().SetTrigger("Jammed");
                sign.enabled = true;
                projectile.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            OnExit_State();
        });
    }

    protected override void OnExit_State()
    {
        base.OnExit_State();

        DOVirtual.DelayedCall(0.2f, () => {
            collider.enabled = false;
            alertProjectileSprite.enabled = false;
        });
        if (activeMissile)
        {
            projectile.GetComponent<SpriteRenderer>().enabled = false;
            ParticleSystem particle = projectile.transform.GetChild(0).GetComponent<ParticleSystem>();
            particle.Play();

            TWAudioController.PlaySFX("SFX_BOSS", "rocket_impact");
            CameraShake.instance.Shake(duration, strength, vibrato);
            DOVirtual.DelayedCall(particle.main.startLifetimeMultiplier, () => { 
                Destroy(projectile);
            });
        }
    }

    public void DashDeactiveMissile()
    {
        sign.enabled = false;
        deactiveMissileDashed = true;
        ParticleSystem smoke = projectile.transform.GetChild(2).GetComponent<ParticleSystem>();
        smoke.Play();
    }

    public void Explode()
    {
        projectile.GetComponent<SpriteRenderer>().enabled = false;
        ParticleSystem particle = projectile.transform.GetChild(1).GetComponent<ParticleSystem>();
        particle.Play();

        BossBehaviour.Instance.TakeDamage();
        CameraShake.instance.Shake(duration, strength, vibrato);

        DOVirtual.DelayedCall(particle.main.startLifetimeMultiplier, () => {
            Destroy(projectile);
        });
    }

    public bool DeactiveMissileDashed()
    {
        sign.enabled = false;
        return !activeMissile && deactiveMissileDashed;
    }
}
