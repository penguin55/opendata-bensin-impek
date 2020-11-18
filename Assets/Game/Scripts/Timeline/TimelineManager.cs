﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager instance;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private RuntimeAnimatorController runtime;
    [SerializeField] private PlayableDirector director;

    [SerializeField] private PlayableAsset terracopter;
    [SerializeField] private PlayableAsset gatekeeper;
    [SerializeField] private PlayableAsset headhunter;
    [SerializeField] private PlayableAsset unholychariot;


    public PlayableDirector Director { get => director; set => director = value; }

    void Start()
    {
        instance = this;
    }

    public void PlayDirector()
    {
        switch (GameData.ActiveBoss)
        {
            case GameData.BossType.TERRORCOPTER:
                director.Play(terracopter);
                break;
            case GameData.BossType.GATEKEEPER:
                director.Play(terracopter);
                break;
            case GameData.BossType.UNHOLYCHARIOT:
                director.Play(terracopter);
                break;
            case GameData.BossType.HEADHUNTER:
                director.Play(terracopter);
                break;
        }   
    }

    public void StopDirector()
    {
        director.Stop();
    }
}
