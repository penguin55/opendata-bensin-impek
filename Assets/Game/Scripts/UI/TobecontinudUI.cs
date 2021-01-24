using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
using UnityEngine.Playables;

public class TobecontinudUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogFungus;
    [SerializeField] private PlayableDirector end;
    void Start()
    {
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_OUT);
        GameTime.GlobalTimeScale = 1f;
    }

    void Update()
    {
        End();
    }

    public void End()
    {
        StartCoroutine(ToMainMenu());
    }

    IEnumerator ToMainMenu()
    {
        if (end.duration >= 21)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BackToMenu();
            }
        }
        yield return new WaitForSeconds((float)end.duration);
        BackToMenu();
    }

    public void OpenDialogPanel(bool active)
    {
        dialogFungus.SetActive(active);
    }

    public void BackToMenu()
    {
        GameTime.GlobalTimeScale = 1f;
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () => TWLoading.LoadScene("EndingScene"));
        TWAudioController.PlaySFX("UI", "transition");
    }
}
