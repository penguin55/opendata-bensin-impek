using System.Collections;
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
    [SerializeField] private GameObject[] hearts, heartsBos;
    [SerializeField] private GameObject shields;
    [SerializeField] [TextArea(0, 30)] private string[] chat;
    [SerializeField] [TextArea(0, 30)] private string[] charname;
    [SerializeField] private Sprite newsprite, newboss;
    [SerializeField] private Sprite oldsprite, oldboss;
    [SerializeField] private GameObject x, commander, heli, mysterious;
    [SerializeField] private int index = 0;
    [SerializeField] private Text dialog, chara;

    [SerializeField] private Sprite aPair, axe, deer, helmet, laklak, terrorcopter, gatekeeper, chariot, headhunter;
    [SerializeField] private Image activateItemImage;

    [Header("Obtain Drop UI")]
    [SerializeField] private Image dropImage;
    [SerializeField] private Text dropItemNameText;
    [SerializeField] private Text dropItemDescText;
    [SerializeField] private string bgm;
    public static InGameUI instance;

    // Start is called before the first frame update
    void Start()
    {
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
        PlayBGM(bgm);
        ItemImage();
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
        if (GameData.ActiveItem.CheckIsOneTimeUse())
        {
            activateItemImage.color = Color.red;
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


    public void OpenPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        GameTime.GlobalTimeScale = 1f;
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () => TWLoading.LoadScene("MainMenu"));
        TWAudioController.PlaySFX("UI", "transition");
    }

    public void BackToPanelBoss()
    {
        GameVariables.DIALOG_START_MESSAGE = "BOSS_PANEL";
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
}