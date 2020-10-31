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

    protected bool isDashed, canDash, dead, insight = false;
    [SerializeField] protected float dashDelay;
    [SerializeField] protected Rigidbody2D rb, projectiles;

    [SerializeField] protected float timeMoveElapsed;
    [SerializeField] protected bool isAccelerating;
    [SerializeField] protected float timeToStop;

    private GameObject enemy;


    public void Init()
    {
        canDash = true;
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
    }


    protected void MoveAccelerate()
    {
        if (isAccelerating)
        {
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
                this.GetComponent<BoxCollider2D>().isTrigger = false;
                StartCoroutine(Delay());
            }
            else
            {
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
        if(collision.gameObject.tag == "projectiles" && PlayerDashing())
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

    }
}
   
