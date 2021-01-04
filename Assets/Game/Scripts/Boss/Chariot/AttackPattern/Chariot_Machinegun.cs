using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;
public class Chariot_Machinegun : AttackEvent
{
    [SerializeField] private Vector2[] swipeAreaPosition;
    [SerializeField] private Transform swipeObject;
    [SerializeField] private bool moveToRight;
    [SerializeField] private float timeToMove;
    [SerializeField] private float timeGoBack;
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
        birdMask.transform.DOMove(helicopterOffset, timeGoBack).OnComplete(() => { base.OnEnter_Attack(); });
    }

    protected override void Attack()
    {
        attack.SetBool("attack", true);
        int indexToMove = moveToRight ? 1 : 0;
        Vector2 helicopterOffset;
        if (moveToRight)
        {
            TWAudioController.PlaySFX("BOSS_SFX", "helicopter_strafing");
            helicopterOffset = new Vector2(swipeAreaPosition[1].x, birdMask.transform.position.y);
            birdMask.transform.DORotate(Vector3.forward * -25, 0.5f);
        }
        else
        {
            TWAudioController.PlaySFX("BOSS_SFX", "helicopter_strafing");
            helicopterOffset = new Vector2(swipeAreaPosition[0].x, birdMask.transform.position.y);
            birdMask.transform.DORotate(Vector3.forward * 25, 0.5f);
        }

        Sound("machine_gun");
        birdMask.transform.DOMove(helicopterOffset, timeToMove).SetEase(Ease.Linear);
        swipeObject.DOLocalMove(swipeAreaPosition[indexToMove], timeToMove).SetEase(Ease.Linear).OnComplete(() =>
        {
            DOTween.Kill("MachineGun_Sound");
            base.Attack();
        });
    }

    private void Sound(string name)
    {
        float audioLength = TWAudioController.AudioLength(name, "SFX");
        DOTween.Sequence()
            .AppendCallback(() => TWAudioController.PlaySFX("SFX_BOSS", name))
            .AppendCallback(() => CameraShake.instance.Shake(audioLength, 1, 2))
            .PrependInterval(audioLength / 2)
            .SetLoops(-1)
            .SetId("MachineGun_Sound");
    }

    protected override void OnExit_Attack()
    {
        attack.SetBool("attack", false);
        Vector2 helicopterOffset = new Vector2(0, birdMask.transform.position.y);

        if (moveToRight) birdMask.transform.DORotate(Vector3.forward * 25, 0.5f);
        else birdMask.transform.DORotate(Vector3.forward * -25, 0.5f);

        birdMask.transform.DOMove(helicopterOffset, timeGoBack).SetEase(Ease.Linear).OnComplete(() => {
            birdMask.transform.DORotate(Vector3.zero, 0.5f);
            base.OnExit_Attack();
        });
        swipeObject.gameObject.SetActive(false);
    }
}
