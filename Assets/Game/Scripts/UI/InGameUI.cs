﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class InGameUI : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pauseMenuUI, gameOverUI, dialogUI, dialogFungus, obtainItemBoss;
    [SerializeField] private Text activeSkipText;
    [SerializeField] private GameObject[] hearts, heartsBos;
    [SerializeField] private GameObject shields;
    [SerializeField] [TextArea(0, 30)] private string[] chat;
    [SerializeField] [TextArea(0, 30)] private string[] charname;
    [SerializeField] private Sprite newsprite, newboss;
    [SerializeField] private Sprite oldsprite, oldboss;
    [SerializeField] private GameObject x, commander, heli, mysterious;
    [SerializeField] private int index = 0;
    [SerializeField] private Text dialog, chara;

    [SerializeField] private Sprite aPair, axe, deer, helmet, laklak, terrorcopter, gatekeeper, chariot, tidemaster;
    [SerializeField] private Image activateItemImage;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private Image timeFill;

    [Header("Obtain Drop UI")]
    [SerializeField] private Image dropImage;
    [SerializeField] private Text dropItemNameText;
    [SerializeField] private Text dropItemDescText;

    [Header("Button Restart")]
    [SerializeField] private Button buttonRestart;
    public static InGameUI instance;

    // Start is called before the first frame update
    void Start()
    {
        if (GameData.ActiveItem) GameData.ActiveItem.ResetStatusItem();
        //GameData.ActiveBoss = GameData.BossType.UNHOLYCHARIOT;
        GameTime.GlobalTimeScale = 1;
        GameVariables.GAME_OVER = false;
        index = 0;
        instance = this;
        TWLoading.OnSuccessLoad(() => {
            TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_OUT,.5f, ()=>
            {
                GameVariables.GAME_FREEZE = false;
                GameVariables.FREEZE_INPUT = false;
            });
        });
        
        ItemImage();
    }

    private void OnDisable()
    {
        instance = null;
    }
  

    // Update is called once per frame
    void Update()
    {
        OpenPauseMenu();

        if (GameVariables.ACTIVE_SKIP_CUTSCENE)
        {
            Skip();
        }
    }

    public void UpdateItemImage()
    {
        if (GameData.ActiveItem.wasUsed)
        {
            activateItemImage.DOFade(0.5f, 0);
        }
        else
        {
            activateItemImage.DOFade(1f, 0);
        }
    }

    public void ItemImage()
    {
        if (GameData.ActiveItem.itemName.Contains("Deer"))
        {
            activateItemImage.overrideSprite = deer;
        }
        if (GameData.ActiveItem.itemName.Contains("Helmet"))
        {
            activateItemImage.overrideSprite = helmet;
        }
        if (GameData.ActiveItem.itemName.Contains("Lak"))
        {
            activateItemImage.overrideSprite = laklak;
        }
        if (GameData.ActiveItem.itemName.Contains("Axe"))
        {
            activateItemImage.overrideSprite = axe;
        }
        if (GameData.ActiveItem.itemName.Contains("Pair"))
        {
            activateItemImage.overrideSprite = aPair;
        }
        if (GameData.ActiveItem.itemName.Contains("Tiger"))
        {
            activateItemImage.overrideSprite = gatekeeper;
        }
        if (GameData.ActiveItem.itemName.Contains("Bird"))
        {
            activateItemImage.overrideSprite = terrorcopter;
        }
        if (GameData.ActiveItem.itemName.Contains("Barong"))
        {
            activateItemImage.overrideSprite = chariot;
        }
        if (GameData.ActiveItem.itemName.Contains("Kalimantan"))
        {
            activateItemImage.overrideSprite = tidemaster;
        }
    }

    public void ActivateSkipButton(bool flag)
    {
        GameVariables.ACTIVE_SKIP_CUTSCENE = flag;
        activeSkipText.gameObject.SetActive(flag);
    }

    public void Skip()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateSkipButton(false);
            switch (GameData.ActiveBoss)
            {
                case GameData.BossType.TERRORCOPTER:
                    BackToPanelBoss();
                    break;
                case GameData.BossType.GATEKEEPER:
                    BackToPanelBoss();
                    break;
                case GameData.BossType.UNHOLYCHARIOT:
                    BackToPanelBoss();
                    break;
                case GameData.BossType.TIDEMASTER:
                    BackToPanelBoss();
                    break;
            }
        }
    }

    public void GameWin()
    {
        dialogUI.SetActive(true);
        Dialog();
    }

    public void ChangeImage()
    {
        if (charname[index].Contains("X"))
        {
            x.SetActive(true);
            commander.SetActive(false);
            heli.SetActive(false);
            mysterious.SetActive(false);
        }
        if (charname[index].Contains("Terrorcopter"))
        {
            x.SetActive(false);
            commander.SetActive(false);
            heli.SetActive(true);
            mysterious.SetActive(false);
        }
        if (charname[index].Contains("Colonel"))
        {
            x.SetActive(false);
            commander.SetActive(true);
            heli.SetActive(false);
            mysterious.SetActive(false);
        }
    }

    public void Dialog()
    {
        if (index < chat.Length)
        {
            dialog.text = chat[index];
            chara.text = charname[index];
            ChangeImage();
        }
        //else
        //{
        //    TWAudioController.PlaySFX("UI", "click");
        //    TWAudioController.PlaySFX("UI", "transition");
        //    dialogUI.SetActive(false);
        //    TWTransition.FadeIn(() => SceneManager.LoadScene("ToBeContinued"));
        //}
    }


    public void next()
    {
        TWAudioController.PlaySFX("UI", "click");
        index++;
        if (index == 2)
        {
            obtainItemBoss.SetActive(true);
            dialogUI.SetActive(false);

            RenderObtainDrop(BossBehaviour.Instance.GetDrop());
        }
        else if(obtainItemBoss.activeSelf)
        {
            obtainItemBoss.SetActive(false);
            dialogUI.SetActive(true);
        }
        Dialog();
    }

    public void RenderObtainDrop(ItemData data)
    {
        dropImage.sprite = data.image;
        dropItemNameText.text = data.itemName;
        dropItemDescText.text = data.shortDesc;
    }

    public void FreezeInput(bool flag)
    {
        GameVariables.FREEZE_INPUT = flag;
    }

    public void OpenPauseMenu()
    {
        if (GameData.ActiveBossData.wasDie) return;

        if (Input.GetKeyDown(KeyCode.Escape) && !GameVariables.FREEZE_INPUT)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void OpenDialogPanel(bool active)
    {
        dialogFungus.SetActive(active);
        FreezeInput(active);
    }

    public void OpenItemPanel(bool active)
    {
        obtainItemBoss.SetActive(active);
    }

    public void PlaySFX(string name)
    {
        float audioLength = TWAudioController.AudioLength(name, "SFX");
        DOTween.Sequence()
            .AppendCallback(() => TWAudioController.PlaySFX("SFX_Boss", name))
            .PrependInterval(audioLength)
            .SetLoops(-1)
            .SetId(name);
    }

    public void StopSFX(string name)
    {
        DOTween.Kill(name, true);
    }

    public void GameOver()
    {
        if (CharaController.instance.dead)
        {
            gameOverUI.SetActive(true);

            if (GameData.ItemHolds.Count == 0)
            {
                buttonRestart.interactable = false;
            } else
            {
                buttonRestart.interactable = true;
            }

            GameTime.GlobalTimeScale = 0f;
        }
        else
        {
            gameOverUI.SetActive(false);
        }
    }

    public void Resume()
    {
        GameTrackRate.StartTimePlay = Time.time;

        TWAudioController.PlaySFX("UI", "click");
        pauseMenuUI.SetActive(false);
        GameTime.GlobalTimeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        GameTrackRate.EndTimePlay = Time.time;
        GameTrackRate.CalculateTime();

        pauseMenuUI.SetActive(true);
        GameTime.GlobalTimeScale = 0f;
        isPaused = true;
    }

    public void Restart()
    {
        GameTrackRate.EndTimePlay = Time.time;
        GameTrackRate.CalculateTime();

        BGMStop();
        BackToPanelItem();
    }

    public void BGMStop()
    {
        switch (GameData.ActiveBoss)
        {
            case GameData.BossType.GATEKEEPER:
                TWAudioController.StopBGMPlayed("BGM_BOSS", true);
                
                break;
            case GameData.BossType.TERRORCOPTER:
                TWAudioController.StopBGMPlayed("ENGINE_COPTER", true);
                TWAudioController.StopBGMPlayed("BGM_BOSS", true);
                break;
            case GameData.BossType.UNHOLYCHARIOT:
                TWAudioController.StopBGMPlayed("ENGINE_BIKE", true);
                TWAudioController.StopBGMPlayed("ENGINE_TRAIN", true);
                TWAudioController.StopBGMPlayed("BGM_ADVANCED", true);
                break;
            case GameData.BossType.TIDEMASTER:
                TWAudioController.StopBGMPlayed("BGM_BOSS", true);
                TWAudioController.StopBGMPlayed("ENGINE_SHIP", true);
                break;
        }
    }

    public void BackToMenu()
    {
        ItemManager.instance.ItemNull();
        GameTime.GlobalTimeScale = 1f;
        BGMStop();
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () => TWLoading.LoadScene("MainMenu"));
        TWAudioController.PlaySFX("UI", "transition");
    }

    public void BackToPanelBoss()
    {
        GameTime.GlobalTimeScale = 1f;
        GameVariables.DIALOG_START_MESSAGE = "BOSS_PANEL";
        BGMStop();

        if (GameTrackRate.BossKilled == 4)
        {
            TWAudioController.PlaySFX("UI", "click");
            TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () => TWLoading.LoadScene("ToBeContinued"));
            TWAudioController.PlaySFX("UI", "transition");
        } else
        {
            TWAudioController.PlaySFX("UI", "click");
            TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () => TWLoading.LoadScene("dialogFungus"));
            TWAudioController.PlaySFX("UI", "transition");
        }
    }

    public void BackToPanelItem()
    {
        GameTime.GlobalTimeScale = 1f;
        GameVariables.DIALOG_START_MESSAGE = "ITEM_PANEL";
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () => TWLoading.LoadScene("dialogFungus"));
        TWAudioController.PlaySFX("UI", "transition");
    }

    public void UpdateLive()
    {
        int i = 0;
        while (i < CharaData.maxhp)
        {
            hearts[i].SetActive(true);
            if (i < CharaData.hp)
            {
                hearts[i].GetComponent<Image>().overrideSprite = oldsprite;
            }
            else
            {
                hearts[i].GetComponent<Image>().overrideSprite = newsprite;
            }
            i++;
        }
    }

    public void UpdateShield()
    {
        shields.SetActive(CharaData.shield > 0);
    }

    public void UpdateHpBos(int health)
    {
        int i = 0;
        while (i < 3)
        {
            heartsBos[i].SetActive(true);
            if (i < health)
            {
                heartsBos[i].GetComponent<Image>().overrideSprite = oldboss;
            }
            else
            {
                heartsBos[i].GetComponent<Image>().overrideSprite = newboss;
            }
            i++;
        }
    }

    public void UpdateCooldownTimer(float baseTime, float time)
    {
        //timerInfo.text = "Torpedo Launch in " + (int) time - 1 + " seconds";
        //timeFill.fillAmount = time / baseTime;
        float offset = time / baseTime; ;
        Vector2 scaleFill = new Vector2(offset, 1f);
        timeFill.rectTransform.localScale = scaleFill;
    }

    public void ActivatedCooldownTimer(bool flag)
    {
        timerObject.SetActive(flag);
    }
}