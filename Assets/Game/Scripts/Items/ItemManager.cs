using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager manager;
    [SerializeField] private ItemData[] items;

    private static ItemData activeItem;
    [SerializeField] private Image activateItemImage;

    private void Start()
    {
        manager = this;
        if (activateItemImage) activateItemImage.color = activeItem ? Color.white : Color.red;
    }

    public void ChooseItem(string itemName)
    {
        activeItem = items.First(e => e.itemName == itemName);
    }

    public void ActivateItem()
    {
        if (activeItem)
        {
            if (activateItemImage.color != Color.red)
            {
                bool status = activeItem.TakeEffect();

                if (status)
                {

                }
                else
                {

                }

                if (activeItem.CheckIsOneTimeUse())
                {
                    activateItemImage.color = Color.red;
                }
            }
        }
    }

    public ItemData GetActiveItem()
    {
        return activeItem;
    }
}
