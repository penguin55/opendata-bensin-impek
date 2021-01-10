using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustTest : MonoBehaviour
{
    public string move;

    private void Update()
    {
        Chariot boss = BossBehaviour.Instance as Chariot;
        boss.animationTweening.MoveTrain(move);
    }
}
