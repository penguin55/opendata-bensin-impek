using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingDebugger : MonoBehaviour
{
    public static RatingDebugger Instance;
    [SerializeField] private RatingUIManager ratingSystem;

    [SerializeField] private float time;
    [SerializeField] private int deathCount;

    [SerializeField] private List<ItemData> items;

    private void Start()
    {
        Instance = this;
    }

    [ContextMenu("Init")]
    public void InitToGame()
    {
        GameTrackRate.StartTimePlay = time;
        GameTrackRate.EndTimePlay = time + time;
        GameTrackRate.CalculateTime();

        GameTrackRate.DeathCount = deathCount;

        if (items == null) items = new List<ItemData>();
        GameTrackRate.ItemUsed = items;

        ratingSystem.RenderRating();
    }
}
