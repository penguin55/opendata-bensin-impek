using TomWill;
using UnityEngine;

public class BasicTrainingManager : MonoBehaviour
{
    [SerializeField] private BasicTrainingData[] trainingDatas;
    [SerializeField] private SpriteRenderer display_button_desc, display_body, display_button;
    private BasicTrainingData activeTrainingData;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        activeTrainingData = trainingDatas[currentIndex];
        GameVariables.FREEZE_INPUT = true;

        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_OUT, 1f, () =>
        {
            SetDisplay();
            LaunchTraining();
        });
    }

    private void NextTraining()
    {
        activeTrainingData.eventTraining.ActivateEventListener(false);

        currentIndex++;
        if (currentIndex >= trainingDatas.Length)
        {

        } else
        {
            activeTrainingData = trainingDatas[currentIndex];
            SetDisplay();

            LaunchTraining();
        }
    }

    public void CompleteTrainingSection()
    {
        TWTransition.ScreenTransition(TWTransition.TransitionType.UP_IN, 1f, () => 
        {
            GameVariables.FREEZE_INPUT = true;
            NextTraining();
            TWTransition.ScreenTransition(TWTransition.TransitionType.UP_OUT, 1f);
        });
    }

    private void LaunchTraining()
    {
        GameVariables.FREEZE_INPUT = false;
        activeTrainingData.eventTraining.ActivateEventListener(true);
    }

    private void SetDisplay()
    {
        display_button_desc.sprite = activeTrainingData.training_button_desc;
        display_button.sprite = activeTrainingData.training_button;
    }
}

[System.Serializable]
public class BasicTrainingData
{
    public string training_name;
    public Sprite training_button;
    public Sprite training_button_desc;
    public TrainingListenerEvent eventTraining;
}