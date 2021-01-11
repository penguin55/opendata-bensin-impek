using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TomWill;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DialogMainMenu : MonoBehaviour
{
    public static DialogMainMenu instance;
    [SerializeField] public Color32 blue, brown;
    [SerializeField] private BossManager bossManager;
    [SerializeField] private Button[] bosses;
    [SerializeField] Camera mainCamera;
    [SerializeField] private GameObject bossChoicePanel, itemChoicePanel, dialogPanel, narasiPanel, environment, grass, railTrack, cannon;
    [SerializeField] private GameObject tv, tv1, tv2;
    [SerializeField] private FungusController fungusController;
    [SerializeField] private ListItemUIManager listUIManager;
    [SerializeField] [TextArea(0, 60)] private string[] bossDesc;
    [SerializeField] [TextArea(0, 60)] private string[] itemDesc;
    [SerializeField] private Text boss, item;
    [SerializeField] private int index = 0, bossIndex, itemIndex;
    [SerializeField] private Sprite[] environments, grasses;
    private string activeBoss;
    private void Start()
    {
        TWLoading.OnSuccessLoad(()=>
        {
            fungusController.Init();
            TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_OUT, .5f);
        });
        
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

        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () =>
        {
            BossListTransition();
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_OUT, .5f, ()=>
            {
                fungusController.NextBlock("Weakness"+activeBoss);
            });
        });
    }

    private void BossListTransition()
    {
        tv.SetActive(false);
        tv1.SetActive(false);
        tv2.SetActive(false);
        cannon.SetActive(false);
        railTrack.SetActive(false);
        switch (activeBoss)
        {
            case "Terrorcopter":
                mainCamera.backgroundColor = blue;
                environment.GetComponent<SpriteRenderer>().sprite = environments[0];
                grass.GetComponent<SpriteRenderer>().sprite = grasses[0];
                GameData.ActiveBoss = GameData.BossType.TERRORCOPTER;
                GameData.ActiveBossData = bossManager.bossesData[0];
                break;
            case "GateKeeper":
                mainCamera.backgroundColor = blue;
                cannon.SetActive(true);
                environment.GetComponent<SpriteRenderer>().sprite = environments[1];
                grass.GetComponent<SpriteRenderer>().sprite = grasses[1];
                GameData.ActiveBoss = GameData.BossType.GATEKEEPER;
                GameData.ActiveBossData = bossManager.bossesData[1];
                break;
            case "Chariot":
                mainCamera.backgroundColor = brown;
                environment.SetActive(false);
                grass.SetActive(false);
                railTrack.SetActive(true);
                GameData.ActiveBoss = GameData.BossType.UNHOLYCHARIOT;
                GameData.ActiveBossData = bossManager.bossesData[2];
                break;
            case "HeadHunter":
                environment.GetComponent<SpriteRenderer>().sprite = environments[3];
                grass.GetComponent<SpriteRenderer>().sprite = grasses[3];
                GameData.ActiveBoss = GameData.BossType.HEADHUNTER;
                GameData.ActiveBossData = bossManager.bossesData[3];
                break;
        }
    }

    public void ChangeEnvironment()
    {
        tv.SetActive(false);
        tv1.SetActive(false);
        tv2.SetActive(false);
        switch (GameData.ActiveBoss)
        {
            case GameData.BossType.TERRORCOPTER:
                environment.GetComponent<SpriteRenderer>().sprite = environments[0];
                grass.GetComponent<SpriteRenderer>().sprite = grasses[0];
                break;
            case GameData.BossType.GATEKEEPER:
                environment.GetComponent<SpriteRenderer>().sprite = environments[1];
                grass.GetComponent<SpriteRenderer>().sprite = grasses[1];
                break;
            case GameData.BossType.UNHOLYCHARIOT:
                mainCamera.backgroundColor = brown;
                environment.SetActive(false);
                grass.SetActive(false);
                railTrack.SetActive(true);
                break;
            case GameData.BossType.HEADHUNTER:
                environment.GetComponent<SpriteRenderer>().sprite = environments[3];
                grass.GetComponent<SpriteRenderer>().sprite = grasses[3];
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

    public void GoToScene(string nameScene, TWTransition.TransitionType type)
    {
        TWTransition.ScreenTransition(type, .5f, () => DOVirtual.DelayedCall(0.5f, ()=> SceneManager.LoadScene(nameScene))); 
    }

    public void GoToScene(string nameScene)
    {
        TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_IN, .5f, () => DOVirtual.DelayedCall(0.5f, () => SceneManager.LoadScene(nameScene)));
    }

    public void OpenBossPanel(bool active)
    {
        bossChoicePanel.SetActive(active);
        for (int i = 0; i < bosses.Length; i++)
        {
            bosses[i].interactable = !bossManager.bossesData[i].wasDie;
        }
    }

    public void OpenItemPanel(bool active)
    {
        listUIManager.Render();
        itemChoicePanel.SetActive(active);
    }

    public void StopBGM()
    {
        TWAudioController.StopBGMPlayed("BGM", false);
    }

    public void OpenDialogPanel(bool active)
    {
        dialogPanel.SetActive(active);
    }

    public void PlaySFX(string name)
    {
        float audioLength = TWAudioController.AudioLength(name, "SFX");
        DOTween.Sequence()
            .AppendCallback(() => TWAudioController.PlaySFX("SFX_Boss", name))
            .PrependInterval(audioLength)
            .SetLoops(-1);
    }
}
