using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLE_Item : TrainingListenerEvent
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private TLEO_Missile missile;
    private bool item_used;
    private bool missileActive;

    public override void ActivateEventListener(bool flag)
    {
        base.ActivateEventListener(flag);

        if (flag)
        {
            if (missileActive) missile.ExecutePattern(OnCompleteSection);
        }
    }

    public override void InitEventListener(string param, bool value)
    {
        base.InitEventListener(param, value);
        switch (param.ToLower())
        {
            case "missile":
                missileActive = true;
                break;
            default:
                break;
        }
    }

    protected override bool ValidateEventListener(string param)
    {
        switch (param.ToLower())
        {
            case "item_used":
                return item_used;
            default:
                return false;
        }
    }

    public override void CompleteEventListener(string param, bool value = true, bool forceComplete = false)
    {
        if (activeEventListener || forceComplete)
        {
            switch (param.ToLower())
            {
                case "item_used":
                    item_used = value;
                    break;
            }
        }
    }

    public override void RestartStateListener()
    {
        base.RestartStateListener();
        item_used = false;
    }

    private void OnCompleteSection()
    {
        if (item_used)
        {
            manager.CompleteTrainingSection();
        } else
        {
            manager.InteruptTrainingSection();
        }
    }
}
