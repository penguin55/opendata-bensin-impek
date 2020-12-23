using System.Collections.Generic;

public static class GameData
{
    public static bool FirstPlay = true;
    public static ItemData ActiveItem;
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
        if (update)
        {
            ItemHolds.Remove(ActiveItem);
            ItemUsed.Add(ActiveItem);
        } else
        {
            ItemUsed.Remove(ActiveItem);
            ItemHolds.Add(ActiveItem);
        }
    }

    public static BossType ActiveBoss;
}
