using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class MissileTM : DamageArea
{
    [SerializeField] private float projectileTimeToMove;
    private Collider2D collider;
    private SpriteRenderer alertProjectileSprite;
    private GameObject projectile;
    [SerializeField] private float timer;

    [SerializeField] private SpriteRenderer sign;
    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;

    private bool deactiveMissileDashed;
    private Tidemaster_Missile.SpawnProjectileArea spawnAreaData;
    private UnityAction onExplodeCallback;

    public void InitData(Tidemaster_Missile.SpawnProjectileArea spawnAreaData)
    {
        this.spawnAreaData = spawnAreaData;
    }

    public void OnExplodeCallback(UnityAction action)
    {
        onExplodeCallback = action;
    }

    public void Launch(GameObject projectile, float timeToLaunch)
    {
        ParticleSystem smoke = projectile.transform.GetChild(2).GetComponent<ParticleSystem>();
        smoke.Play();
        deactiveMissileDashed = false;
        collider = GetComponent<Collider2D>();
        alertProjectileSprite = GetComponent<SpriteRenderer>();
        collider.enabled = false;
        sign.enabled = false;
        alertProjectileSprite.enabled = true;
        this.projectile = projectile;
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
            ParticleSystem smoke = projectile.transform.GetChild(2).GetComponent<ParticleSystem>();
            smoke.Stop();
            projectile.GetComponent<Animator>().SetTrigger("Jammed");
            sign.enabled = true;
            projectile.GetComponent<SpriteRenderer>().sortingOrder = 13;

            CameraShake.instance.Shake(duration, strength, vibrato);

            DOVirtual.DelayedCall(timer, () => { if (!deactiveMissileDashed) OnExit_State(); }).SetId("Timer_Missile");
            
        });
    }

    protected override void OnExit_State()
    {
        base.OnExit_State();

        sign.enabled = false;

        DOVirtual.DelayedCall(0.2f, () =>
        {
            alertProjectileSprite.enabled = false;
        });
        projectile.GetComponent<SpriteRenderer>().enabled = false;
        ParticleSystem particle = projectile.transform.GetChild(0).GetComponent<ParticleSystem>();
        particle.Play();
        
        TWAudioController.PlaySFX("SFX_BOSS", "rocket_impact");
        CameraShake.instance.Shake(duration, strength, vibrato);

        spawnAreaData.platform.SetActive(true);
        spawnAreaData.cannon.TakeDamage();
        CharaController.instance.TakeDamage();

        onExplodeCallback.Invoke();

        DOVirtual.DelayedCall(particle.main.startLifetimeMultiplier, () =>
        {
            Destroy(projectile);
        });
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
        alertProjectileSprite.enabled = false;
        DOTween.Kill("Timer_Missile");
        Destroy(projectile);
    }

    public bool DeactiveMissileDashed()
    {
        sign.enabled = false;
        return deactiveMissileDashed;
    }
}
