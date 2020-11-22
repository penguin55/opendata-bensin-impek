using UnityEngine;
using TomWill;
public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        TWTransition.FadeOut();
        TWAudioController.PlayBGM("BGM", "MainMenu", TWAudioController.PlayType.TRANSITION);
    }
    public void PlayGame()
    {
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.FadeIn(() => TWLoading.LoadScene("dialogFungus"));
        TWAudioController.PlaySFX("UI", "transition");
    }

    public void Keluar()
    {
        TWAudioController.PlaySFX("UI", "click");
        Application.Quit();
    }
}

