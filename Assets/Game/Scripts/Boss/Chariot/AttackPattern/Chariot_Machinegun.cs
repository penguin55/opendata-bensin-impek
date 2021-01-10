using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;
public class Chariot_Machinegun : AttackEvent
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
        if(attack) attack.SetBool("attack", true);

        Sound("machine_gun");
        machinegunDothair.DOMove(movePosition[1].position, timeToMove).SetEase(Ease.Linear).OnComplete(() => {
            machinegunParent.SetActive(false);
            base.Attack();
            if (attack) attack.SetBool("attack", false);
            DOTween.Kill("MachineGun_Sound");
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
        base.OnExit_Attack();
    }
}
