using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTrackGame : MonoBehaviour
{
    public static DebugTrackGame Instance;

    public float gameplayBoss;
    public int gameplayDeath;
    public List<ItemData> itemUsed = new List<ItemData>();
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameplayBoss = GameTrackRate.Time;
        gameplayDeath = GameTrackRate.DeathCount;
        itemUsed = GameTrackRate.ItemUsed;
        time = GameTrackRate.CurrentTime;
    }
}
