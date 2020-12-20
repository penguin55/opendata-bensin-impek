using DG.Tweening;
using UnityEngine;

public class TLE_Dash : TrainingListenerEvent
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject damageArea;
    [SerializeField] private float startTime, moveTime;
    [SerializeField] private Transform[] positionsMove;
    [SerializeField] private Transform[] playerPositions;

    private bool dash;
    private Transform start, target;

    private bool moveRight;

    public override void ActivateEventListener(bool flag)
    {
        base.ActivateEventListener(flag);

        parent.SetActive(flag);

        if (flag)
        {
            if (moveRight)
            {
                start = positionsMove[0];
                target = positionsMove[positionsMove.Length - 1];
                CharaController.instance.transform.position = playerPositions[positionsMove.Length - 1].position;
            }
            else
            {
                start = positionsMove[positionsMove.Length - 1];
                target = positionsMove[0];
                CharaController.instance.transform.position = playerPositions[0].position;
            }

            damageArea.transform.DOMove(start.position, 0f);

            DOVirtual.DelayedCall(startTime, ()=> MoveDamage());
        }
    }

    public override void InitEventListener<E>(E param)
    {
        if (typeof(bool).Equals(typeof(E)))
        {
            moveRight = ((bool)(object)param);
        }
    }

    protected override bool ValidateEventListener<E>(E eventListener)
    {
        return base.ValidateEventListener(eventListener);
    }

    protected override void CompleteEventListener<E>(ref E eventListener)
    {
        base.CompleteEventListener(ref eventListener);
        eventListener = (E)System.Convert.ChangeType(true, typeof(E));

        manager.CompleteTrainingSection();
    }

    private void MoveDamage()
    {
        damageArea.transform.DOMove(target.position, moveTime).SetEase(Ease.Linear)
            .OnComplete(()=>
            {
                CompleteEventListener(ref dash);
            });
    }
}
