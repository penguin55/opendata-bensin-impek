using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemData[] items;

    private static ItemData activeItem;

    public void ChooseItem(string itemName)
    {
        if (activeItem) activeItem = items.First(e => e.itemName == itemName);
    }

    public void ActivateItem()
    {
        activeItem.TakeEffect();
    }
}
