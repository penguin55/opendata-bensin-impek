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
        TWAudioController.PlaySFX("click");
        TWTransition.FadeIn(() => TWLoading.LoadScene("dialog"));
        TWAudioController.PlaySFX("transition");
    }

    public void Keluar()
    {
        TWAudioController.PlaySFX("click");
        Application.Quit();
    }
}

