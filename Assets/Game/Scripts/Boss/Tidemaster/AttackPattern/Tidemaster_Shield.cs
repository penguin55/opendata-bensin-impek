using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class Tidemaster_Shield : AttackEvent
{
    [SerializeField] private float shieldTimer;
    [SerializeField] private UIMiniGameTidemaster uiMiniGame;

    [SerializeField] private Transform startLaunchPos;
    [SerializeField] private GameObject projectileTorpedo;
    [SerializeField] private float durationExplodeTorpedo;
    [SerializeField] private float strengthExplodeTorpedo;
    [SerializeField] private int vibratoExplodeTorpedo;

    private bool miniGameActive;
    // Button status
    private bool rightButtonIsActive;
    private bool bottomButtonIsActive;
    private bool leftButtonIsActive;
    private bool topButtonIsActive;

    private bool playerShipImmune;

    protected override void OnPrepare_Attack()
    {
        base.OnPrepare_Attack();
    }

    protected override void OnEnter_Attack()
    {
        TWAudioController.PlaySFX("BOSS_SFX", "shield_popup");
        miniGameActive = true;
        GameVariables.FREEZE_INPUT = true;

        ActivateRandomButtonMiniGame();

        DOVirtual.Float(0, shieldTimer, shieldTimer, (time)=>
        {
            OnInputMiniGame();
            uiMiniGame.UpdateTimerInfo(shieldTimer, (shieldTimer - time));
        }).SetEase(Ease.Linear).OnComplete(()=>
        {
            uiMiniGame.ActivateMiniGame(false);
            miniGameActive = false;
            GameVariables.FREEZE_INPUT = false;

            playerShipImmune = CheckAllButtonActive();

            base.OnEnter_Attack();
        });

        /*DOVirtual.DelayedCall(shieldTimer, ()=> {
            uiMiniGame.ActivateMiniGame(false);
            miniGameActive = false;
            GameVariables.FREEZE_INPUT = false;

            playerShipImmune = CheckAllButtonActive();

            base.OnEnter_Attack();
        }).OnUpdate(()=>
        {
            OnInputMiniGame();
        });*/
    }

    protected override void Attack()
    {
        if (playerShipImmune)
        {
            base.Attack();
        } else
        {
            Launch(() => base.Attack());
        }
    }

    protected override void OnExit_Attack()
    {
        base.OnExit_Attack();
    }

    private void OnInputMiniGame()
    {
        if (Input.GetKeyDown(InputManager.instance.moveRight))
        {
            Validate(rightButtonIsActive);
            rightButtonIsActive = true;
            uiMiniGame.ActivateRightButton(true);
        }

        if (Input.GetKeyDown(InputManager.instance.moveDown))
        {
            Validate(bottomButtonIsActive);
            bottomButtonIsActive = true;
            uiMiniGame.ActivateBottomButton(true);
        }

        if (Input.GetKeyDown(InputManager.instance.moveLeft))
        {
            Validate(leftButtonIsActive);
            leftButtonIsActive = true;
            uiMiniGame.ActivateLeftButton(true);
        }

        if (Input.GetKeyDown(InputManager.instance.moveUp))
        {
            Validate(topButtonIsActive);
            topButtonIsActive = true;
            uiMiniGame.ActivateTopButton(true);
        }
    }

    private void ActivateRandomButtonMiniGame()
    {
        bool deactiveFlag = false;

        bool rightActive = Random.Range(1, 101) % 2 == 0;
        bool bottomActive = Random.Range(1, 101) % 2 == 0;
        bool leftActive = Random.Range(1, 101) % 2 == 0;
        bool topActive = Random.Range(1, 101) % 2 == 0;

        deactiveFlag = !(rightActive && bottomActive && leftActive && topActive);

        if (!deactiveFlag)
        {
            float offset = Random.Range(0, 4);

            switch (offset)
            {
                case 0:
                    rightActive = false;
                    break;
                case 1:
                    bottomActive = false;
                    break;
                case 2:
                    leftActive = false;
                    break;
                case 3:
                    topActive = false;
                    break;
            }
        }

        rightButtonIsActive = rightActive;
        bottomButtonIsActive = bottomActive;
        leftButtonIsActive = leftActive;
        topButtonIsActive = topActive;

        uiMiniGame.ActivateMiniGame(true, rightActive, bottomActive, leftActive, topActive);
    }

    private bool CheckAllButtonActive()
    {
        return rightButtonIsActive && bottomButtonIsActive && leftButtonIsActive && topButtonIsActive;
    }

    private void Launch(UnityAction action)
    {
        TWAudioController.PlaySFX("BOSS_SFX", "torpedo_launch");
        projectileTorpedo.transform.position = startLaunchPos.position;
        projectileTorpedo.SetActive(true);

        projectileTorpedo.transform.DOLocalMoveY(-4f, 2f).SetEase(Ease.InOutQuart)
            .OnComplete(()=>
            {
                CameraShake.instance.Shake(durationExplodeTorpedo, strengthExplodeTorpedo, vibratoExplodeTorpedo);
                TWAudioController.PlaySFX("BOSS_SFX", "torpedo_impact");
                CharaController.instance.TakeDamage();
                projectileTorpedo.transform.position = startLaunchPos.position;
                projectileTorpedo.SetActive(false);
                action.Invoke();
            });
    }

    private void Validate(bool flag)
    {
        if (flag)
        {
            TWAudioController.PlaySFX("BOSS_SFX", "shield_wrong");
            CameraShake.instance.Shake(0.2f, 1, 50);
        }
        else
        {
            TWAudioController.PlaySFX("BOSS_SFX", "shield_right");
        }
    }
}
