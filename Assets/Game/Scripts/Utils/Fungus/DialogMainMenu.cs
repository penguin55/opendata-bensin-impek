using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TomWill;
public class DialogMainMenu : MonoBehaviour
{
    public static DialogMainMenu instance;

    [SerializeField] private GameObject bossChoicePanel, itemChoicePanel, dialogPanel, narasiPanel;
    [SerializeField] private FungusController fungusController;
    [SerializeField] private ListItemUIManager listUIManager;
    [SerializeField] [TextArea(0, 30)] private string[] bossDesc;
    [SerializeField] [TextArea(0, 30)] private string[] itemDesc;
    [SerializeField] private Text boss, item;
    [SerializeField] private int index = 0, bossIndex, itemIndex;
    private string activeBoss;
    private void Start()
    {
        TWTransition.FadeOut(() => fungusController.Init());
        instance = this;
    }
    public void ItemDesc(string desc)
    {
        item.text = desc;
    }
    public void BossDesc()
    {
        boss.text = bossDesc[bossIndex];
    }

    public void ConfirmSelectedBoss()
    {
        OpenBossPanel(false);
        switch (activeBoss)
        {
            case "Terrorcopter":
                GameData.ActiveBoss = GameData.BossType.TERRORCOPTER;
                fungusController.NextBlock("WeaknessTerrorcopter");
                break;
            case "GateKeeper":
                GameData.ActiveBoss = GameData.BossType.GATEKEEPER;
                fungusController.NextBlock("WeaknessGateKeeper");
                break;
            case "Chariot":
                GameData.ActiveBoss = GameData.BossType.UNHOLYCHARIOT;
                fungusController.NextBlock("WeaknessChariot");
                break;
            case "HeadHunter":
                GameData.ActiveBoss = GameData.BossType.HEADHUNTER;
                fungusController.NextBlock("WeaknessHeadHunter");
                break;
        }
        
    }

    public void SelectBoss(string bossname)
    {
        activeBoss = bossname;
        switch (bossname)
        {
            case "Terrorcopter":
                bossIndex = 0;
                BossDesc();
                break;
            case "GateKeeper":
                bossIndex = 1;
                BossDesc();
                break;
            case "Chariot":
                bossIndex = 2;
                BossDesc();
                break;
            case "HeadHunter":
                bossIndex = 3;
                BossDesc();
                break;
        }
    }

    public void SelectItem(ItemData data)
    {
        itemIndex = data.itemId;
        ItemDesc(data.itemDesc);
        GameData.ActiveItem = data;
    }

    public void GoToScene(string nameScene)
    {
        TWTransition.FadeIn(() => TWLoading.LoadScene(nameScene));
    }

    public void OpenBossPanel(bool active)
    {
        bossChoicePanel.SetActive(active);
    }

    public void OpenItemPanel(bool active)
    {
        listUIManager.Render();
        itemChoicePanel.SetActive(active);
    }

    public void OpenDialogPanel(bool active)
    {
        dialogPanel.SetActive(active);
    }

    
}
