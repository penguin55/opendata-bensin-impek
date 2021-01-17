using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager instance;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private RuntimeAnimatorController runtime;
    [SerializeField] private PlayableDirector terrorcopter, gatekeeper, headhunter, unholyChariot;
    private PlayableDirector currentDirector;
    public PlayableDirector Director { get => currentDirector; set => currentDirector = value; }

    void Start()
    {
        instance = this;
    }

    public void PlayDirector()
    {
        Debug.Log(GameData.ActiveBoss);
        switch (GameData.ActiveBoss)
        {
            case GameData.BossType.TERRORCOPTER:
                currentDirector = terrorcopter;
                break;
            case GameData.BossType.GATEKEEPER:
                currentDirector = gatekeeper;
                break;
            case GameData.BossType.UNHOLYCHARIOT:
                currentDirector = unholyChariot;
                break;
            case GameData.BossType.TIDEMASTER:
                currentDirector = headhunter;
                break;
        }
        currentDirector.Play();
    }

    public void StopDirector()
    {
        currentDirector.Stop();
    }
}
