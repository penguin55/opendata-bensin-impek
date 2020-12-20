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

    public void CompleteActiveTLE(string param, bool value = true)
    {
        activeTLE?.CompleteEventListener(param, value);
    }
}
