using DG.Tweening;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteChariot : MonoBehaviour
{
    [SerializeField] private float maxTimeExplode;
    [SerializeField] private Vector2 perfectTime;
    [SerializeField] private Animator anim;

    private Transform[] dropPosition;
    private float moveTime;
    private float elapsedTime;
    private float[] distance;
    private float distanceTotal;

    private bool onHand;
    private bool activeDynamite;
    private bool onThrow;
    private bool readyToExplode;


    public void Init(Transform[] dropPosition, float moveTime)
    {
        readyToExplode = false;
        this.dropPosition = dropPosition;
        this.moveTime = moveTime;
        CalculateDistanceTime();
        MoveDynamite();
        DynamiteUIHandle.Instance.Init(perfectTime.x, perfectTime.y, maxTimeExplode);
    }

    public void MoveDynamite()
    {
        MoveTo(1);
    }

    public void GetDynamite()
    {
        onHand = true;
        DOTween.Kill("Dynamite");
        ActivateTickDynamite();
        anim.SetTrigger("OnHand");
    }

    public void ThrowDynamite()
    {
        if (onThrow) return;

        onThrow = true;

        DeactiveTickDynamite();

        if (elapsedTime >= perfectTime.x && elapsedTime < perfectTime.y)
        {
            Chariot boss = (BossBehaviour.Instance as Chariot);
            transform.DOMove(boss.GetActiveGerbong(), AdjustTimeByDistance()).SetEase(Ease.Linear).OnComplete(()=> { Explode(false); });
        } else if (elapsedTime < perfectTime.x)
        {
            Destroy(gameObject);
        } else
        {
            Explode(true);
        }
    }

    public void DisableDynamite()
    {
        DeactiveTickDynamite();
        Destroy(gameObject);
    }

    private void Explode(bool explodeOnHand)
    {
        if (explodeOnHand)
        {
            CharaController.instance.TakeDamage();
        }
        else
        {
            BossBehaviour.Instance.TakeDamage();
        }

        Destroy(gameObject);
    }

    private void MoveTo(int index)
    {
        if (index < dropPosition.Length)
        {
            transform.DOMove(dropPosition[index].position, moveTime * (distance[index] / distanceTotal)).SetEase(Ease.Linear)
                .OnComplete(() => {if (!onHand) MoveTo(index + 1); }).SetId("Dynamite");
        } else
        {
            Destroy(gameObject);
        }
    }

    private void CalculateDistanceTime()
    {
        distance = new float[dropPosition.Length];
        distanceTotal = 0;

        for (int i = 0; i<dropPosition.Length; i++)
        {
            if (i > 0) distance[i] = Mathf.Sqrt((dropPosition[i].position - dropPosition[i - 1].position).sqrMagnitude);
            else distance[i] = 0;

            distanceTotal += distance[i];
        }
    }

    private float AdjustTimeByDistance()
    {
        float distance = Mathf.Sqrt(((BossBehaviour.Instance as Chariot).GetActiveGerbong() - transform.position).sqrMagnitude);
        Debug.Log(distance);
        return (distance / 16f) * 0.7f;
    }

    private void ActivateTickDynamite()
    {
        DynamiteUIHandle.Instance.ShowBar(true);
        activeDynamite = true;
    }

    private void DeactiveTickDynamite()
    {
        DynamiteUIHandle.Instance.ShowBar(false);
        activeDynamite = false;
    }

    private void Update()
    {
        if (activeDynamite)
        {
            elapsedTime += GameTime.LocalTime;

            if (!readyToExplode && elapsedTime >= perfectTime.x)
            {
                readyToExplode = true;
                anim.SetTrigger("ReadyToExplode");
            }

            if (elapsedTime >= maxTimeExplode)
            {
                elapsedTime = maxTimeExplode;
                activeDynamite = false;

                DynamiteUIHandle.Instance.UpdateTickTimeBar(elapsedTime, CharaController.instance.GetHandPlaceholder() + Vector3.up * 1f);
                DynamiteUIHandle.Instance.ShowBar(false);

                ThrowDynamite();
            } else
            {
                DynamiteUIHandle.Instance.UpdateTickTimeBar(elapsedTime, CharaController.instance.GetHandPlaceholder() + Vector3.up * 1f);
            }
        }
    }
}
