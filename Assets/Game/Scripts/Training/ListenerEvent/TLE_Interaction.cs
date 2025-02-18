﻿using DG.Tweening;
using Fungus;
using UnityEngine;

public class TLE_Interaction : TrainingListenerEvent
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private SpriteRenderer gateRender;
    [SerializeField] private Collider2D gatePhysic;
    [SerializeField] private Sprite gateOpen, gateClose;

    [SerializeField] private SpriteRenderer buttonRender;
    [SerializeField] private Sprite buttonNormal, buttonPressed;

    private bool interact_button;
    private bool buttonActive;

    public override void ActivateEventListener(bool flag)
    {
        base.ActivateEventListener(flag);

        if (!flag) 
        {
            gateRender.gameObject.SetActive(false);
            buttonRender.gameObject.SetActive(false);
            gatePhysic.enabled = false;
        }
    }

    public override void InitEventListener(string param, bool value)
    {
        base.InitEventListener(param, value);
        InitPreparation();
    }

    public void ButtonPressed()
    {
        if (!buttonActive)
        {
            gateRender.sprite = gateOpen;
            buttonRender.sprite = buttonPressed;

            gatePhysic.enabled = false;

            CompleteEventListener("interact_button");
        }
    }

    protected override bool ValidateEventListener(string param)
    {
        return false;
    }

    public override void CompleteEventListener(string param, bool value = true, bool forceComplete = false)
    {
        if (activeEventListener || forceComplete)
        {
            if (param.ToLower().Equals("interact_button"))
            {
                interact_button = value;

                buttonActive = value;
                activeEventListener = !buttonActive;
                DOVirtual.DelayedCall(2f, ()=> manager.CompleteTrainingSection());
            }
        }
    }

    public override void RestartStateListener()
    {
        base.RestartStateListener();
        interact_button = false;
        buttonActive = false;
        gateRender.gameObject.SetActive(false);
        buttonRender.gameObject.SetActive(false);

        InitPreparation();
    }

    private void InitPreparation()
    {
        CharaControlTraining.instance.transform.position = playerPosition.position;

        gateRender.sprite = gateClose;
        buttonRender.sprite = buttonNormal;

        gateRender.gameObject.SetActive(true);
        buttonRender.gameObject.SetActive(true);

        gatePhysic.enabled = true;
    }
}
