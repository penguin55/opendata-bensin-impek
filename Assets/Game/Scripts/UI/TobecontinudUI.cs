using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
public class TobecontinudUI : MonoBehaviour
{
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
        if (TimelineManager.instance.Director.duration >= 21)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, 1f, () => TWLoading.LoadScene("MainMenu"));
            }
        }
        yield return new WaitForSeconds((float)TimelineManager.instance.Director.duration);
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, 1f, () => TWLoading.LoadScene("MainMenu"));
    }
}
