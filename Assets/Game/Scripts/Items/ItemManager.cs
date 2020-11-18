using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemData[] baseItems;

    private void Start()
    {
        if (GameData.FirstPlay)
        {
            GameData.FirstPlay = false;
            GameData.ItemHolds = new List<ItemData>();
            GameData.ItemUsed = new List<ItemData>();
            GameData.ItemHolds.AddRange(baseItems);
        }
    }

    public void ChooseItem(string itemName)
    {
        GameData.ActiveItem = baseItems.First(e => e.itemName == itemName);
    }
}
