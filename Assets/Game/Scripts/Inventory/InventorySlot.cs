using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    [Header("UI Stuff to change")]
    [SerializeField] private Text itemNumberText;
    [SerializeField] private Image itemImage;

    [Header("Variable from the text")]
    public InventoryItem thisItem;
    public InventoryManager thisManager;

    public void Setup(InventoryItem newItem, InventoryManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;
        if (thisItem)
        {
            itemImage.sprite = thisItem.itemSprite;
            itemNumberText.text = "" + thisItem.numberHeld;
        }
    }

    public void ClickedOn()
    {
        if (thisItem)
        {
            thisManager.SetupDescriptionAndButton(thisItem.itemDescription, thisItem.usable, thisItem);
        }
    }
}
