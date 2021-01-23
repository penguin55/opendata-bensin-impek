using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaData : MonoBehaviour
{
   private bool isDashing;
   [SerializeField]private float speed, dashSpeed, baseDashSpeed, baseDashDelay;

    public static bool canChargeDash, chargeDash;

    public static  float hp = 3, maxhp = 3, shield = 0, chargeTimeDash, maxChargeTimeDash, multiplierChargeDash;

    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float Speed { get => speed + GameVariables.SPEED_BUFF; set => speed = value; }
    public float Hp { get => hp; set => hp = value; }
    public float Shield { get => shield; set => shield = value; }
    public float BaseDashDelay { get => baseDashDelay; set => baseDashDelay = value; }
    public float ChargeTimeDash { get => chargeTimeDash; set => chargeTimeDash = value; }
    public bool CanChargeDash { get => canChargeDash; set => canChargeDash = value; }
    public bool ChargeDash { get => chargeDash; set => chargeDash = value; }
    public float MaxChargeTimeDash { get => maxChargeTimeDash; set => maxChargeTimeDash = value; }
    public float BaseDashSpeed { get => baseDashSpeed; set => baseDashSpeed = value; }
    public float MultiplierChargeDash { get => multiplierChargeDash; set => multiplierChargeDash = value; }

    public void ResetStatus()
    {
        canChargeDash = false;
        chargeDash = false;
    }
}
