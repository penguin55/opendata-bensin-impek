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
                CharaControlTraining.instance.transform.position = playerPositions[positionsMove.Length - 1].position;
            }
            else
            {
                start = positionsMove[positionsMove.Length - 1];
                target = positionsMove[0];
                CharaControlTraining.instance.transform.position = playerPositions[0].position;
            }

            damageArea.transform.DOMove(start.position, 0f);

            DOVirtual.DelayedCall(startTime, ()=> MoveDamage());
        }
    }

    public override void InitEventListener(string param, bool value)
    {
        if (param.ToLower().Equals("move_right"))
        {
            moveRight = value;
        }
    }

    protected override bool ValidateEventListener(string param)
    {
        if (param.ToLower().Equals("dash"))
        {
            return dash;
        }
        else return base.ValidateEventListener(param);
    }

    public override void CompleteEventListener(string param, bool value = true, bool forceComplete = false)
    {
        if (activeEventListener || forceComplete)
        {
            if (param.ToLower().Equals("dash"))
            {
                dash = value;
            }
        }
    }

    public override void RestartStateListener()
    {
        base.RestartStateListener();
        dash = false;
        parent.SetActive(false);
    }

    private void MoveDamage()
    {
        damageArea.transform.DOMove(target.position, moveTime).SetEase(Ease.Linear)
            .OnComplete(()=>
            {
                manager.CompleteTrainingSection();
            });
    }
}
