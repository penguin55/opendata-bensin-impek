using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Fungus;

public class TrainingUI : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pauseMenuUI, gameOverUI, dialogUI, dialogFungus, dialogtry, obtainItemBoss, heartContainer;
    [SerializeField] private GameObject[] hearts, heartsBos;
    [SerializeField] private GameObject shields;
    [SerializeField] private Sprite newsprite, newboss;
    [SerializeField] private Sprite oldsprite, oldboss;
    [SerializeField] private Sprite aPair, axe, deer, helmet, laklak, terrorcopter, gatekeeper, chariot, tidemaster;
    [SerializeField] private Image activateItemImage;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private Image timeFill;

    [Header("Obtain Drop UI")]
    [SerializeField] private Image dropImage;
    [SerializeField] private Text dropItemNameText;
    [SerializeField] private Text dropItemDescText;
    [SerializeField] private string bgm;
    public static TrainingUI instance;

    // Start is called before the first frame update
    void Start()
    {
        GameTime.GlobalTimeScale = 1;
        GameVariables.GAME_OVER = false;
        instance = this;
        TWLoading.OnSuccessLoad(() => {
            TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_OUT, .5f, ()=>
            {
                GameVariables.GAME_FREEZE = false;
            });
        });
        PlayBGM(bgm);
        if (GameData.ActiveItem) ItemImage();
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void PlayBGM(string name)
    {
        TWAudioController.PlayBGM("BGM", name, TWAudioController.PlayType.TRANSITION);
    }


    // Update is called once per frame
    void Update()
    {
        OpenPauseMenu();
    }

    public void UpdateItemImage()
    {
        if (GameData.ActiveItem.wasUsed)
        {
            activateItemImage.DOFade(0.5f , 0);
        } else
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

    public void TryAgain(bool active)
    {
        dialogtry.SetActive(active);
        FreezeInput(active);
    }

    public void OpenItemPanel(bool active)
    {
        obtainItemBoss.SetActive(active);
        FreezeInput(false);
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
            GameTime.GlobalTimeScale = 0f;
        }
        else
        {
            gameOverUI.SetActive(false);
        }
    }

    public void Resume()
    {
        TWAudioController.PlaySFX("UI", "click");
        pauseMenuUI.SetActive(false);
        GameTime.GlobalTimeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameTime.GlobalTimeScale = 0f;
        isPaused = true;
    }

    public void Restart(string name)
    {
        GameTime.GlobalTimeScale = 1f;
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () => TWLoading.LoadScene(name));
        TWAudioController.PlaySFX("UI", "transition");
    }

    public void BackToMenu()
    {
        ItemManager.instance.ItemNull();
        GameTime.GlobalTimeScale = 1f;
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_IN, .5f, () => TWLoading.LoadScene("MainMenu"));
        TWAudioController.PlaySFX("UI", "transition");
    }

    public void BackToPanelBoss()
    {
        GameVariables.DIALOG_START_MESSAGE = "BOSS_PANEL";
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_IN, .5f, () => TWLoading.LoadScene("dialogFungus"));
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

    public void ActivateHeart(bool active)
    {
        heartContainer.SetActive(active);
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
