using UnityEngine;
using TomWill;
public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_OUT);
        TWAudioController.PlayBGM("BGM", "MainMenu", TWAudioController.PlayType.TRANSITION);
    }
    public void PlayGame()
    {
        GameVariables.DIALOG_START_MESSAGE = "MISSION_START";
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () => TWLoading.LoadScene("dialogFungus"));
        TWAudioController.PlaySFX("UI", "transition");
    }

    public void Keluar()
    {
        TWAudioController.PlaySFX("UI", "click");
        Application.Quit();
    }
}

