using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public BossManager bossManager;
    public static ItemManager instance;
    [SerializeField] private ItemData[] baseItems;

    [SerializeField] private ItemData[] itemCollections;

    private void Start()
    {
        instance = this;

        if (GameData.FirstPlay)
        {
            bossManager.ResetBossState();
            GameData.FirstPlay = false;
            GameData.ItemHolds = new List<ItemData>();
            GameData.ItemUsed = new List<ItemData>();

            ResetAllItem();
            GameTrackRate.ResetTrack();

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
            data.onDelay = false;
        }
    }

    public void ResetAllItem()
    {
        foreach (ItemData data in itemCollections)
        {
            data.wasUsed = false;
            data.onDelay = false;
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
