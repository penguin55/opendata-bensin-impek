using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BirdMask_Laser : AttackEvent
{
    [SerializeField] private GameObject laserParent;
    [SerializeField] private Vector2[] originalPosition;
    [SerializeField] private Transform[] damageArea;
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float timeToMove;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        laserParent.SetActive(true);
        ActivateDamageArea(true);
        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        DOTween.Sequence()
            .Append(damageArea[0].DOMove(target.position + Vector3.up * offset.y, timeToMove))
            .Join(damageArea[1].DOMove(target.position + Vector3.right * offset.x, timeToMove))
            .Join(damageArea[2].DOMove(target.position + Vector3.down * offset.y, timeToMove))
            .Join(damageArea[3].DOMove(target.position + Vector3.left * offset.x, timeToMove))
            .OnComplete(() => base.Attack());
    }

    protected override void OnExit_Attack()
    {
        ActivateDamageArea(false);
        laserParent.SetActive(false);
        base.OnExit_Attack();
    }

    private void ActivateDamageArea(bool active)
    {
        for (int i = 0; i < damageArea.Length; i++)
        {
            damageArea[i].gameObject.SetActive(active);
            if (active)
            {
                damageArea[i].position = originalPosition[i];
            }
        }
    }
}