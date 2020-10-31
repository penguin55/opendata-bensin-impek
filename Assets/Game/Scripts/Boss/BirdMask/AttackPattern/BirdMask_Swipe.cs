using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BirdMask_Swipe : AttackEvent
{
    [SerializeField] private Vector2[] swipeAreaPosition;
    [SerializeField] private Transform swipeObject;
    [SerializeField] private bool moveToRight;
    [SerializeField] private float timeToMove;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    public void MoveToLeft()
    {
        moveToRight = false;
    }

    protected override void OnEnter_Attack()
    {
        swipeObject.gameObject.SetActive(true);
        if (moveToRight) swipeObject.position = swipeAreaPosition[0];
        else swipeObject.position = swipeAreaPosition[1];

        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        int indexToMove = moveToRight ? 1 : 0; 
        swipeObject.DOMove(swipeAreaPosition[indexToMove], timeToMove)
            .SetEase(Ease.Linear)
            .OnComplete(() => base.Attack());
    }

    protected override void OnExit_Attack()
    {
        base.OnExit_Attack();
        swipeObject.gameObject.SetActive(false);
    }


}
