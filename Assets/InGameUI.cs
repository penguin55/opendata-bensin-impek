using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pauseMenuUI, DialogUI;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private Sprite newsprite;
    [SerializeField] private Sprite oldsprite;
    [SerializeField][TextArea] private string [] chat;
    [SerializeField] private Text dialog;
    [SerializeField] private int index=0;

    public static InGameUI instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        TWLoading.OnSuccessLoad(() => {
            TWTransition.FadeOut();
        });
        Time.timeScale = 0f;
        DialogUI.SetActive(true);
        uilive();
    }

    public void next()
    {
        index++;
    }

    public void Dialog()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DialogUI.SetActive(false);
            Time.timeScale = 1f;
        }
        if (index < chat.Length) dialog.text = chat[index];
        if(index>= chat.Length)
        {
            DialogUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Dialog();
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
        TWTransition.FadeIn(() => TWLoading.LoadScene("BossTest"));
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
