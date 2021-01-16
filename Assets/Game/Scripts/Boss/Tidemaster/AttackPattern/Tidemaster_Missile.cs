using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tidemaster_Missile : AttackEvent
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
    private MissileTM deactiveMissileParent;

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
            .AppendInterval(fireRate)
            .OnComplete(() =>
            {
                base.Attack();
            })
            .SetId("BM_Missile");
    }

    protected override void OnExit_Attack()
    {
        deactiveMissileWasLaunch = false;
        if (!deactiveMissileParent.DeactiveMissileDashed())
        {
            Destroy(deactiveMissileProjectile);
            missileParent.SetActive(false);
        }
        else
        {
            DOVirtual.DelayedCall(3f, () => missileParent.SetActive(false));
        }

        deactiveMissileProjectile = null;

        base.OnExit_Attack();
    }

    private void SpawnProjectile()
    {
        randomSpawn = Random.Range(0, queueSpawn.Count);
        spawnChoicePosition = queueSpawn[randomSpawn];
        queueSpawn.Remove(spawnChoicePosition);

        bool activeMissile = true;

        if (!deactiveMissileWasLaunch)
        {
            if (queueSpawn.Count == 0)
            {
                deactiveMissileWasLaunch = true;
                activeMissile = false;
            }
            else
            {
                int randomValue = Random.Range(0, 100);
                activeMissile = randomValue % 2 == 1;
                if (!activeMissile) deactiveMissileWasLaunch = true;
            }
        }

        if (!activeMissile) deactiveMissileParent = spawnChoicePosition.GetComponent<MissileTM>();

        GameObject missile = Instantiate(projectilePrefabs, (spawnChoicePosition.position + Vector3.up * 30), Quaternion.identity, spawnChoicePosition);
        spawnChoicePosition.GetComponent<MissileTM>().Launch(missile, .1f, activeMissile);

        if (!activeMissile) deactiveMissileProjectile = missile;
    }
}
