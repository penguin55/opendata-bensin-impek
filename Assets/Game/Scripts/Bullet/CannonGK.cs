using DG.Tweening;
using UnityEngine;

public class CannonGK : DamageArea
{
    private Vector3 direction;
    private float speed;
    private bool launch;

    public void Launch(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        DOVirtual.DelayedCall(3, () => { OnExit_State(); }).SetId("Cannon"+transform.GetInstanceID());
        OnEnter_State();
    }

    private void Update()
    {
        if (launch) transform.Translate(direction * speed * GameTime.LocalTime);
    }

    protected override void OnEnter_State()
    {
        launch = true;
        base.OnEnter_State();
    }

    protected override void OnExit_State()
    {
        base.OnExit_State();
    }
}
