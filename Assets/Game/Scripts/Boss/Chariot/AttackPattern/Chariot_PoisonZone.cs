using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chariot_PoisonZone : AttackEvent
{
    [SerializeField] private GameObject poisonParent;
    [SerializeField] private Transform damageArea;
    [SerializeField] private Transform[] poisonSpawn;
    [SerializeField] private Collider2D collide;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        ChangePoisonPosition();
        base.OnEnter_Attack();
    }

    protected override void Attack()
    {

        DOTween.Sequence()
            .AppendCallback(() => poisonParent.SetActive(true))
            .AppendInterval(.5f)
            .AppendCallback(() => collide.enabled = true);
        base.Attack();
    }

    protected override void OnExit_Attack()
    {
        collide.enabled = false;
        poisonParent.SetActive(false);
        base.OnExit_Attack();
    }

    public void ChangePoisonPosition()
    {
        switch (Chariot.Instance.health)
        {
            case 1:
                damageArea.position = poisonSpawn[0].position;
                break;
            case 2:
                damageArea.position = poisonSpawn[1].position;
                break;
            case 3:
                damageArea.position = poisonSpawn[2].position;
                break;
        }
    }
}
