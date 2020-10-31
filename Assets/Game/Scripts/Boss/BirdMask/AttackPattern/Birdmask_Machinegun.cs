using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Birdmask_Machinegun : AttackEvent
{
    [SerializeField] private Transform[] movePosition;
    [SerializeField] private GameObject machinegunParent;
    [SerializeField] private Transform machinegunDothair;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float timeToMove;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        machinegunParent.SetActive(true);
        machinegunDothair.position = movePosition[0].position;
        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        machinegunDothair.DOMove(movePosition[1].position, timeToMove).SetEase(Ease.Linear).OnComplete(() => {
            machinegunParent.SetActive(false);        
            base.Attack();
        });
    }

    protected override void OnExit_Attack()
    {
        base.OnExit_Attack();
    }
}
