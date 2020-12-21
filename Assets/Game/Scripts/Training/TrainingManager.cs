using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    protected TrainingListenerEvent activeTLE;

    public virtual void InteruptTrainingSection()
    {

    }

    public virtual void CompleteTrainingSection()
    {

    }

    public virtual void RestartActiveTrainingSection()
    {
        activeTLE?.RestartStateListener();
    }

    public void CompleteActiveTLE(string param, bool value = true, bool forceComplete = false)
    {
        activeTLE?.CompleteEventListener(param, value, forceComplete);
    }
}
