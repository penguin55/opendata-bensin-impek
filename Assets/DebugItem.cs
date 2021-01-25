using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugItem : MonoBehaviour
{
    public List<ItemData> ItemHolds;
    public List<ItemData> ItemUsed;

    // Update is called once per frame
    void Update()
    {
        ItemHolds = GameData.ItemHolds;
        ItemUsed = GameData.ItemUsed;
    }
}
