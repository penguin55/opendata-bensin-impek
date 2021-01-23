using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tidemaster_EnergyBalls : AttackEvent
{
    [SerializeField] private GameObject missileParent;
    [SerializeField] private Transform[] spawnProjectilePosition;
    [SerializeField] private GameObject projectilePrefabs;
    [SerializeField] protected float fireRate;

    [SerializeField] private Animator attack;

    private int randomSpawn;

    private List<Transform> queueSpawn;
    private Transform spawnChoicePosition;

    private bool deactiveMissileWasLaunch;
    private GameObject deactiveMissileProjectile;
    private EnergyBalssTM deactiveMissileParent;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    private void Start()
    {
        queueSpawn = new List<Transform>();
    }

    protected override void OnEnter_Attack()
    {
        missileParent.SetActive(true);
        deactiveMissileWasLaunch = false;
        queueSpawn.AddRange(spawnProjectilePosition);

        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        DOTween.Sequence()
            .AppendCallback(SpawnProjectile)
            .AppendInterval(.2f)
            .AppendCallback(SpawnProjectile)
            .AppendInterval(.2f)
            .AppendCallback(SpawnProjectile)
            .AppendInterval(.2f)
            .AppendCallback(SpawnProjectile)
            .AppendInterval(.2f)
            .AppendCallback(SpawnProjectile)
            .AppendInterval(.2f)
            .AppendCallback(SpawnProjectile)
            .AppendInterval(.2f)
            .AppendCallback(SpawnProjectile)
            .AppendInterval(.2f)
            .AppendCallback(SpawnProjectile)
            .AppendInterval(.2f)
            .AppendCallback(SpawnProjectile)
            .AppendInterval(.2f)
            .AppendCallback(SpawnProjectile)
            .AppendInterval(fireRate)
            .OnComplete(() =>
            {
                if (queueSpawn.Count > 0) Attack();
                else base.Attack();
            })
            .SetId("BM_Missile");
    }

    protected override void OnExit_Attack()
    {
        deactiveMissileWasLaunch = false;
        DOVirtual.DelayedCall(3f, () => missileParent.SetActive(false));
        deactiveMissileProjectile = null;
        base.OnExit_Attack();
    }

    private void SpawnProjectile()
    {
        randomSpawn = Random.Range(0, queueSpawn.Count);
        spawnChoicePosition = queueSpawn[randomSpawn];
        queueSpawn.Remove(spawnChoicePosition);
        deactiveMissileParent = spawnChoicePosition.GetComponent<EnergyBalssTM>();
        GameObject missile = Instantiate(projectilePrefabs, (spawnChoicePosition.position + Vector3.up * 30), Quaternion.identity, spawnChoicePosition);
        spawnChoicePosition.GetComponent<EnergyBalssTM>().Launch(missile, .1f);
        deactiveMissileProjectile = missile;
    }
}
