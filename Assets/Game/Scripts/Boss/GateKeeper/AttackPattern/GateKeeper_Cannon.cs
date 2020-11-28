using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class GateKeeper_Cannon : AttackEvent
{
    [SerializeField] private float delay_attack;
    [SerializeField] private GateKeeper bossbehaviour;
    [SerializeField] private GameObject[] lasers;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeToFullRotate;
    [SerializeField] private int attackRate;
    private int randomIndex;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        randomIndex = Random.Range(0, lasers.Length);

        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        DOVirtual.DelayedCall((delay_attack / attackRate), () => {
            CannonAttack();
        }).SetLoops(attackRate)
        .OnComplete(()=>
        {
            lasers[randomIndex].SetActive(true);
            base.Attack();
        }).OnUpdate(()=>
        {
            float angle = GetAngleFromDirection(bossbehaviour.transform.position, CharaController.instance.gameObject.transform.position, true);
            bossbehaviour.RotateTank(angle);
        });
    }

    protected override void OnExit_Attack()
    {
        lasers[randomIndex].SetActive(false);
        base.OnExit_Attack();
    }

    private void CannonAttack()
    {
        CannonGK cannon = Instantiate(prefab, bossbehaviour.GetActiveSpawnPosition(), Quaternion.identity).GetComponent<CannonGK>();
        Vector3 direction = (bossbehaviour.GetActiveSpawnPosition() - bossbehaviour.GetCenterRotatePosition()).normalized;
        cannon.Launch(direction, bulletSpeed);
    }

    private float GetAngleFromDirection(Vector3 from, Vector3 to, bool clockwise)
    {
        Vector3 diff = to - from;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        return rot_z;
    }
}
