using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public List<BossData> bossesData = new List<BossData>();

    public void ResetBossState()
    {
        foreach (BossData data in bossesData)
        {
            data.wasDie = false;
        }
    }
}
