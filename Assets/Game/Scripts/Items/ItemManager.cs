using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    [SerializeField] private ItemData[] baseItems;

    private void Start()
    {
        instance = this;

        if (GameData.FirstPlay)
        {
            GameData.FirstPlay = false;
            GameData.ItemHolds = new List<ItemData>();
            GameData.ItemUsed = new List<ItemData>();

            ResetBaseItem();

            LoadItem(baseItems);
        }

        ValidateItem();
    }

    public void ChooseItem(string itemName)
    {
        GameData.ActiveItem = baseItems.First(e => e.itemName == itemName);
    }

    private void LoadItem(ItemData[] items)
    {
        foreach (ItemData data in items)
        {
            LoadItem(data);
        }
    }

    private void LoadItem(ItemData data)
    {
        if (data.wasUsed) GameData.ItemUsed.Add(data);
        else GameData.ItemHolds.Add(data);
    }

    private void ValidateItem()
    {
        if (GameData.ItemHolds.Count == 0)
        {
            GameData.ItemHolds.Clear();
            GameData.ItemUsed.Clear();

            ResetBaseItem();

            LoadItem(baseItems);
        }
    }

    public void ResetBaseItem()
    {
        foreach (ItemData data in baseItems)
        {
            data.wasUsed = false;
        }
    }

    public void ItemNull()
    {
        GameData.ActiveItem = null;
        GameData.ItemHolds.Clear();
        GameData.ItemUsed.Clear();

        ResetBaseItem();

        LoadItem(baseItems);
    }
}
