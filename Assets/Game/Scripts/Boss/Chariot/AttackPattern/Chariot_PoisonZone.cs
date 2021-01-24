using DG.Tweening;
using System.Collections.Generic;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class Chariot_PoisonZone : AttackEvent
{
    [SerializeField] private GameObject poisonParent;
    [SerializeField] private Transform poisonPosition1;
    [SerializeField] private Transform poisonPosition2;
    [SerializeField] private Transform[] poisonSpawn;
    [SerializeField] private Collider2D collide1;
    [SerializeField] private Collider2D collide2;
    [SerializeField] private ParticleSystem poisonParticle1;
    [SerializeField] private ParticleSystem poisonParticle2;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        ChangePoisonPosition();
        base.OnEnter_Attack();
    }

    protected override void Attack()
    {

        DOTween.Sequence()
            .AppendCallback(()=> TWAudioController.PlaySFX("BOSS_SFX", "poison_gas"))
            .AppendCallback(() => poisonParticle1.Play())
            .AppendCallback(() => poisonParticle2.Play())
            .AppendInterval(1f)
            .AppendCallback(() => collide1.enabled = true)
            .AppendCallback(() => collide2.enabled = true);
        base.Attack();
    }

    protected override void OnExit_Attack()
    {
        collide1.enabled = false;
        collide2.enabled = false;
        poisonParticle1.Stop();
        poisonParticle2.Stop();
        base.OnExit_Attack();
    }

    public void ChangePoisonPosition()
    {
        switch (Chariot.Instance.health)
        {
            case 1:
                poisonPosition1.position = poisonSpawn[0].position;
                poisonPosition2.position = new Vector2(poisonSpawn[0].position.x, poisonPosition2.position.y);
                break;
            case 2:
                poisonPosition1.position = poisonSpawn[1].position;
                poisonPosition2.position = new Vector2(poisonSpawn[1].position.x, poisonPosition2.position.y);
                break;
            case 3:
                poisonPosition1.position = poisonSpawn[2].position;
                poisonPosition2.position = new Vector2(poisonSpawn[2].position.x, poisonPosition2.position.y);
                break;
        }
    }
}
