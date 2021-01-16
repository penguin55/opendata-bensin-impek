using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tidemaster_Shield : AttackEvent
{
    [SerializeField] private float shieldTimer;
    [SerializeField] private UIMiniGameTidemaster uiMiniGame;

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
        Debug.Log(playerShipImmune ? "Immune" : "Damage Taken");
        base.Attack();
    }

    protected override void OnExit_Attack()
    {
        base.OnExit_Attack();
    }

    private void OnInputMiniGame()
    {
        if (Input.GetKey(InputManager.instance.moveRight))
        {
            rightButtonIsActive = true;
            uiMiniGame.ActivateRightButton(true);
        }

        if (Input.GetKey(InputManager.instance.moveDown))
        {
            bottomButtonIsActive = true;
            uiMiniGame.ActivateBottomButton(true);
        }

        if (Input.GetKey(InputManager.instance.moveLeft))
        {
            leftButtonIsActive = true;
            uiMiniGame.ActivateLeftButton(true);
        }

        if (Input.GetKey(InputManager.instance.moveUp))
        {
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
}
