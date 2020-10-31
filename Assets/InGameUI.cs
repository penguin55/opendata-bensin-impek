using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject[] hearts;
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
        isPaused = false;
        uilive();
    }

    // Update is called once per frame
    void Update()
    {
        OpenPauseMenu();
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

    public void Resume()
    {
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
        TWTransition.FadeIn(() => TWLoading.LoadScene("SampleScene 1"));
    }

    public void BackToMenu()
    {
        TWTransition.FadeIn(() => TWLoading.LoadScene("MainMenu"));
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
}
