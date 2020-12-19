using UnityEngine;

public class TLE_Move : TrainingListenerEvent
{
    private bool move_right, move_up, move_left, move_down;

    protected override bool ValidateEventListener<E>(E eventListener)
    {
        if (typeof(E) == typeof(bool))
        {
            return ((bool)(object)eventListener);
        } else return false;
    }

    protected override void CompleteEventListener<E>(ref E eventListener)
    {
        if (typeof(E) == typeof(bool))
        {
            eventListener = (E) System.Convert.ChangeType(true, typeof(E));
        }
    }

    private void Update()
    {
        if (activeEventListener)
        {
            if (Input.GetKey(InputManager.instance.moveUp))
            {
                CompleteEventListener(ref move_up);
            }

            if (Input.GetKey(InputManager.instance.moveLeft))
            {
                CompleteEventListener(ref move_left);
            }

            if (Input.GetKey(InputManager.instance.moveDown))
            {
                CompleteEventListener(ref move_down);
            }

            if (Input.GetKey(InputManager.instance.moveRight))
            {
                CompleteEventListener(ref move_right);
            }
        }
    }
}
