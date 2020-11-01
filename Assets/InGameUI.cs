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
    [SerializeField] [TextArea(0, 30)] private string[] chat;
    [SerializeField] [TextArea(0, 30)] private string[] charname;
    [SerializeField] private Sprite newsprite;
    [SerializeField] private Sprite oldsprite;
    [SerializeField] private GameObject x, commander, heli, mysterious;
    [SerializeField] private int index = 0;
    [SerializeField] private Text dialog, chara;


    public static InGameUI instance;

    // Start is called before the first frame update
    void Start()
    {
        GameVariables.GAME_OVER = false;
        index = 0;
        instance = this;
        TWLoading.OnSuccessLoad(() => {
            TWTransition.FadeOut();
        });
        TWAudioController.PlayBGM("VsBoss", TWAudioController.PlayType.AUTO);
        uilive();
        UpdateHpBos(BossBehaviour.Instance.health);
    }

  

    // Update is called once per frame
    void Update()
    {
        OpenPauseMenu();
        if (Input.GetKeyDown(KeyCode.Space) && dialogUI.activeSelf)
        {
            TWAudioController.PlaySFX("click");
            SceneManager.LoadScene("ToBeContinued");
            TWAudioController.PlaySFX("transition");
            dialogUI.SetActive(false);
            TWTransition.FadeIn();
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
            TWAudioController.PlaySFX("click");
            SceneManager.LoadScene("ToBeContinued");
            TWAudioController.PlaySFX("transition");
            dialogUI.SetActive(false);
            TWTransition.FadeIn();

        }
    }


    public void next()
    {
        TWAudioController.PlaySFX("click");
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
        TWAudioController.PlaySFX("click");
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
        TWAudioController.PlaySFX("click");
        TWTransition.FadeIn(() => TWLoading.LoadScene("BossTest"));
        TWAudioController.PlaySFX("transition");
    }

    public void BackToMenu()
    {
        TWAudioController.PlaySFX("click");
        TWTransition.FadeIn(() => TWLoading.LoadScene("MainMenu"));
        TWAudioController.PlaySFX("transition");
    }

    public void uilive()
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

    public void UpdateHpBos(int health)
    {
        int i = 0;
        while (i < 3)
        {
            heartsBos[i].SetActive(true);
            if (i < health)
            {
                heartsBos[i].GetComponent<Image>().overrideSprite = oldsprite;
            }
            else
            {
                heartsBos[i].GetComponent<Image>().overrideSprite = newsprite;
            }
            i++;
        }
    }
}