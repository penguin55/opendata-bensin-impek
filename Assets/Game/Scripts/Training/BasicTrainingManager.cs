using TomWill;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicTrainingManager : TrainingManager
{
    [SerializeField] private BasicTrainingData[] trainingDatas;
    [SerializeField] private SpriteRenderer display_button_desc, display_body, display_button;
    [SerializeField] private FungusController fungusController;
    private BasicTrainingData activeTrainingData;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        GameData.ActiveItem = null;
        activeTrainingData = null;
        //currentIndex = 0;
        //activeTrainingData = trainingDatas[currentIndex];
        //activeTLE = activeTrainingData.eventTraining;
        GameVariables.FREEZE_INPUT = true;
        LaunchStartDialog();
    }

    public override void CompleteTrainingSection()
    {
        base.CompleteTrainingSection();
        TWTransition.ScreenTransition(TWTransition.TransitionType.UP_IN, 1f, () => 
        {
            GameVariables.FREEZE_INPUT = true;
            LaunchFinishDialog();
            TWTransition.ScreenTransition(TWTransition.TransitionType.UP_OUT, 1f);
        });
    }

    public override void InteruptTrainingSection()
    {
        base.InteruptTrainingSection();
    }

    public override void RestartActiveTrainingSection()
    {
        base.RestartActiveTrainingSection();
        TWTransition.ScreenTransition(TWTransition.TransitionType.UP_IN, 1f, () =>
        {
            GameVariables.FREEZE_INPUT = true;
            LaunchStartDialog();
            TWTransition.ScreenTransition(TWTransition.TransitionType.UP_OUT, 1f);
        });
    }

    public void BacktoPickItem()
    {
        GameVariables.DIALOG_START_MESSAGE = "ITEM_PANEL";
        TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_IN, 1, ()=> SceneManager.LoadScene("dialogFungus"));
    }

    public void LaunchStartDialog()
    {
        if (activeTrainingData == null) fungusController.NextBlock("Health");
        else
        {
            switch (activeTrainingData.training_name)
            {
                case "move":
                    GameVariables.FREEZE_INPUT = true;
                    fungusController.NextBlock("S_Move");
                    break;
                case "dash":
                    GameVariables.FREEZE_INPUT = true;
                    fungusController.NextBlock("S_Dash");
                    break;
                case "interact":
                    GameVariables.FREEZE_INPUT = true;
                    fungusController.NextBlock("S_Interact");
                    break;
            }
        }
    }

    public void LaunchFinishDialog()
    {
        switch (activeTrainingData.training_name)
        {
            case "move":
                GameVariables.FREEZE_INPUT = true;
                fungusController.NextBlock("F_Move");
                break;
            case "dash":
                GameVariables.FREEZE_INPUT = true;
                fungusController.NextBlock("F_Dash");
                break;
            case "interact":
                GameVariables.FREEZE_INPUT = true;
                fungusController.NextBlock("F_Interact");
                break;
        }
    }

    public void NextTraining()
    {

        if (activeTrainingData == null)
        {
            currentIndex = 0;
            activeTrainingData = trainingDatas[currentIndex];
            activeTLE = activeTrainingData.eventTraining;
            SetDisplay();
            LaunchStartDialog();
        }
        else
        {
            activeTrainingData.eventTraining.ActivateEventListener(false);

            currentIndex++;
            if (currentIndex >= trainingDatas.Length)
            {
                fungusController.NextBlock("Finish");
            }
            else
            {
                Debug.Log(activeTrainingData.training_name);
                activeTrainingData = trainingDatas[currentIndex];
                activeTLE = activeTrainingData.eventTraining;
                SetDisplay();
                LaunchStartDialog();
            }
        }

        //activeTrainingData.eventTraining.ActivateEventListener(false);

        //currentIndex++;
        //if (currentIndex >= trainingDatas.Length)
        //{

        //}
        //else
        //{
        //    activeTrainingData = trainingDatas[currentIndex];
        //    activeTLE = activeTrainingData.eventTraining;
        //    SetDisplay();
        //    LaunchTraining();
        //}
    }

    public void GetLaunchTraining()
    {
        LaunchTraining();
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