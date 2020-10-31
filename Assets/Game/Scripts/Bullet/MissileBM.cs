using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBM : DamageArea
{
    [SerializeField] private float projectileTimeToMove;
    private Collider2D collider;
    private SpriteRenderer alertProjectileSprite;
    private GameObject projectile;

    private bool activeMissile;

    public void Launch(GameObject projectile, float timeToLaunch, bool activeMissile = true)
    {
        collider = GetComponent<Collider2D>();
        alertProjectileSprite = GetComponent<SpriteRenderer>();

        collider.enabled = false;
        alertProjectileSprite.enabled = true;

        this.projectile = projectile;
        this.activeMissile = activeMissile;
        if (activeMissile) projectile.GetComponent<Collider2D>().enabled = false;
        else projectile.GetComponent<Collider2D>().enabled = true;

        alertProjectileSprite.DOFade(0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetId("Alert"+transform.GetInstanceID());

        DOVirtual.DelayedCall(timeToLaunch, OnEnter_State);
    }

    protected override void OnEnter_State()
    {
        base.OnEnter_State();
        DOVirtual.DelayedCall(projectileTimeToMove/2f, () => collider.enabled = activeMissile);
        projectile.transform.DOMove(transform.position, projectileTimeToMove).SetEase(Ease.InSine).OnComplete( () =>
        {
            DOTween.Kill("Alert" + transform.GetInstanceID());
            alertProjectileSprite.DOFade(1f, 0f);
            if (!activeMissile)
            {
                projectile.GetComponent<Animator>().SetTrigger("Jammed");
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
        if (activeMissile) Destroy(projectile);
    }

    public void Explode()
    {
        Destroy(projectile);
    }
}
