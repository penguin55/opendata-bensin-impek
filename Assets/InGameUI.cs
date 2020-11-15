using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pauseMenuUI, gameOverUI, dialogUI, obtainCoin;
    [SerializeField] private GameObject[] hearts, heartsBos;
    [SerializeField] private GameObject shields;
    [SerializeField] [TextArea(0, 30)] private string[] chat;
    [SerializeField] [TextArea(0, 30)] private string[] charname;
    [SerializeField] private Sprite newsprite, newboss;
    [SerializeField] private Sprite oldsprite, oldboss;
    [SerializeField] private GameObject x, commander, heli, mysterious;
    [SerializeField] private int index = 0;
    [SerializeField] private Text dialog, chara;


    public static InGameUI instance;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        GameVariables.GAME_OVER = false;
        index = 0;
        instance = this;
        TWLoading.OnSuccessLoad(() => {
            TWTransition.FadeOut();
        });
        TWAudioController.PlayBGM("BGM", "VsBoss", TWAudioController.PlayType.TRANSITION);
    }

  

    // Update is called once per frame
    void Update()
    {
        OpenPauseMenu();
        if (Input.GetKeyDown(KeyCode.Space) && dialogUI.activeSelf)
        {
            TWAudioController.PlaySFX("UI", "click");
            TWAudioController.PlaySFX("UI", "transition");
            dialogUI.SetActive(false);
            TWTransition.FadeIn( ()=> SceneManager.LoadScene("ToBeContinued"));
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
        else
        {
            TWAudioController.PlaySFX("UI", "click");
            TWAudioController.PlaySFX("UI", "transition");
            dialogUI.SetActive(false);
            TWTransition.FadeIn(() => SceneManager.LoadScene("ToBeContinued"));
        }
    }


    public void next()
    {
        TWAudioController.PlaySFX("UI", "click");
        index++;
        if (index == 2)
        {
            obtainCoin.SetActive(true);
            dialogUI.SetActive(false);
        }
        else if(obtainCoin.activeSelf)
        {
            obtainCoin.SetActive(false);
            dialogUI.SetActive(true);
        }
        Dialog();
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

    public void GameOver()
    {
        if (CharaController.instance.dead)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
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
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.FadeIn(() => TWLoading.LoadScene("BossTest"));
        TWAudioController.PlaySFX("UI", "transition");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.FadeIn(() => TWLoading.LoadScene("MainMenu"));
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