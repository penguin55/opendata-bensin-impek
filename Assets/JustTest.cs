using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

public class JustTest : MonoBehaviour
{
    public string move;

    public bool damage;

    private void Update()
    {
        //Chariot boss = BossBehaviour.Instance as Chariot;
        //boss.animationTweening.MoveTrain(move);

        if (damage)
        {
            damage = false;
            BossBehaviour.Instance.TakeDamage();
        }
    }

    private void Start()
    {
        TWAudioController.PlayBGMAdvanced("BGM_ADVANCED", "chariot_cutscene", AudioLoop.LoopType.LOOP_WITH_INTRO, 48.8f, 361f);

        TWAudioController.PlayBGM("ENGINE_TRAIN", "train_engine", TWAudioController.PlayType.DEFAULT);
    }
}
