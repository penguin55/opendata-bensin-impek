using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingListenerEvent : MonoBehaviour
{
    [SerializeField] protected BasicTrainingManager manager;
    protected bool activeEventListener;

    public virtual void ActivateEventListener(bool flag)
    {
        activeEventListener = flag;
    }

    protected virtual bool ValidateEventListener<E>(E eventListener)
    {
        return false;
    }

    protected virtual void CompleteEventListener<E>(ref E eventListener)
    {

    }
}
