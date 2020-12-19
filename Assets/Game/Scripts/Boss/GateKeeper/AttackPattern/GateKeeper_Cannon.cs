using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class GateKeeper_Cannon : AttackEvent
{
    [SerializeField] private ObjectRotator rotator;
    [SerializeField] private float delay_attack;
    [SerializeField] private GateKeeper bossbehaviour;
    [SerializeField] private GateKeeper_LaserDamage laserDamage;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeToFullRotate;
    [SerializeField] private int attackRate;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        rotator.ActiveFollow(false);
        laserDamage.ActivateInteractLaser(true);

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
            base.Attack();
        }).OnUpdate(()=>
        {
            float angle = GetAngleFromDirection(bossbehaviour.transform.position, rotator.gameObject.transform.position, true);
            bossbehaviour.RotateTank(angle);
        });
    }

    protected override void OnExit_Attack()
    {
        laserDamage.ActivateInteractLaser(false);
        rotator.ActiveFollow(false);
        base.OnExit_Attack();
    }

    private void CannonAttack()
    {
        CannonGK cannon = Instantiate(prefab, bossbehaviour.GetActiveSpawnPosition(), Quaternion.identity).GetComponent<CannonGK>();
        Vector3 direction = (bossbehaviour.GetActiveSpawnPosition() - bossbehaviour.GetCenterRotatePosition()).normalized;

        float angleDir = GetAngleFromDirection(bossbehaviour.GetCenterRotatePosition(), CharaController.instance.gameObject.transform.position, true);

        if (bossbehaviour.IsSameDirection(angleDir))
        {
            direction = (CharaController.instance.gameObject.transform.position - bossbehaviour.GetCenterRotatePosition()).normalized;
        }

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
}

[System.Serializable]
public class LasersGateKeeper
{
    public GunInteractDetect gun;
    public GameObject laser;
    public GameObject sign;
}