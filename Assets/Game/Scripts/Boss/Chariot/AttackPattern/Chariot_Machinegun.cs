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

    private Chariot.DataGerbong activeGerbong;
    private float distance;
    private int idAnimation;

    public void InitialitationPattern(Chariot.DataGerbong activeGerbong)
    {
        this.activeGerbong = activeGerbong;
        idAnimation = -1;
    }

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        distance = movePosition[1].position.x - movePosition[0].position.x;
        machinegunParent.SetActive(true);
        machinegunDothair.position = movePosition[0].position;
        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        if(attack) attack.SetBool("attack", true);

        Sound("machine_gun");
        RotateFace();
        machinegunDothair.DOMove(movePosition[1].position, timeToMove).SetEase(Ease.Linear).OnComplete(() => {
            machinegunParent.SetActive(false);
            base.Attack();
            if (attack) attack.SetBool("attack", false);
            DOTween.Kill("MachineGun_Sound");
            activeGerbong.anim.SetTrigger("idle");
        }).OnUpdate(()=>
        {
            if (Time.frameCount % 5 == 0) RotateFace();
        });
    }

    protected override void OnExit_Attack()
    {
        base.OnExit_Attack();
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

    private void RotateFace()
    {
        float diff = machinegunDothair.position.x - movePosition[0].position.x;
        if (diff >= (distance * 2 / 3) && idAnimation != 2)
        {
            idAnimation = 2;
            activeGerbong.anim.SetTrigger("right_gun");
        }
        else if (diff >= (distance * 1 / 3) && diff < (distance * 2 / 3) && idAnimation != 1)
        {
            idAnimation = 1;
            activeGerbong.anim.SetTrigger("middle_gun");
        } 
        else if (diff >= 0 && diff < (distance * 1 / 3) && idAnimation != 0)
        {
            idAnimation = 0;
            activeGerbong.anim.SetTrigger("left_gun");
        }
    }
}
