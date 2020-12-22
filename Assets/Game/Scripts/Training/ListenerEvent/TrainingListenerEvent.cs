using UnityEngine;

public class TrainingListenerEvent : MonoBehaviour
{
    [SerializeField] protected TrainingManager manager;
    protected bool activeEventListener;

    public virtual void ActivateEventListener(bool flag)
    {
        activeEventListener = flag;
    }

    public virtual void InitEventListener(string param = "", bool value = true)
    {

    }

    public virtual void RestartStateListener()
    {
        CharaData.hp = CharaData.maxhp;
        TrainingUI.instance.UpdateLive();
    }

    protected virtual bool ValidateEventListener(string param)
    {
        return false;
    }

    public virtual void CompleteEventListener(string param, bool value, bool forceComplete)
    {

    }
}
