using DG.Tweening;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

public class DynamiteChariot : MonoBehaviour
{
    [SerializeField] private float maxTimeExplode;
    [SerializeField] private Vector2 perfectTime;
    [SerializeField] private Animator anim;

    private Transform[] dropPosition;
    private float moveTime;
    private float elapsedTime;
    private float[] distanceDrop;
    private float distanceDropTotal;
    private float throwTime;
    private float[] distanceThrow;
    private float distanceThrowTotal;

    private bool onHand;
    private bool activeDynamite;
    private bool onThrow;
    private bool readyToExplode;

    private List<Vector2> throwingPosition;


    public void Init(Transform[] dropPosition, float moveTime)
    {
        throwingPosition = new List<Vector2>();
        readyToExplode = false;
        this.dropPosition = dropPosition;
        this.moveTime = moveTime;
        CalculateDistanceDropTime();
        MoveDynamite();
        DynamiteUIHandle.Instance.Init(perfectTime.x, perfectTime.y, maxTimeExplode);
    }

    public void MoveDynamite()
    {
        MoveToDrop(1);
    }

    public void GetDynamite()
    {
        onHand = true;
        DOTween.Kill("DropDynamite");
        TWAudioController.PlaySFX("SFX_PROJECTILE", "dynamite_obtained");
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
            ThrowingDynamite();
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

    private void ThrowingDynamite()
    {
        TWAudioController.PlaySFX("SFX_PROJECTILE", "dynamite_throw");
        Chariot boss = (BossBehaviour.Instance as Chariot);
        Vector2 controlPoint = ExtendMath.GetPerpendicular(transform.position, boss.GetActiveGerbong(), 7f, ExtendMath.PerpendicularType.ALWAYS_UP);

        ExtendMath.BezierCurve(ref throwingPosition, transform.position, boss.GetActiveGerbong(), controlPoint, 10);
        CalculateDistanceThrowTime();
        throwTime = AdjustTimeByDistance();
        MoveToThrow(1);
    }

    private void Explode(bool explodeOnHand)
    {
        TWAudioController.PlaySFX("SFX_PROJECTILE", "rocket_impact");
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

    private void MoveToDrop(int index)
    {
        if (index < dropPosition.Length)
        {
            transform.DOMove(dropPosition[index].position, moveTime * (distanceDrop[index] / distanceDropTotal)).SetEase(Ease.Linear)
                .OnComplete(() => {if (!onHand) MoveToDrop(index + 1); }).SetId("DropDynamite");
        } else
        {
            Destroy(gameObject);
        }
    }

    private void MoveToThrow(int index)
    {
        if (index < throwingPosition.Count)
        {
            transform.DOMove(throwingPosition[index], throwTime * (distanceThrow[index] / distanceThrowTotal)).SetEase(Ease.Linear)
                .OnComplete(() => { MoveToThrow(index + 1); }).SetId("ThrowDynamite");
        }
        else
        {
            Explode(false);
        }
    }

    private void CalculateDistanceDropTime()
    {
        distanceDrop = new float[dropPosition.Length];
        distanceDropTotal = 0;

        for (int i = 0; i<dropPosition.Length; i++)
        {
            if (i > 0) distanceDrop[i] = Mathf.Sqrt((dropPosition[i].position - dropPosition[i - 1].position).sqrMagnitude);
            else distanceDrop[i] = 0;

            distanceDropTotal += distanceDrop[i];
        }
    }

    private void CalculateDistanceThrowTime()
    {
        distanceThrow = new float[throwingPosition.Count];
        distanceThrowTotal = 0;

        for (int i = 0; i < throwingPosition.Count; i++)
        {
            if (i > 0) distanceThrow[i] = Mathf.Sqrt((throwingPosition[i] - throwingPosition[i - 1]).sqrMagnitude);
            else distanceThrow[i] = 0;

            distanceThrowTotal += distanceThrow[i];
        }
    }

    private float AdjustTimeByDistance()
    {
        float distance = Mathf.Sqrt(((BossBehaviour.Instance as Chariot).GetActiveGerbong() - transform.position).sqrMagnitude);
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

    private void OnDrawGizmos()
    {
        foreach (Vector2 data in throwingPosition)
        {
            Gizmos.DrawSphere(data, 1f);
        }
    }
}
