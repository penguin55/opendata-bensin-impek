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

    [SerializeField] private Animator attack;

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
            swipeObject.localPosition = swipeAreaPosition[0];
            birdMask.transform.DORotate(Vector3.forward * 25, 0.5f);
        }
        else
        {
            helicopterOffset = new Vector2(swipeAreaPosition[1].x, birdMask.transform.position.y);
            swipeObject.localPosition = swipeAreaPosition[1];
            birdMask.transform.DORotate(Vector3.forward * -25, 0.5f);
        }
        birdMask.transform.DOMove(helicopterOffset, 1).OnComplete(() => { base.OnEnter_Attack(); });
    }

    protected override void Attack()
    {
        attack.SetBool("attack", true);
        int indexToMove = moveToRight ? 1 : 0;
        Vector2 helicopterOffset;
        if (moveToRight)
        {
            helicopterOffset = new Vector2(swipeAreaPosition[1].x, birdMask.transform.position.y);
            birdMask.transform.DORotate(Vector3.forward * -25, 0.5f);
        }
        else
        {
            helicopterOffset = new Vector2(swipeAreaPosition[0].x, birdMask.transform.position.y);
            birdMask.transform.DORotate(Vector3.forward * 25, 0.5f);
        }
        birdMask.transform.DOMove(helicopterOffset, timeToMove).SetEase(Ease.Linear);
        swipeObject.DOMove(swipeAreaPosition[indexToMove], timeToMove)
            .SetEase(Ease.Linear)
            .OnComplete(() => base.Attack());
    }

    protected override void OnExit_Attack()
    {
        attack.SetBool("attack", false);
        Vector2 helicopterOffset = new Vector2(0, birdMask.transform.position.y);
        
        if (moveToRight) birdMask.transform.DORotate(Vector3.forward * 25, 0.5f);
        else birdMask.transform.DORotate(Vector3.forward * -25, 0.5f);

        birdMask.transform.DOMove(helicopterOffset, timeToMove).SetEase(Ease.Linear).OnComplete(() => {
            birdMask.transform.DORotate(Vector3.zero, 0.5f);
            base.OnExit_Attack();
        });
        swipeObject.gameObject.SetActive(false);
    }
}
