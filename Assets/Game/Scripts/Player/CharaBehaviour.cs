using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using TomWill;
using UnityEngine;

public class CharaBehaviour : MonoBehaviour
{
    [SerializeField] private bool usingBike;
    [SerializeField] protected CharaData data;
    [SerializeField] protected Vector2 direction, lastDirection;
    [SerializeField] protected float startDashTime, dashTime;

    protected bool isDashed,immune, canDash, insight = false;
    public bool dead;
    protected float dashDelay;
    [SerializeField] protected bool isCharged = false;
    [SerializeField] protected Rigidbody2D rb;

    protected SpriteRenderer sprite;
    protected Material defaultMaterial;
    [SerializeField] protected Material whiteflash;
    [SerializeField] protected float flashDelay;

    [SerializeField] protected float timeMoveElapsed;
    [SerializeField] protected bool isAccelerating;
    [SerializeField] protected float timeToStop;

    public GameObject kiriatas, kananbawah;
    float minX, maxX, minY, maxY;
    float posisilamaX, posisilamaY;
    float posisibaruX, posisibaruY;

    [SerializeField] protected Animator anim;
    [SerializeField] protected ParticleSystem walkDustParticle,walkDustParticle2, dashDustParticle;

    [SerializeField] private CharaInteract interact;
    [SerializeField] private bool clamp;
    [SerializeField] private Transform placeholderHand;
    [SerializeField] private GameObject markDown;
    [SerializeField] private ParticleSystem markdownParticle;


    public void Init()
    {
        GameVariables.EFFECT_IMMUNE = false;
        GameVariables.PLAYER_IMMUNE = false;
        if (GameData.ActiveItem) GameData.ActiveItem.ResetStatusItem();
        data.ResetStatus();
        data.DashSpeed = data.BaseDashSpeed;
        Time.timeScale = 1f;
        dashDelay = data.BaseDashDelay;
        GameVariables.STILL_ALIVE = true;
        canDash = true;
        dead = false;
        data.Hp = 3;
        GameTime.PlayerTimeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
        defaultMaterial = sprite.material;
        anim = GetComponent<Animator>();
        minX = kiriatas.transform.position.x;
        maxX = kananbawah.transform.position.x;

        maxY = kiriatas.transform.position.y;
        minY = kananbawah.transform.position.y;

        if (GameData.ActiveItem && GameData.ActiveItem.ActivateOnStart)
        {
            UseItem();
        }

        InGameUI.instance?.UpdateLive();
        InGameUI.instance?.UpdateShield();
        switch (GameData.ActiveBoss)
        {
            case GameData.BossType.UNHOLYCHARIOT:
                walkDustParticle2.Play();
                walkDustParticle.Play();
                break;
            default:
                break;
        }
    }

    protected void Clamp()
    {
        posisilamaX = this.transform.position.x;

        posisibaruX = Mathf.Clamp(posisilamaX, minX, maxX);

        posisilamaY = this.transform.position.y;

        posisibaruY = Mathf.Clamp(posisilamaY, minY, maxY);
        transform.position = new Vector3(posisibaruX, posisibaruY, 0f);
    }

    public void InvisibleFrame(float value)
    {
        Color temp = sprite.color;
        temp.a = value;
        sprite.color = temp;
    }

    public void MarkDown(bool flag)
    {
        if (flag)
        {
            markDown.transform.position = transform.position;
            markDown.SetActive(true);
        }
        else if (!flag)
        {
            transform.position = markDown.transform.position;
            markdownParticle.gameObject.SetActive(true);
            markdownParticle.Play();
            DOVirtual.DelayedCall(markdownParticle.main.startLifetimeMultiplier, () =>
            {
                markdownParticle.gameObject.SetActive(false);
                markDown.SetActive(false);
            });
        }
    }

    public void ImmuneFrame(bool flag)
    {
        if (flag)
        {
            DOTween.Sequence()
                .AppendCallback(()=> sprite.material = whiteflash)
                .AppendInterval(flashDelay)
                .AppendCallback(() => { sprite.material = defaultMaterial; })
                .AppendInterval(flashDelay)
                .SetLoops(-1, LoopType.Restart)
                .SetId("ImmuneFrame");
        }
        else
        {
            DOTween.Kill("ImmuneFrame");
            sprite.material = defaultMaterial;
        }
    }

    protected void MoveAccelerate()
    {
        if (isDashed) return;

        if (isAccelerating)
        {
            if (!walkDustParticle.isPlaying) walkDustParticle.Play();
            Movement(1);
        }
        else
        {
            Stop();
            Movement(timeMoveElapsed / timeToStop);
        }
    }

    protected void Movement(float accelerate)
    {
        switch (GameData.ActiveBoss)
        {
            case GameData.BossType.UNHOLYCHARIOT:
                break;
            default:
                if (direction == Vector2.zero) walkDustParticle.Stop();
                break;
        }
        
        float newSpeed = data.Speed * (GameVariables.SPEED_BUFF > 0 ? GameVariables.SPEED_BUFF : 1);
        transform.Translate(direction * newSpeed * GameTime.PlayerTime * accelerate);
    }


    private void Stop()
    {
        timeMoveElapsed -= GameTime.PlayerTime;

        if (timeMoveElapsed <= 0) timeMoveElapsed = 0;
    }

