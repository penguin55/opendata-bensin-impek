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
        SACRIFICE
    }

    public Sprite image;
    public string itemName;
    [TextArea(2,20)] public string itemDesc;
    public int itemId;
    public bool wasUsed;
    [SerializeField] private ItemEffect effect;
    [SerializeField] private float timeEffect;
    [SerializeField] private float amountEffect;
    [SerializeField] private float cost;
    [SerializeField] private bool oneTimeUse;
    [SerializeField] private bool activateOnStart;

    private bool onDelay;

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
            default:
                return false;
        }
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
        InGameUI.instance.UpdateShield();
        return true;
    }

    private bool Slowmo_Effect()
    {
        if (!onDelay)
        {
            wasUsed = true;
            onDelay = true;
            GameVariables.SLOW_MO = true;
            Time.timeScale = Mathf.Clamp(amountEffect, 0, 1);
            DOVirtual.DelayedCall(timeEffect, () =>
            {
                Time.timeScale = 1;
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
        InGameUI.instance.UpdateLive();

        return true;
    }

    private bool Sacrifice_Effect()
    {
        wasUsed = true;
        CharaData.hp-=cost;

        if (CharaData.hp < 0) CharaData.hp = 0;

        InGameUI.instance.UpdateLive();
        GameVariables.SPEED_BUFF = amountEffect;
        return true;
    }
}