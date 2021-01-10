using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChariotTimeline : MonoBehaviour
{
    public ChariotTweening chariot;
    public void SetMoveName(string _move)
    {
        chariot.MoveTrain(_move);
    }
}
