using System.Collections.Generic;

public static class GameData
{
    public static bool FirstPlay = true;
    public static ItemData ActiveItem;
    public static BossData ActiveBossData;
    public static List<ItemData> ItemHolds;
    public static List<ItemData> ItemUsed;

    public enum BossType
    {
        TERRORCOPTER,
        HEADHUNTER,
        GATEKEEPER,
        UNHOLYCHARIOT
    }

    public static void ShiftItemList(bool update = true) {
        if (ItemHolds == null && ItemUsed == null) return;
        if (update)
        {
            ActiveItem.wasUsed = true;
            if (ItemHolds.Contains(ActiveItem)) ItemHolds.Remove(ActiveItem);
            if (!ItemUsed.Contains(ActiveItem)) ItemUsed.Add(ActiveItem);
        } else
        {
            if (ItemUsed.Contains(ActiveItem)) ItemUsed.Remove(ActiveItem);
            if (!ItemHolds.Contains(ActiveItem)) ItemHolds.Add(ActiveItem);
        }
    }

    public static BossType ActiveBoss;
}
