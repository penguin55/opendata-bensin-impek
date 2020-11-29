using DG.Tweening;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TomWill;
using UnityEngine;

public class GateKeeper : BossBehaviour
{
    enum State_Gatekeeper
    {
        PREPARATION,
        FINAL,
        DIE,
        IDLE,
        FLAMETHROWER_CLOCK,
        FLAMETHROWER_ANTICLOCK,
        CANNON,
        ROUND_FLAME,
    }

    [SerializeField] private GateKeeperRotateStates[] gateKeeperRotateStates;
    [SerializeField] private Transform centerTransformRotate;
    [SerializeField] private Sprite[] spriteStates;
    [SerializeField] private SpriteRenderer renderSpriteBody;
    private State_Gatekeeper currentState;
    private GateKeeperRotateStates activeRotateStates;
    [SerializeField] private State_Gatekeeper[] stateSequences;
    private int stateIndex;
    private float currentRotation;

    private void Start()
    {
        Sound("helicopter_blades");
        Instance = this;
        stateIndex = 0;
        currentState = State_Gatekeeper.PREPARATION;
        Sprite = GetComponent<SpriteRenderer>();
        DefaultMaterial = Sprite.material;

        UpdateState();
        InGameUI.instance.UpdateHpBos(health);
        RotateTank(270f);
    }


    private void UpdateState()
    {
        switch (currentState)
        {
            case State_Gatekeeper.PREPARATION:
                Preparation();
                break;
            case State_Gatekeeper.IDLE:
                Idle();
                break;
            case State_Gatekeeper.CANNON:
                Cannon();
                break;
            case State_Gatekeeper.ROUND_FLAME:
                RoundFlame();
                break;
            case State_Gatekeeper.FLAMETHROWER_CLOCK:
                FlameThower(true);
                break;
            case State_Gatekeeper.FLAMETHROWER_ANTICLOCK:
                FlameThower(false);
                break;
            case State_Gatekeeper.FINAL:
                Final();
                break;
            case State_Gatekeeper.DIE:
                Die();
                break;
        }
    }



    private void NextState()
    {
        if (!GameVariables.GAME_OVER)
        {
            stateIndex++;
            currentState = stateSequences[stateIndex];
            UpdateState();
        }
    }

    protected override void Preparation()
    {
        ((GateKeeper_FlameThower) patterns.First(e => e.attackName == "FlameThower").attackEvent).SetBossBehaviour(this);
        base.Preparation();
        NextState();
    }

    #region IDLE
    private void OnEnterIdle()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Cannon").attackEvent;
    }

    private void Idle()
    {
        OnEnterIdle();
        currentAttackEvent.ExecutePattern(OnExitIdle);
    }

    private void OnExitIdle()
    {
        NextState();
    }
    #endregion

    #region CANNON
    private void OnEnterCannon()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Cannon").attackEvent;
    }

    private void Cannon()
    {
        OnEnterCannon();

        currentAttackEvent.ExecutePattern(OnExitCannon);
    }

    private void OnExitCannon()
    {
        NextState();
    }
    #endregion

    #region ROUND_FLAME
    private void OnEnterRoundFlame()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "Round_Flame").attackEvent;
    }

    private void RoundFlame()
    {
        OnEnterRoundFlame();
        currentAttackEvent.ExecutePattern(OnExitRoundFlame);
    }

    private void OnExitRoundFlame()
    {
        NextState();
    }
    #endregion

    #region FLAMETHOWER
    private void OnEnterFlameThower()
    {
        currentAttackEvent = patterns.First(e => e.attackName == "FlameThower").attackEvent;
    }

    private void FlameThower(bool clockwise)
    {
        OnEnterFlameThower();
        if (!clockwise) ((GateKeeper_FlameThower)currentAttackEvent).Clockwise(false);
        if (clockwise) ((GateKeeper_FlameThower)currentAttackEvent).Clockwise(true);

        currentAttackEvent.ExecutePattern(OnExitFlameThower);
    }

    private void OnExitFlameThower()
    {
        NextState();
    }
    #endregion

    protected override void Final()
    {
        base.Final();

        stateIndex = 0;
        currentState = stateSequences[stateIndex];
        UpdateState();
    }

    public float GetCurrentRotation()
    {
        return currentRotation;
    }

    public void RotateTank(float angle)
    {
        currentRotation = angle >= 360 ? (angle - 360) : angle <= 0 ? (360 + angle) : angle;
        GateKeeperRotateStates newRotateStates = gateKeeperRotateStates[GetAngleSection((int)currentRotation)];

        if (activeRotateStates != newRotateStates)
        {
            if (activeRotateStates != null) activeRotateStates.collider.enabled = false;
            TWAudioController.PlaySFX("BOSS_SFX", "tank_move");
            activeRotateStates = newRotateStates;
            renderSpriteBody.sprite = activeRotateStates.sprite;
            activeRotateStates.collider.enabled = true;
        }
    }

    private void Sound(string name)
    {
        float audioLength = TWAudioController.AudioLength(name, "SFX");
        DOTween.Sequence()
            .AppendCallback(() => TWAudioController.PlaySFX("SFX_BOSS", name))
            .PrependInterval(audioLength)
            .SetLoops(-1)
            .SetId("tank_idle");
    }

    public Vector3 GetActiveSpawnPosition()
    {
        return activeRotateStates.spawnPosition.position;
    }

    public Vector3 GetCenterRotatePosition()
    {
        return centerTransformRotate.position;
    }

    public void TakingDamage(string name)
    {
        if (name.Contains(GetAdjacentSide(activeRotateStates.direction.ToLower())))
        {
            TakeDamage();
        }
    }

    private string GetAdjacentSide(string side)
    {
        if (side.Equals("right")) return "left";
        else if (side.Equals("left")) return "right";
        else if (side.Equals("upper")) return "bottom";
        else if (side.Equals("bottom")) return "upper";
        else return "unknown";
    }

    private int GetAngleSection(float angle)
    {
        angle = angle < 0 ? 180 + Mathf.Abs(angle) : angle;
        int offset = 360 / 8;
        float angleDiff = (angle + 22.5f) / offset;
        angleDiff = angleDiff >= 8 ? 0 : angleDiff;
        return (int) Mathf.Floor(angleDiff);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("laser_cannon"))
        {
            TakingDamage(collision.gameObject.name.ToLower());
        }
    }
}

[System.Serializable]
public class GateKeeperRotateStates
{
    public string direction;
    public Sprite sprite;
    public Transform spawnPosition;
    public Collider2D collider;
}