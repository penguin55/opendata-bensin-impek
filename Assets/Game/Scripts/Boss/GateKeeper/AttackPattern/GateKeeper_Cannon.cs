using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class GateKeeper_Cannon : AttackEvent
{
    [SerializeField] private ObjectRotator rotator;
    [SerializeField] private float delay_attack;
    [SerializeField] private GateKeeper bossbehaviour;
    [SerializeField] private LasersGateKeeper[] lasersGateKeeper;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeToFullRotate;
    [SerializeField] private int attackRate;
    private int randomIndex;

    private bool canActiveLaser;
    private bool active_attack;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        rotator.ActiveFollow(false);
        randomIndex = Random.Range(0, lasersGateKeeper.Length);

        canActiveLaser = true;
        lasersGateKeeper[randomIndex].sign.SetActive(true);

        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        rotator.ActiveFollow(true);
        DOVirtual.DelayedCall((delay_attack / attackRate), () => {
            CannonAttack();
        }).SetLoops(attackRate)
        .OnComplete(()=>
        {
            lasersGateKeeper[randomIndex].sign.SetActive(false);
            if (active_attack)
            {
                TWAudioController.PlaySFX("BOSS_SFX", "laserbeam_firing");
                lasersGateKeeper[randomIndex].laser.SetActive(true);
            }
            canActiveLaser = false;
            base.Attack();
        }).OnUpdate(()=>
        {
            float angle = GetAngleFromDirection(bossbehaviour.transform.position, rotator.gameObject.transform.position, true);
            bossbehaviour.RotateTank(angle);
        });
    }

    protected override void OnExit_Attack()
    {
        lasersGateKeeper[randomIndex].laser.SetActive(false);
        active_attack = false;
        rotator.ActiveFollow(false);
        base.OnExit_Attack();
    }

    private void CannonAttack()
    {
        CannonGK cannon = Instantiate(prefab, bossbehaviour.GetActiveSpawnPosition(), Quaternion.identity).GetComponent<CannonGK>();
        Vector3 direction = (bossbehaviour.GetActiveSpawnPosition() - bossbehaviour.GetCenterRotatePosition()).normalized;
        cannon.Launch(direction, bulletSpeed);
        TWAudioController.PlaySFX("BOSS_SFX", "tank_firing");
    }

    private float GetAngleFromDirection(Vector3 from, Vector3 to, bool clockwise)
    {
        Vector3 diff = to - from;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        return rot_z;
    }

    public void ActivateLaser(GunInteractDetect gun)
    {
        if (canActiveLaser && gun == lasersGateKeeper[randomIndex].gun)
        {
            TWAudioController.PlaySFX("BOSS_SFX", "laserbeam_gettingready");
            gun.transform.DOPunchScale(Vector3.one * 0.25f, 0.2f, 1, 0);

            DOVirtual.DelayedCall(3f, () => gun.transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 1, 0)).SetLoops(3);

            canActiveLaser = false;
            active_attack = true;
        }
    }
}

[System.Serializable]
public class LasersGateKeeper
{
    public GunInteractDetect gun;
    public GameObject laser;
    public GameObject sign;
}