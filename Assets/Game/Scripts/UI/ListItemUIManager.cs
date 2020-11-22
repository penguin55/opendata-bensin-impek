using Fungus;
using UnityEngine;
using UnityEngine.UI;

public class ListItemUIManager : MonoBehaviour
{
    [SerializeField] private GameObject placeholderItem;
    [SerializeField] private Transform contentViewList;
    [SerializeField] private Button confirmChooseItem;
    [SerializeField] private ScrollRect scrollRect;
    private RectTransform elementAnchor;
    private float baseSize = 5;
    private float startValue;

    [Header("Visualize Section")]
    [SerializeField] private Color availableItemColor;
    [SerializeField] private Color unavailableItemColor;

    public void Render()
    {
        int itemsSize = (GameData.ItemHolds != null ? GameData.ItemHolds.Count : 0) + (GameData.ItemUsed != null ? GameData.ItemUsed.Count : 0);

        if (GameData.ItemHolds != null) {
            foreach (ItemData item in GameData.ItemHolds)
            {
                SetPlaceholder(item);
            }
        }

        if (GameData.ItemUsed != null)
        {
            foreach (ItemData item in GameData.ItemUsed)
            {
                SetPlaceholder(item);
            }
        }

        SetStartPosition(itemsSize);
    }

    private void SetStartPosition(int itemSize)
    {
        if (itemSize > baseSize)
        {
            float widthModify = elementAnchor.sizeDelta.x * (itemSize - baseSize);
            startValue = -120;
            scrollRect.content.anchoredPosition = new Vector2(startValue, scrollRect.content.anchoredPosition.y);
            scrollRect.content.sizeDelta = new Vector2(widthModify, scrollRect.content.sizeDelta.y);
        }
    }

    private void SetPlaceholder(ItemData data)
    {
        GameObject instanceObject = Instantiate(placeholderItem, contentViewList);
        Image instanceImage = instanceObject.GetComponent<Image>();
        Button instanceButton = instanceObject.GetComponent<Button>();

        instanceImage.sprite = data.image;
        instanceImage.color = data.wasUsed ? unavailableItemColor : availableItemColor;
        instanceButton.onClick.AddListener(() => DialogMainMenu.instance.SelectItem(data));
        instanceButton.onClick.AddListener(() => confirmChooseItem.gameObject.SetActive(!data.wasUsed));

        if (!elementAnchor) elementAnchor = instanceObject.GetComponent<RectTransform>();
    }
}
