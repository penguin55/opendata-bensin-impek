using Fungus;
using TomWill;
using UnityEngine;

public class CharaController : CharaBehaviour
{
    public static CharaController instance;
    //[Header ("Input Controller")]
    //[SerializeField] private KeyCode moveUp;
    //[SerializeField] private KeyCode moveDown;
    //[SerializeField] private KeyCode moveRight;
    //[SerializeField] private KeyCode moveLeft;
    //[SerializeField] private KeyCode dash;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

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

        if (!GameVariables.FREEZE_INPUT && !isCharged)
        {
            /*note : 1 : up , 2 : down, 3 : left , 4 : right*/
            if (Input.GetKey(InputManager.instance.moveUp))
            {
                //TWAudioController.PlaySFX("SFX_PLAYER", "player_move");
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
                //TWAudioController.PlaySFX("SFX_PLAYER", "player_move");
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
                //TWAudioController.PlaySFX("SFX_PLAYER", "player_move");
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
                //TWAudioController.PlaySFX("SFX_PLAYER", "player_move");
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
        if (data.ChargeDash)
        {
            if (data.CanChargeDash)
            {
                if (Input.GetKey(InputManager.instance.dash) && !GameVariables.FREEZE_INPUT && data.ChargeTimeDash <= data.MaxChargeTimeDash)
                {
                    isCharged = true;
                    data.ChargeTimeDash += Time.deltaTime;
                    //anim.SetBool("dash", true);
                    if (walkDustParticle.isPlaying) walkDustParticle.Stop();
                    dashDustParticle.Play();
                }
                else if (Input.GetKeyDown(InputManager.instance.dash) && !GameVariables.FREEZE_INPUT)
                {
                    if (canDash)
                    {
                        Debug.Log("MASUK SINI");
                        TWAudioController.PlaySFX("SFX_PLAYER", "dash");
                        dashTime = startDashTime;
                        isDashed = true;
                        data.IsDashing = true;
                        canDash = false;

                        GameVariables.PLAYER_IMMUNE = true;
                    }
                }

                if (Input.GetKeyUp(InputManager.instance.dash) && !GameVariables.FREEZE_INPUT && data.ChargeTimeDash > 0)
                {
                    data.DashSpeed = data.BaseDashSpeed * (1f + ((Mathf.Clamp(data.ChargeTimeDash, 0f, data.MaxChargeTimeDash) / data.MaxChargeTimeDash) * (data.MultiplierChargeDash - 1f)));

                    TWAudioController.PlaySFX("SFX_PLAYER", "dash");
                    dashTime = startDashTime;
                    isDashed = true;
                    data.IsDashing = true;
                    canDash = false;
                    data.CanChargeDash = false;

                    GameVariables.PLAYER_IMMUNE = true;
                }

                if (data.ChargeTimeDash >= data.MaxChargeTimeDash && !GameVariables.FREEZE_INPUT)
                {
                    data.ChargeTimeDash = data.MaxChargeTimeDash;
                    data.DashSpeed = data.BaseDashSpeed * data.MultiplierChargeDash;

                    TWAudioController.PlaySFX("SFX_PLAYER", "dash");
                    dashTime = startDashTime;
                    isDashed = true;
                    data.IsDashing = true;
                    canDash = false;
                    data.CanChargeDash = false;

                    GameVariables.PLAYER_IMMUNE = true;
                }
                               
            }
        }
        else
        {
            if (Input.GetKeyDown(InputManager.instance.dash) && !GameVariables.FREEZE_INPUT)
            {
                if (canDash)
                {
                    TWAudioController.PlaySFX("SFX_PLAYER", "dash");
                    dashTime = startDashTime;
                    isDashed = true;
                    data.IsDashing = true;
                    canDash = false;

                    GameVariables.PLAYER_IMMUNE = true;
                }
            }
        }
    }

    private void ActivateItem()
    {
        if (Input.GetKeyDown(InputManager.instance.activateItem) && !GameVariables.FREEZE_INPUT)
        {
            UseItem();
        }
    }

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }
}
