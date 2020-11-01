using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

public class CharaBehaviour : MonoBehaviour
{
    [SerializeField] protected CharaData data;
    [SerializeField] protected Vector2 direction, lastDirection;
    [SerializeField] protected float startDashTime, dashTime;
    [SerializeField] protected float kickProjectilesTime, kickTime;

    protected bool isDashed,immune, canDash, insight = false;
    public bool dead;
    [SerializeField] protected float dashDelay;
    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] protected float timeMoveElapsed;
    [SerializeField] protected bool isAccelerating;
    [SerializeField] protected float timeToStop;

    public GameObject kiriatas, kananbawah;
    float minX, maxX, minY, maxY;
    float posisilamaX, posisilamaY;
    float posisibaruX, posisibaruY;

    [SerializeField] protected Animator anim;
    [SerializeField] protected ParticleSystem walkDustParticle, dashDustParticle;

    [SerializeField] private GameObject projectileDetect;


    public void Init()
    {
        GameVariables.STILL_ALIVE = true;
        canDash = true;
        dead = false;
        data.Hp = 3;
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        minX = kiriatas.transform.position.x;
        maxX = kananbawah.transform.position.x;

        maxY = kiriatas.transform.position.y;
        minY = kananbawah.transform.position.y;
    }

    protected void Clamp()
    {
        posisilamaX = this.transform.position.x;

        posisibaruX = Mathf.Clamp(posisilamaX, minX, maxX);

        posisilamaY = this.transform.position.y;

        posisibaruY = Mathf.Clamp(posisilamaY, minY, maxY);
        transform.position = new Vector3(posisibaruX, posisibaruY, 0f);
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
        if (direction == Vector2.zero) walkDustParticle.Stop();
        transform.Translate(direction * data.Speed * Time.deltaTime * accelerate);
    }


    private void Stop()
    {
        timeMoveElapsed -= Time.deltaTime;

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
                data.IsDashing = false;
                anim.SetBool("dash", false);
                dashDustParticle.Stop();
                immune = false;
                DOVirtual.DelayedCall(dashDelay, () => canDash = true); 
            }
            else
            {
                anim.SetBool("dash", true);
                if (walkDustParticle.isPlaying) walkDustParticle.Stop();
                dashDustParticle.Play();
                timeMoveElapsed = 0f;
                dashTime -= Time.deltaTime;
                rb.velocity = lastDirection * data.DashSpeed;

                DashingProjectile();
            }
        }
    }

    public bool PlayerDashing()
    {
        return data.IsDashing;
    }


    public void TakeDamage()
    {
        if (!immune)
        {
            TWAudioController.PlaySFX("player_damaged");
            if (data.Hp >= 1)
            {
                immune = true;
                DOTween.Kill("ImmuneDamage");
                DOVirtual.DelayedCall(2f, () => { immune = false; }).SetId("ImmuneDamage");
                data.Hp -= 1;
                InGameUI.instance.uilive();

                if (data.Hp < 1)
                {
                    dead = true;
                    GameVariables.STILL_ALIVE = false;
                    GameVariables.GAME_OVER = true;
                    InGameUI.instance.GameOver();
                }
            }
        }
    }

    private void DashingProjectile()
    {
        if (projectileDetect)
        {
            GameObject temp = projectileDetect;
            temp.transform.parent.GetComponent<MissileBM>().DashDeactiveMissile();
            temp.GetComponent<Animator>().SetTrigger("Dash");
            temp.GetComponent<SpriteRenderer>().sortingOrder = 5;
            if (lastDirection.y > 0)
            {
                temp.transform.up = temp.transform.position - BossBehaviour.Instance.transform.position;
                float distance = Mathf.Sqrt((BossBehaviour.Instance.transform.position - temp.transform.position).sqrMagnitude);
                temp.transform.DOMove(BossBehaviour.Instance.transform.position, distance/20f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    temp.transform.parent.GetComponent<MissileBM>().Explode();
                });
            }
            else
            {
                temp.GetComponent<Rigidbody2D>().velocity = lastDirection * data.DashSpeed;
                DOVirtual.DelayedCall(2f, () => Destroy(temp));
            }
            projectileDetect = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("projectiles") && PlayerDashing())
        {
            projectileDetect = collision.gameObject;
        }

        if (collision.CompareTag("damage area")&& !immune)
        {
            TakeDamage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (projectileDetect != null && collision.CompareTag("projectiles"))
        {
            projectileDetect = null;
        }
    }

    public void UpdateAnimationWalk(float x, float y, float speed)
    {
        anim.SetFloat("x", x);
        anim.SetFloat("y", y);
        anim.SetFloat("speed", speed);
    }

    
}
   
