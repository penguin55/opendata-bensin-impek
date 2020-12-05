using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform center;
    [SerializeField] private float radius;
    private bool activateRotate;
    private GameObject targetMove;

    private Vector3 newPosition;
    private Vector3 offsetDir;

    public void ActiveFollow(bool flag)
    {
        activateRotate = flag;
    }

    private void Start()
    {
        targetMove = CharaController.instance.gameObject;
    }

    void Update()
    {
        if (activateRotate && targetMove)
        {
            LookAtTarget();

            transform.Translate(speed * transform.right * GameTime.LocalTime, Space.World);

            newPosition = transform.position;

            offsetDir = newPosition - center.position;
            offsetDir.Normalize();

            newPosition = offsetDir * radius;

            transform.position = newPosition;
        }
    }

    private void LookAtTarget()
    {
        Vector3 diff = targetMove.transform.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }
}