    protected void Dash()
    {
        if (lastDirection != Vector2.zero && isDashed)
        {
            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                isDashed = false;
                isCharged = false;
                data.IsDashing = false;
                anim.SetBool("dash", false);
                dashDustParticle.Stop();
                GameVariables.PLAYER_IMMUNE = false;
                data.ChargeTimeDash = 0f;
                /*DOVirtual.DelayedCall(dashDelay, () => canDash = true).timeScale = GameTime.PlayerTimeScale;
                DOVirtual.DelayedCall(dashDelay, () => data.CanChargeDash = true).timeScale = GameTime.PlayerTimeScale;*/
                StartCoroutine(DashRecover());
            }
            else
            {
                anim.SetBool("dash", true);
                if (walkDustParticle.isPlaying) walkDustParticle.Stop();
                dashDustParticle.Play();
                timeMoveElapsed = 0f;
                dashTime -= GameTime.PlayerTime;
                rb.velocity = lastDirection * data.DashSpeed;
                Debug.Log(data.DashSpeed);

                interact.DashingProjectile(lastDirection, data.DashSpeed);
                interact.DashingGun();
                interact.DashingButtonInteract();
                interact.DashingDynamite();
                interact.DashingMissileTidemaster();
                interact.DashingCannonTidemaster();
            }
        }
    }

    IEnumerator DashRecover()
    {
        yield return new WaitForSeconds(dashDelay);
        canDash = true;
        data.CanChargeDash = true;
        data.DashSpeed = data.BaseDashSpeed;
    }

    public bool PlayerDashing()
    {
        return data.IsDashing;
    }


    public void TakeDamage()
    {
        if (GameData.ActiveBossData.wasDie) return; 

        if (!(GameVariables.PLAYER_IMMUNE || GameVariables.EFFECT_IMMUNE))
        {
            TWAudioController.PlaySFX("SFX_PLAYER", "player_damaged");
            sprite.material = whiteflash;
            DOVirtual.DelayedCall(flashDelay, () => { sprite.material = defaultMaterial; }).timeScale = GameTime.PlayerTimeScale;

            if (data.Hp >= 1)
            {
                GameVariables.PLAYER_IMMUNE = true;
                DOTween.Kill("ImmuneDamage");
                DOVirtual.DelayedCall(2f, () => { GameVariables.PLAYER_IMMUNE = false; }).SetId("ImmuneDamage").timeScale = GameTime.PlayerTimeScale;

                if (data.Shield > 0)
                {
                    data.Shield -= 1;
                    InGameUI.instance.UpdateShield();
                }
                else data.Hp -= 1;

                InGameUI.instance.UpdateLive();

                if (data.Hp < 1)
                {
                    GameTrackRate.DeathCount = 1;
                    GameTrackRate.EndTimePlay = Time.time;
                    GameTrackRate.CalculateTime();

                    GameData.ShiftItemList();
                    dead = true;
                    anim.SetBool("dead", true);
                    GameVariables.STILL_ALIVE = false;
                    GameVariables.GAME_OVER = true;
                    TWTransition.ScreenFlash(1, 0.1f);
                    DOTween.Sequence()
                        .AppendInterval(1f)
                        .AppendCallback(() =>
                        {InGameUI.instance.GameOver();});
                }
            }
        }
    }

    

    public void UpdateAnimationWalk(float x, float y, float speed)
    {
        if (!usingBike)
        {
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);
            anim.SetFloat("speed", speed);
        }
    }

    protected void UseItem()
    {
        if (!GameData.ActiveItem.wasUsed && GameData.ActiveItem.CheckIsOneTimeUse())
        {
            TWAudioController.PlaySFX("BOSS_SFX", "item_used");
            GameData.ActiveItem.TakeEffect();
            InGameUI.instance.UpdateItemImage();

            GameData.ShiftItemList();
        }
        else if (!GameData.ActiveItem.CheckIsOneTimeUse())
        {
            if (GameData.ActiveItem.TakeEffect())
            {
                TWAudioController.PlaySFX("BOSS_SFX", "item_used");
                InGameUI.instance.UpdateItemImage();
            }
        }
    }

    public void SetDashDelay(float time)
    {
        dashDelay = time;
    }

    public float GetDashDelay()
    {
        return dashDelay;
    }

    public Vector3 GetHandPlaceholder()
    {
        return placeholderHand.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("projectiles"))
        {
            interact.projectileDetect = collision.gameObject;
        }

        if (collision.CompareTag("damage area") && !GameVariables.PLAYER_IMMUNE)
        {
            TakeDamage();
            collision.GetComponent<CannonGK>()?.Explode();
        }

        if (collision.CompareTag("button_interact"))
        {
            interact.buttonInteract = collision.gameObject;
        }

        if (collision.CompareTag("poison"))
        {
            TakeDamage();
        }

        if (collision.CompareTag("dynamite"))
        {
            interact.SetDynamite(collision.gameObject);
            interact.dynamiteDetect.GetComponent<DynamiteChariot>().GetDynamite();
            interact.dynamiteDetect.transform.parent = placeholderHand;
            interact.dynamiteDetect.transform.localPosition = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interact.projectileDetect && collision.CompareTag("projectiles"))
        {
            interact.projectileDetect = null;
        }

        if (interact.buttonInteract && collision.CompareTag("button_interact"))
        {
            interact.buttonInteract = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("gun"))
        {
            interact.gunDetect = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("cannon"))
        {
            interact.cannonTidemasterDetect = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("missile_tidemaster"))
        {
            interact.missileTidemasterDetect = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (interact.gunDetect && collision.gameObject.CompareTag("gun"))
        {
            interact.gunDetect = null;
        }

        if (interact.cannonTidemasterDetect && collision.gameObject.CompareTag("cannon"))
        {
            interact.cannonTidemasterDetect = null;
        }

        if (interact.missileTidemasterDetect && collision.gameObject.CompareTag("missile_tidemaster"))
        {
            interact.missileTidemasterDetect = null;
        }
    }
}
   
