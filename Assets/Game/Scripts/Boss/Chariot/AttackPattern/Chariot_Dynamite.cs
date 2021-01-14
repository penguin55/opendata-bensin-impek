using Pathfinding.Util;
using UnityEditor;
using UnityEngine;

public class Chariot_Dynamite : MonoBehaviour
{
    [SerializeField] private GameObject dynamitePrefabs;
    [SerializeField] private Transform[] dropPosition;
    [SerializeField] private float time;

    public void SpawnDynamite()
    {
        DynamiteChariot dynamite = Instantiate(dynamitePrefabs, dropPosition[0].position, Quaternion.identity).GetComponent<DynamiteChariot>();
        dynamite.Init(dropPosition, time);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < dropPosition.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(dropPosition[i].position, 0.1f);

            if (i < dropPosition.Length - 1)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(dropPosition[i].position, dropPosition[i+1].position);
            }
        }
    }
}
