using UnityEngine;

public class TLE_Move : TrainingListenerEvent
{
    [SerializeField] private Transform playerPosition;
    private bool move_right, move_up, move_left, move_down;

    public override void ActivateEventListener(bool flag)
    {
        base.ActivateEventListener(flag);
        CharaControlTraining.instance.transform.position = playerPosition.position;
    }

    protected override bool ValidateEventListener(string param)
    {
        switch (param.ToLower())
        {
            case "move_right":
                return move_right;
            case "move_down":
                return move_down;
            case "move_left":
                return move_left;
            case "move_up":
                return move_up;
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
                case "move_right":
                    move_right = value;
                    break;
                case "move_down":
                    move_down = value;
                    break;
                case "move_left":
                    move_left = value;
                    break;
                case "move_up":
                    move_up = value;
                    break;
            }

            activeEventListener = !AllEventClear();
            if (AllEventClear()) manager.CompleteTrainingSection();
        }
    }

    public override void RestartStateListener()
    {
        base.RestartStateListener();
        move_right = false;
        move_left = false;
        move_up = false;
        move_down = false;
    }

    private bool AllEventClear()
    {
        return move_right && move_up && move_left && move_down;
    }
}
