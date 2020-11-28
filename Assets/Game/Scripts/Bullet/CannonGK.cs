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
        transform.localEulerAngles = new Vector3(0f, 0f, GetAngle(direction));
        DOVirtual.DelayedCall(3, () => { OnExit_State(); }).SetId("Cannon"+transform.GetInstanceID());
        OnEnter_State();
    }

    private void Update()
    {
        if (launch) transform.Translate(direction * speed * GameTime.LocalTime, Space.World);
    }

    private float GetAngle(Vector3 difference)
    {
        difference.Normalize();
        return (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg) + 90;
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
