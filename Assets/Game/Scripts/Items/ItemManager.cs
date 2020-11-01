using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemData[] items;

    private static ItemData activeItem;
    [SerializeField] private Button buttonActive;

    private void Start()
    {
        if (buttonActive) buttonActive.interactable = activeItem != null;
    }

    public void ChooseItem(string itemName)
    {
        activeItem = items.First(e => e.itemName == itemName);
    }

    public void ActivateItem()
    {
        if (activeItem)
        {
            activeItem.TakeEffect();
            if (activeItem.CheckIsOneTimeUse()) buttonActive.interactable = false;
        }
    }
}
