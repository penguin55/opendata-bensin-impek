using TomWill;
using UnityEngine;

public class CharaControlTraining : CharaBehaviourTraining
{
    public static CharaControlTraining instance;

    // Start is called before the first frame update
    void Start()
    {
        GameTime.PlayerTimeScale = 1f;
        Init();
        anim.SetBool("dead", false);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameVariables.GAME_OVER) return;
        Clamp();
        Dash();
        KeyboardMovement();
        UpdateAnimationWalk(lastDirection.x, lastDirection.y, direction.sqrMagnitude);
        Action();
        ActivateItem();
    }
    public void KeyboardMovement()
    {
        direction = Vector2.zero;

        if (!GameVariables.FREEZE_INPUT)
        {
            /*note : 1 : up , 2 : down, 3 : left , 4 : right*/
            if (Input.GetKey(InputManager.instance.moveUp))
            {
                trainingManager.CompleteActiveTLE("move_up");
                isAccelerating = true;
                direction += Vector2.up;
                lastDirection = Vector2.up;
            }
            if (Input.GetKeyUp(InputManager.instance.moveUp))
            {
                if (isAccelerating && lastDirection == Vector2.up)
                {
                    isAccelerating = false;
                    timeMoveElapsed = timeToStop;
                }
            }

            if (Input.GetKey(InputManager.instance.moveDown))
            {
                trainingManager.CompleteActiveTLE("move_down");
                isAccelerating = true;
                direction += Vector2.down;
                lastDirection = Vector2.down;
            }
            if (Input.GetKeyUp(InputManager.instance.moveDown))
            {
                if (isAccelerating && lastDirection == Vector2.down)
                {
                    isAccelerating = false;
                    timeMoveElapsed = timeToStop;
                }
            }

            if (Input.GetKey(InputManager.instance.moveLeft))
            {
                trainingManager.CompleteActiveTLE("move_left");
                isAccelerating = true;
                direction += Vector2.left;
                lastDirection = Vector2.left; ;
            }
            if (Input.GetKeyUp(InputManager.instance.moveLeft))
            {
                if (isAccelerating && lastDirection == Vector2.left)
                {

                    isAccelerating = false;
                    timeMoveElapsed = timeToStop;
                }
            }

            if (Input.GetKey(InputManager.instance.moveRight))
            {
                trainingManager.CompleteActiveTLE("move_right");
                isAccelerating = true;
                direction += Vector2.right;
                lastDirection = Vector2.right;
            }
            if (Input.GetKeyUp(InputManager.instance.moveRight))
            {
                if (isAccelerating && lastDirection == Vector2.right)
                {
                    isAccelerating = false;
                    timeMoveElapsed = timeToStop;
                }
            }
        }

        MoveAccelerate();
    }

    public void Action()
    {
        if (Input.GetKeyDown(InputManager.instance.dash) && !GameVariables.FREEZE_INPUT)
        {
            if (canDash)
            {
                trainingManager.CompleteActiveTLE("dash");
                TWAudioController.PlaySFX("SFX_PLAYER", "dash");
                dashTime = startDashTime;
                isDashed = true;
                data.IsDashing = true;
                canDash = false;

                immune = true;
            }
        }
    }

    private void ActivateItem()
    {
        if (Input.GetKeyDown(InputManager.instance.activateItem) && !GameVariables.FREEZE_INPUT)
        {
            if (GameData.ActiveItem)
            {
                trainingManager.CompleteActiveTLE("item_used");
                UseItem();
            }
        }
    }
}
