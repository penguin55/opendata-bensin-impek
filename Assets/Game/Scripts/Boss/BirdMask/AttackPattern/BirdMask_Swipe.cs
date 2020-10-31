using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BirdMask_Swipe : AttackEvent
{
    [SerializeField] private Vector2[] swipeAreaPosition;
    [SerializeField] private Transform swipeObject;
    [SerializeField] private bool moveToRight;
    [SerializeField] private float timeToMove;
    [SerializeField] private GameObject birdMask;

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
        Vector2 helicopterOffset; 
        if (moveToRight)
        {
            helicopterOffset = new Vector2(swipeAreaPosition[0].x, birdMask.transform.position.y);
            swipeObject.position = swipeAreaPosition[0];
        }
        else
        {
            helicopterOffset = new Vector2(swipeAreaPosition[1].x, birdMask.transform.position.y);
            swipeObject.position = swipeAreaPosition[1];
        }
        birdMask.transform.DOMove(helicopterOffset, 1).OnComplete(() => { base.OnEnter_Attack(); });
    }

    protected override void Attack()
    {
        int indexToMove = moveToRight ? 1 : 0;
        Vector2 helicopterOffset;
        if (moveToRight)
        {
            helicopterOffset = new Vector2(swipeAreaPosition[1].x, birdMask.transform.position.y);
        }
        else
        {
            helicopterOffset = new Vector2(swipeAreaPosition[0].x, birdMask.transform.position.y);
        }
        birdMask.transform.DOMove(helicopterOffset, timeToMove).SetEase(Ease.Linear);
        swipeObject.DOMove(swipeAreaPosition[indexToMove], timeToMove)
            .SetEase(Ease.Linear)
            .OnComplete(() => base.Attack());
    }

    protected override void OnExit_Attack()
    {
        Vector2 helicopterOffset = new Vector2(0, birdMask.transform.position.y);
        base.OnExit_Attack();
        birdMask.transform.DOMove(helicopterOffset, timeToMove).SetEase(Ease.Linear);
        swipeObject.gameObject.SetActive(false);
    }


}
