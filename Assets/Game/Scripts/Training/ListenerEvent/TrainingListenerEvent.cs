using UnityEngine;

public class TrainingListenerEvent : MonoBehaviour
{
    [SerializeField] protected TrainingManager manager;
    protected bool activeEventListener;

    public virtual void ActivateEventListener(bool flag)
    {
        activeEventListener = flag;
    }

    public virtual void InitEventListener(string param, bool value)
    {

    }

    protected virtual bool ValidateEventListener(string param)
    {
        return false;
    }

    public virtual void CompleteEventListener(string param, bool value = true)
    {

    }
}
