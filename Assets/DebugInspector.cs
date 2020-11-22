using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInspector : MonoBehaviour
{
    public List<ItemData> itemhold, itemused;

    private void Update()
    {
        itemhold = GameData.ItemHolds;
        itemused = GameData.ItemUsed;
    }
}
