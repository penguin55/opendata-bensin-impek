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

        DOVirtual.DelayedCall(timeToLaunch, OnEnter_State);
    }

    protected override void OnEnter_State()
    {
        base.OnEnter_State();

        projectile.transform.DOMove(transform.position, projectileTimeToMove).SetEase(Ease.InSine).OnComplete( () =>
        {
            collider.enabled = true;
            OnExit_State();
        });
    }

    protected override void OnExit_State()
    {
        base.OnExit_State();

        alertProjectileSprite.enabled = false;
        collider.enabled = false;
        if (activeMissile) Destroy(projectile);
    }
}
