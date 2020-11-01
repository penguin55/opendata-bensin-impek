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

    public string itemName;
    [SerializeField] private ItemEffect effect;
    [SerializeField] private float timeEffect;
    [SerializeField] private float amountEffect;
    [SerializeField] private float cost;
    [SerializeField] private bool oneTimeUse;

    private bool onDelay;

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
        GameVariables.PLAYER_IMMUNE = true;
        DOVirtual.DelayedCall(timeEffect, () => { 
            GameVariables.PLAYER_IMMUNE = false; 
        });
        return true;
    }

    private bool Shield_Effect()
    {
        return false;
    }

    private bool Slowmo_Effect()
    {
        if (!onDelay)
        {
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
        CharaData.hp = CharaData.maxhp;
        InGameUI.instance.uilive();

        return true;
    }

    private bool Sacrifice_Effect()
    {
        CharaData.hp-=cost;

        if (CharaData.hp < 0) CharaData.hp = 0;

        InGameUI.instance.uilive();
        GameVariables.SPEED_BUFF = amountEffect;
        return true;
    }
}