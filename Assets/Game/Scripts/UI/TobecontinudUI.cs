using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
public class TobecontinudUI : MonoBehaviour
{
    void Start()
    {
        TWTransition.FadeOut();
        Time.timeScale = 1f;
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
                TWTransition.FadeIn(() => TWLoading.LoadScene("MainMenu"));
            }
        }
        yield return new WaitForSeconds((float)TimelineManager.instance.Director.duration);
        TWTransition.FadeIn(() => TWLoading.LoadScene("MainMenu"));
    }
}
