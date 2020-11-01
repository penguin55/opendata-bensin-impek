using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pauseMenuUI, gameOverUI, WinGameUI;
    [SerializeField] private GameObject[] hearts, heartsBos;
    [SerializeField] private Sprite newsprite;
    [SerializeField] private Sprite oldsprite;
    

    public static InGameUI instance;

    // Start is called before the first frame update
    void Start()
    {
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
        GameOver();
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