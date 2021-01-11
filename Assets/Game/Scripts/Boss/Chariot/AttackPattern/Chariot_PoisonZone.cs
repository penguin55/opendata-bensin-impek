using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chariot_PoisonZone : AttackEvent
{
    [SerializeField] private GameObject poisonParent;
    [SerializeField] private Transform damageArea;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        poisonParent.SetActive(true);
        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        //DOTween.Sequence()
        //    .AppendCallback(SpawnProjectile)
        //    .AppendCallback(SpawnProjectile)
        //    .AppendInterval(fireRate)
        //    .OnComplete(() =>
        //    {
        //        if (queueSpawn.Count > 0) Attack();
        //        else base.Attack();
        //    })
        //    .SetId("BM_Missile");
        base.Attack();
    }

    protected override void OnExit_Attack()
    {
        poisonParent.SetActive(false);
        base.OnExit_Attack();
    }
}
