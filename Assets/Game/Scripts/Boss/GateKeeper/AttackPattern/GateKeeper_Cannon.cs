using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class GateKeeper_Cannon : AttackEvent
{
    [SerializeField] private GateKeeper bossbehaviour;
    [SerializeField] private GameObject[] lasers;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float speed;
    [SerializeField] private int attackRate;

    public override void ExecutePattern(UnityAction onComplete)
    {
        base.ExecutePattern(onComplete);
    }

    protected override void OnEnter_Attack()
    {
        int random = Random.Range(0, lasers.Length);

        

        base.OnEnter_Attack();
    }

    protected override void Attack()
    {
        DOVirtual.DelayedCall((delay_active / attackRate), () => {
            CannonAttack();
        }).SetLoops(attackRate)
        .OnComplete(()=> base.Attack());
    }

    protected override void OnExit_Attack()
    {
        base.OnExit_Attack();
    }

    private void CannonAttack()
    {
        CannonGK cannon = Instantiate(prefab, bossbehaviour.GetActiveSpawnPosition(), Quaternion.identity).GetComponent<CannonGK>();
        Vector3 direction = (bossbehaviour.GetActiveSpawnPosition() - bossbehaviour.GetCenterRotatePosition()).normalized;
        cannon.Launch(direction, speed);
    }
}
