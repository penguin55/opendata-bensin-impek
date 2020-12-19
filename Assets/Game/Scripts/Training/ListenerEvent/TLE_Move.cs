using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLE_Move : TrainingListenerEvent
{
    private bool move_right, move_up, move_left, move_down;

    protected override bool ValidateEventListener<E>(E eventListener)
    {
        return false;
    }
}
