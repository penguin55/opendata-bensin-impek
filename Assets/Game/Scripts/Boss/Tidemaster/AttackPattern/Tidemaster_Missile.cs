using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tidemaster_Missile : AttackEvent
{
    [System.Serializable]
    public struct SpawnProjectileArea
    {
        public Transform centerPlatform;
        public GameObject platform;
        public CannonShipBehaviour cannon;
    }

    [SerializeField] private GameObject missileParent;
    [SerializeField] private SpawnProjectileArea[] spawnProjectileArea;
    [SerializeField] private ParticleSystem[] smoke;
    [SerializeField] private GameObject projectilePrefabs;
    [SerializeField] protected float fireRate;

    [SerializeField] private Animator attack;

    private SpawnProjectileArea activeSpawnArea;

    private int priorityChoose;

    private GameObject missileTMProjectile;
    private MissileTM missileTMParent;

    // Store random priority
    private List<int> availablePriority;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    private void Start()
    {
        availablePriority = new List<int>();

        for (int i = 0; i < spawnProjectileArea.Length; i++ )
        {
            availablePriority.Add(i);
        }
    }

    protected override void OnEnter_Attack()
    {
        missileParent.SetActive(true);

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
        if (!missileTMParent.DeactiveMissileDashed())
        {
            Destroy(missileTMProjectile);
            missileParent.SetActive(false);
        }
        else
        {
            DOVirtual.DelayedCall(3f, () => missileParent.SetActive(false));
        }

        activeSpawnArea.cannon.Activate(false);

        missileTMProjectile = null;

        base.OnExit_Attack();
    }

    private void SpawnProjectile()
    {
        RandomPriority();

        missileTMParent = activeSpawnArea.centerPlatform.GetComponent<MissileTM>();
        GameObject missile = Instantiate(projectilePrefabs, (activeSpawnArea.centerPlatform.position + Vector3.up * 30), Quaternion.identity, activeSpawnArea.centerPlatform);
        missileTMParent.InitData(activeSpawnArea);
        missileTMParent.OnExplodeCallback(() => RemovePriority());
        missileTMParent.Launch(missile, .1f);

        activeSpawnArea.cannon.Activate(true);

        missileTMProjectile = missile;
    }

    private void RandomPriority()
    {
        int minPriority = availablePriority[0];
        int maxPriority = availablePriority[availablePriority.Count - 1];

        priorityChoose = Random.Range(0, 101) % 2 == 0 ? minPriority : maxPriority;
        activeSpawnArea = spawnProjectileArea[priorityChoose];
        smoke[priorityChoose].Play();
    }

    private void RemovePriority()
    {
        availablePriority.Remove(priorityChoose);
    }
}
