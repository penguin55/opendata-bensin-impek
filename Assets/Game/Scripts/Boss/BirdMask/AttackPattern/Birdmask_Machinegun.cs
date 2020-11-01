using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class Birdmask_Machinegun : AttackEvent
{
    [SerializeField] private Transform[] movePosition;
    [SerializeField] private GameObject machinegunParent;
    [SerializeField] private Transform machinegunDothair;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float timeToMove;
    [SerializeField] private Animator attack;

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
        attack.SetBool("attack", true);
        machinegunDothair.DOMove(movePosition[1].position, timeToMove)
            .SetEase(Ease.Linear).OnUpdate(()=> { TWAudioController.PlaySFX("machine_gun"); CameraShake.instance.Shake(1, 1, 2); }).OnComplete(() => {
            machinegunParent.SetActive(false);        
            base.Attack();
            attack.SetBool("attack", false);
        });
    }

    protected override void OnExit_Attack()
    {
        base.OnExit_Attack();
    }
}
