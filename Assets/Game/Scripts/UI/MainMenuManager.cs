using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TomWill;
public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        TWTransition.FadeOut();
        TWAudioController.PlayBGM("MainMenu", TWAudioController.PlayType.AUTO);
    }
    public void PlayGame()
    {
        TWTransition.FadeIn(() => TWLoading.LoadScene("dialog"));
    }

    public void Keluar()
    {
        Application.Quit();
    }
}

