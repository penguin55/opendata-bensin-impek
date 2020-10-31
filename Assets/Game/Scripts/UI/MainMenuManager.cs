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
    }
    public void PlayGame()
    {
        TWTransition.FadeIn(() => TWLoading.LoadScene("SampleScene 1"));
    }

    public void Keluar()
    {
        Application.Quit();
    }
}

