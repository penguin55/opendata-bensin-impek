using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaData : MonoBehaviour
{
   private bool isDashing;
   [SerializeField]private float speed, dashSpeed, baseDashDelay;

    public static  float hp = 3, maxhp = 3, shield = 0;

    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float Speed { get => speed + GameVariables.SPEED_BUFF; set => speed = value; }
    public float Hp { get => hp; set => hp = value; }
    public float Shield { get => shield; set => shield = value; }
    public float BaseDashDelay { get => baseDashDelay; set => baseDashDelay = value; }
}
