using DG.Tweening;
using TomWill;
using UnityEngine;

public class TLE_Dash : TrainingListenerEvent
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject damageArea;
    [SerializeField] private float startTime, moveTime;
    [SerializeField] private Transform[] positionsMove;
    [SerializeField] private Transform[] playerPositions;
    [SerializeField] private Animator anim;

    private bool dash;
    private Transform start, target;

    private bool moveRight;

    public override void ActivateEventListener(bool flag)
    {
        base.ActivateEventListener(flag);

        parent.SetActive(flag);

        if (flag)
        {
            damageArea.transform.DOMove(start.position, 0f);

            DOTween.Sequence()
                .AppendInterval(1)
                .AppendCallback(() => { anim.gameObject.SetActive(true); })
                .AppendCallback(() => { TWAudioController.PlaySFX("BOSS_SFX", "boss_attack_telegraph"); })
                .AppendInterval(1f)
                .AppendCallback(() => anim.gameObject.SetActive(false))
                .AppendInterval(startTime)
                .AppendCallback(() => MoveDamage());
        }
    }

    public override void InitEventListener(string param, bool value)
    {
        if (param.ToLower().Equals("move_right"))
        {
            moveRight = value;
        }

        InitPreparation();
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
        InitPreparation();
    }

    private void MoveDamage()
    {
        TWAudioController.PlaySFX("BOSS_SFX", "laserbeam_firing");
        damageArea.transform.DOMove(target.position, moveTime).SetEase(Ease.Linear)
            .OnComplete(()=>
            {
                OnCompleteSection();
            });
    }

    private void OnCompleteSection()
    {
        if (dash)
        {
            manager.CompleteTrainingSection();
        }
        else
        {
            manager.InteruptTrainingSection();
        }
    }

    private void InitPreparation()
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

        parent.SetActive(true);
        damageArea.transform.DOMove(start.position, 0f);
    }
}
