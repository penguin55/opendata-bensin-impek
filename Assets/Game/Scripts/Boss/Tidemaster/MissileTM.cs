using DG.Tweening;
using Fungus;
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
    //public bool exploded;
    private Tidemaster_Missile.SpawnProjectileArea spawnAreaData;
    private UnityAction onExplodeCallback;

    private bool isBusyDash;

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
        //exploded = false;
        deactiveMissileDashed = false;
        collider = GetComponent<Collider2D>();
        alertProjectileSprite = GetComponent<SpriteRenderer>();
        collider.enabled = false;
        sign.enabled = false;
        alertProjectileSprite.enabled = true;
        this.projectile = projectile;
        alertProjectileSprite.DOFade(0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetId("Alert" + transform.GetInstanceID());
        TWAudioController.PlaySFX("SFX_BOSS", "bigrocket_launch");
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

            DOVirtual.DelayedCall(timer, () =>
            {
                if (!deactiveMissileDashed) OnExit_State();
            }).SetId("MissileTM");
            
        });
    }

    protected override void OnExit_State()
    {
        base.OnExit_State();
        isBusyDash = true;
        sign.enabled = false;

        DOVirtual.DelayedCall(0.2f, () =>
        {
            alertProjectileSprite.enabled = false;
        });
        projectile.GetComponent<SpriteRenderer>().enabled = false;
        ParticleSystem particle = projectile.transform.GetChild(0).GetComponent<ParticleSystem>();
        particle.Play();
        
        TWAudioController.PlaySFX("SFX_BOSS", "bigrocket_explosion");
        CameraShake.instance.Shake(duration, strength, vibrato);
        //exploded = true;
        spawnAreaData.platform.SetActive(true);
        spawnAreaData.cannon.TakeDamage();

        if (CheckIsCloseDistance())
        {
            CharaController.instance.TakeDamage();
        }

        onExplodeCallback.Invoke();

        DOVirtual.DelayedCall(particle.main.startLifetimeMultiplier, () =>
        {
            Destroy(projectile);
        });
    }

    public void DashDeactiveMissile()
    {
        if (!isBusyDash)
        {
            //exploded = false;
            sign.enabled = false;
            deactiveMissileDashed = true;
            alertProjectileSprite.enabled = false;
            ParticleSystem smoke = projectile.transform.GetChild(2).GetComponent<ParticleSystem>();
            smoke.Play();
        }
    }

    public void Explode()
    {
        projectile.SetActive(false);
        Destroy(projectile,2f);
    }

    public bool DeactiveMissileDashed()
    {
        sign.enabled = false;
        return deactiveMissileDashed;
    }

    private bool CheckIsCloseDistance()
    {
        float distance = Mathf.Sqrt((CharaController.instance.transform.position - transform.position).sqrMagnitude);
        return distance <= 2f;
    }
}
