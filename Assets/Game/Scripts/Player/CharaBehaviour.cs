using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBehaviour : MonoBehaviour
{
    [SerializeField] protected CharaData data;
    [SerializeField] protected Vector2 direction, lastDirection;
    [SerializeField] protected float startDashTime, dashTime;
    [SerializeField] protected float kickProjectilesTime, kickTime;

    protected bool isDashed,immune, canDash, dead, insight = false;
    [SerializeField] protected float dashDelay;
    [SerializeField] protected Rigidbody2D rb, projectiles;

    [SerializeField] protected float timeMoveElapsed;
    [SerializeField] protected bool isAccelerating;
    [SerializeField] protected float timeToStop;

    public GameObject kiriatas, kananbawah;
    float minX, maxX, minY, maxY;
    float posisilamaX, posisilamaY;
    float posisibaruX, posisibaruY;

    private GameObject enemy;

    [SerializeField] protected Animator anim;
    [SerializeField] protected ParticleSystem particle;


    public void Init()
    {
        canDash = true;
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
        this.transform.position = new Vector3(posisibaruX, posisibaruY, 0f);
    }

    protected void MoveAccelerate()
    {
        if (isDashed) return;

        if (isAccelerating)
        {
            if (!particle.isPlaying) particle.Play();
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
        if (accelerate < 0.2f) particle.Stop();
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
                particle.maxParticles = 2;
                particle.emissionRate = 4;
                particle.Stop();
                immune = true;
                DOVirtual.DelayedCall(2f, () => { immune = false; });
                this.GetComponent<BoxCollider2D>().isTrigger = false;
                StartCoroutine(Delay());
            }
            else
            {
                anim.SetBool("dash", true);
                particle.maxParticles = 40;
                particle.emissionRate = 80;
                particle.Play();
                timeMoveElapsed = 0f;
                dashTime -= Time.deltaTime;
                rb.velocity = lastDirection * data.DashSpeed;
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(this.dashDelay);
        canDash = true;
    }

    public bool PlayerDashing()
    {
        return data.IsDashing;
    }


    public void TakeDamage()
    {
        if (data.Hp >= 1)
        {
            immune = true;
            DOVirtual.DelayedCall(2f, () => { immune = false; });
            data.Hp -= 1;
            Debug.Log(data.Hp);
            InGameUI.instance.uilive();
            
            if (data.Hp < 1)
            {
                dead = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("projectiles")&& PlayerDashing())
        {
            projectiles = collision.attachedRigidbody;
            if (lastDirection != Vector2.zero && isDashed)
            {
                if (dashTime <= 0)
                {
                    projectiles.velocity = Vector2.zero;
                    isDashed = false;
                    data.IsDashing = false;
                }
                else
                {
                    dashTime -= Time.deltaTime;
                    projectiles.velocity = lastDirection * data.DashSpeed;
                }
            }
        }

        if (collision.CompareTag("damage area")&& !immune)
        {
            TakeDamage();
        }
    }

    public void UpdateAnimationWalk(float x, float y, float speed)
    {
        anim.SetFloat("x", x);
        anim.SetFloat("y", y);
        anim.SetFloat("speed", speed);
    }

    
}
   
