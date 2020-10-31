using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BirdMask_Missile : AttackEvent
{
    [SerializeField] private Transform[] spawnProjectilePosition;
    [SerializeField] private GameObject projectilePrefabs;

    private int randomSpawn;

    private List<Transform> queueSpawn;
    private Transform spawnChoicePosition;

    private bool deactiveMissileWasLaunch;
    private GameObject deactiveMissileProjectile;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
        OnEnter_Attack();
    }

    private void Start()
    {
        queueSpawn = new List<Transform>();
    }

    private void OnEnter_Attack()
    {
        deactiveMissileWasLaunch = false;
        queueSpawn.AddRange(spawnProjectilePosition);

        DOVirtual.DelayedCall(delay_enter, Attack);
    }

    private void Attack()
    {
        DOTween.Sequence()
            .AppendCallback(SpawnProjectile)
            .AppendInterval(fireRate)
            .OnComplete(() =>
            {
                if (queueSpawn.Count > 0) Attack();
                else DOVirtual.DelayedCall(delay_exit, OnExit_Attack);
            });
    }

    private void OnExit_Attack()
    {
        deactiveMissileWasLaunch = false;
        Destroy(deactiveMissileProjectile);
        deactiveMissileProjectile = null;
        onCompleteAction.Invoke();
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
            } else
            {
                int randomValue = Random.Range(0, 100);
                activeMissile = randomValue % 2 == 1;
                if (!activeMissile) deactiveMissileWasLaunch = true;
            }
        }

        GameObject missile = Instantiate(projectilePrefabs, (spawnChoicePosition.position + Vector3.up * 30), Quaternion.identity, spawnChoicePosition);
        spawnChoicePosition.GetComponent<MissileBM>().Launch(missile, 2f, activeMissile);

        if (!activeMissile) deactiveMissileProjectile = missile;
    }
}
