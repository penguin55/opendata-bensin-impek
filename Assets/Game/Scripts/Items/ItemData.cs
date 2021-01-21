using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Item", fileName = "New Item")]
public class ItemData : ScriptableObject
{
    public enum ItemEffect
    {
        IMMUNE,
        SHIELD,
        SLOW_MOTION,
        HEAL,
        SACRIFICE,
        LOWERDASHDELAY,
        CHARGEDASH,
        INVISIBLE,
        GETBACK
    }

    public Sprite image;
    public string itemName;
    [TextArea(2,20)] public string itemDesc;
    [TextArea(2, 20)] public string shortDesc;
    public int itemId;
    public bool wasUsed;
    [SerializeField] private ItemEffect effect;
    [SerializeField] private float timeEffect;
    [SerializeField] private float amountEffect;
    [SerializeField] private float timeCooldown;
    [SerializeField] private float cost;
    [SerializeField] private bool oneTimeUse;
    [SerializeField] private bool activateOnStart;

    public bool onDelay;

    public bool ActivateOnStart { get => activateOnStart;}

    public bool TakeEffect()
    {
        switch (effect)
        {
            case ItemEffect.IMMUNE:
                return Immune_Effect();
            case ItemEffect.SHIELD:
                return Shield_Effect();
            case ItemEffect.SLOW_MOTION:
                return Slowmo_Effect();
            case ItemEffect.HEAL:
                return Heal_Effect();
            case ItemEffect.SACRIFICE:
                return Sacrifice_Effect();
            case ItemEffect.LOWERDASHDELAY:
                return LowerDashDelay();
            case ItemEffect.CHARGEDASH:
                return ChargeDashEffect();
            case ItemEffect.INVISIBLE:
                return InvisibleEffect();
            case ItemEffect.GETBACK:
                return GetBackEffect();
            default:
                return false;
        }
    }

    public bool GetBackEffect()
    {
        CharaController.instance.MarkDown(true);
        DOVirtual.DelayedCall(timeEffect, () =>
        {
            CharaController.instance.MarkDown(false);
        }).OnComplete(() =>
        {
            DOVirtual.Float(0, timeCooldown, timeCooldown, (time) =>
            {
                InGameUI.instance.ActivatedCooldownTimer(true);
                InGameUI.instance.UpdateCooldownTimer(timeCooldown, (timeCooldown - time));
            }).OnComplete(() =>
            {
                InGameUI.instance.ActivatedCooldownTimer(false);
                wasUsed = false;
            });
        });
        return true;
    }

    public bool InvisibleEffect()
    {
        CharaController.instance.InvisibleFrame(amountEffect);
        wasUsed = true;
        GameVariables.PLAYER_IMMUNE = true;
        DOVirtual.DelayedCall(timeEffect, () => {
            GameVariables.PLAYER_IMMUNE = false;
            CharaController.instance.InvisibleFrame(1);
        });
        return true;
    }

    public bool LowerDashDelay()
    {
        wasUsed = true;
        CharaController.instance.SetDashDelay(CharaController.instance.GetDashDelay() / amountEffect);
        return true;
    }

    public bool ChargeDashEffect()
    {
        CharaData.canChargeDash = true;
        CharaData.maxChargeTimeDash = amountEffect;
        CharaData.chargeDash = true;
        CharaData.chargeTimeDash = 0f;
        return true;
    }

    public bool CheckIsOneTimeUse()
    {
        return oneTimeUse;
    }

    private bool Immune_Effect()
    {
        wasUsed = true;
        GameVariables.PLAYER_IMMUNE = true;
        DOVirtual.DelayedCall(timeEffect, () => { 
            GameVariables.PLAYER_IMMUNE = false; 
        });
        return true;
    }

    private bool Shield_Effect()
    {
        wasUsed = true;
        CharaData.shield = 1;
        InGameUI.instance?.UpdateShield();
        TrainingUI.instance?.UpdateShield();
        return true;
    }

    private bool Slowmo_Effect()
    {
        if (!onDelay)
        {
            wasUsed = true;
            onDelay = true;
            GameVariables.SLOW_MO = true;
            GameTime.LocalTimeScale = Mathf.Clamp(amountEffect, 0, 1);
            DOVirtual.DelayedCall(timeEffect, () =>
            {
                GameTime.LocalTimeScale = 1;
                GameVariables.SLOW_MO = false;
                onDelay = false;
            });

            return true;
        }

        return false;
    }

    private bool Heal_Effect()
    {
        wasUsed = true;
        CharaData.hp = CharaData.maxhp;
        InGameUI.instance?.UpdateLive();
        TrainingUI.instance?.UpdateLive();
        return true;
    }

    private bool Sacrifice_Effect()
    {
        wasUsed = true;
        CharaData.hp-=cost;

        if (CharaData.hp < 0) CharaData.hp = 0;

        InGameUI.instance?.UpdateLive();
        TrainingUI.instance?.UpdateLive();
        GameVariables.SPEED_BUFF = amountEffect;
        return true;
    }
}