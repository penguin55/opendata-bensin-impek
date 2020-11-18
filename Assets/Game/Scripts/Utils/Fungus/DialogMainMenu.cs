using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TomWill;
public class DialogMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject bossChoicePanel, itemChoicePanel, dialogPanel, narasiPanel;
    [SerializeField] private FungusController fungusController;
    [SerializeField] [TextArea(0, 30)] private string[] bossDesc;
    [SerializeField] [TextArea(0, 30)] private string[] itemDesc;
    [SerializeField] private Text boss, item;
    [SerializeField] private int index = 0, bossIndex, itemIndex;
    private string activeBoss;
    private void Start()
    {
        TWTransition.FadeOut(() => fungusController.Init());
    }
    public void ItemDesc()
    {
        item.text = itemDesc[itemIndex];
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

    public void SelectItem(string itemName)
    {
        switch (itemName)
        {
            case "deer":
                itemIndex = 0;
                ItemDesc();
                break;
            case "pair":
                itemIndex = 1;
                ItemDesc();
                break;
            case "laklak":
                itemIndex = 2;
                ItemDesc();
                break;
            case "axe":
                itemIndex = 3;
                ItemDesc();
                break;
            case "helmet":
                itemIndex = 4;
                ItemDesc();
                break;
            case "bird":
                itemIndex = 5;
                ItemDesc();
                break;
            case "tiger":
                itemIndex = 6;
                ItemDesc();
                break;
            case "kalimantan":
                itemIndex = 7;
                ItemDesc();
                break;
            case "barong":
                itemIndex = 8;
                ItemDesc();
                break;
        }
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
        itemChoicePanel.SetActive(active);
    }

    public void OpenDialogPanel(bool active)
    {
        dialogPanel.SetActive(active);
    }

    
}
