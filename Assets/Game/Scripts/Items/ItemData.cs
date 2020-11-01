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

    public void TakeEffect()
    {
        switch (effect)
        {
            case ItemEffect.IMMUNE:
                Immune_Effect();
                break;
            case ItemEffect.SHIELD:
                Shield_Effect();
                break;
            case ItemEffect.SLOW_MOTION:
                Slowmo_Effect();
                break;
            case ItemEffect.HEAL:
                Heal_Effect();
                break;
            case ItemEffect.SACRIFICE:
                Sacrifice_Effect();
                break;
        }
    }

    public bool CheckIsOneTimeUse()
    {
        return oneTimeUse;
    }

    private void Immune_Effect()
    {
        GameVariables.PLAYER_IMMUNE = true;
        DOVirtual.DelayedCall(timeEffect, () => { GameVariables.PLAYER_IMMUNE = false; });
    }

    private void Shield_Effect()
    {

    }

    private void Slowmo_Effect()
    {
        GameVariables.SLOW_MO = true;
        Time.timeScale = Mathf.Clamp(amountEffect, 0, 1);
        DOVirtual.DelayedCall(timeEffect, () => {
            Time.timeScale = 1;
            GameVariables.SLOW_MO = false;
        });
    }

    private void Heal_Effect()
    {
        CharaData.hp = CharaData.maxhp;
    }

    private void Sacrifice_Effect()
    {
        CharaData.hp-=cost;

        if (CharaData.hp < 0) CharaData.hp = 0;

        InGameUI.instance.uilive();
        GameVariables.SPEED_BUFF = amountEffect;
    }
}