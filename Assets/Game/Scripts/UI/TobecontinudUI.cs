using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
public class TobecontinudUI : MonoBehaviour
{
    void Start()
    {
        TWTransition.FadeOut();
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
        yield return new WaitForSeconds((float)TimelineManager.instance.Director.duration);
        TWTransition.FadeIn(() => TWLoading.LoadScene("MainMenu"));
    }
}
