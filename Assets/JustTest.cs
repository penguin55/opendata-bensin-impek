using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

[ExecuteInEditMode]
public class JustTest : MonoBehaviour
{
    public string move;

    public bool damage;

    public Transform moveTool1;
    public Transform moveTool2;
    public Transform moveTool3;

    private List<Vector2> positionCurve = new List<Vector2>();

    private void Update()
    {
        //Chariot boss = BossBehaviour.Instance as Chariot;
        //boss.animationTweening.MoveTrain(move);

        //if (damage)
        //{
        //    damage = false;
        //    BossBehaviour.Instance.TakeDamage();
        //}
        Vector2 controlPoint = ExtendMath.GetPerpendicular(moveTool1.position, moveTool2.position, 15f, ExtendMath.PerpendicularType.ALWAYS_UP);
        moveTool3.position = controlPoint;
        ExtendMath.BezierCurve(ref positionCurve, moveTool1.position, moveTool2.position, moveTool3.position, 10);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(moveTool1.position, moveTool2.position);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(moveTool1.position, 1f);
        Gizmos.DrawSphere(moveTool2.position, 1f);

        

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(moveTool3.position, 1f);

        Gizmos.color = Color.yellow;
        foreach(Vector2 data in positionCurve)
        {
            Gizmos.DrawSphere(data, 1f);
        }
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(moveTool1.position, center);
        //Gizmos.DrawLine(moveTool2.position, center);
    }
}
