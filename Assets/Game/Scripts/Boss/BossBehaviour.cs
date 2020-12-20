using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;
using UnityEngine.Playables;

public class BossBehaviour : MonoBehaviour
{
    public static BossBehaviour Instance;

    [SerializeField] private ItemData dropItem;
    [SerializeField] private ParticleSystem explosion;

    [SerializeField] public int health;

    private SpriteRenderer sprite;
    private Material defaultMaterial;
    [SerializeField] private Material whiteFlash;
    [SerializeField] private float flashDelay;
    [SerializeField] private PlayableDirector director;

    [SerializeField] protected AttackPattern[] patterns;
    [SerializeField]private FungusController fungus;
    

    protected AttackEvent currentAttackEvent;

    public Material DefaultMaterial { get => defaultMaterial; set => defaultMaterial = value; }
    public Material WhiteFlash { get => whiteFlash; set => whiteFlash = value; }
    public float FlashDelay { get => flashDelay; set => flashDelay = value; }
    public SpriteRenderer Sprite { get => sprite; set => sprite = value; }

    protected virtual void Init()
    {
        defaultMaterial = sprite.material;
    }

    protected virtual void Preparation()
    {

    }

    protected virtual void Final()
    {

    }

    protected virtual void Die()
    {
        GameVariables.GAME_OVER = true;
        TWAudioController.PlaySFX("SFX_BOSS", "helicopter_destroyed");
        TWAudioController.PlaySFX("SFX_BOSS", "helicopter_destroyed_2");
        TWTransition.ScreenFlash(1, 0.1f);
        switch (GameData.ActiveBoss)
        {
            case GameData.BossType.TERRORCOPTER:
                DOTween.Sequence()
                    .AppendCallback(() => { explosion.Play(); })
                    .AppendCallback(() => { CameraShake.instance.Shake(explosion.main.duration, 3, 10); })
                    .AppendInterval(explosion.main.duration / 2)
                    .AppendCallback(() => { gameObject.SetActive(false); })
                    .AppendInterval(explosion.main.duration)
                    .AppendCallback(() => fungus.NextBlock("Dialog_1"));
                break;
            case GameData.BossType.GATEKEEPER:
                DOTween.Sequence()
                    .AppendCallback(() => { explosion.Play(); })
                    .AppendCallback(() => { CameraShake.instance.Shake(explosion.main.duration, 3, 10); })
                    .AppendInterval(explosion.main.duration)
                    .AppendCallback(() => director.Play());
                break;
            case GameData.BossType.UNHOLYCHARIOT:
                break;
            case GameData.BossType.HEADHUNTER:
                break;
        }
    }

    public void TakeDamage()
    {
        TWAudioController.PlaySFX("SFX_BOSS", "helicopter_damage");
        if (health >= 1)
        {
            DOTween.Sequence()
           .AppendCallback(() => { Sprite.material = WhiteFlash; })
           .AppendInterval(FlashDelay)
           .AppendCallback(() => { Sprite.material = DefaultMaterial; }
           );
            health -= 1;
            InGameUI.instance.UpdateHpBos(health);

            if (health < 1)
            {
                DOTween.Kill("BM_Missile");
                Die();
            }
        }
    }

    public ItemData GetDrop()
    {
        if (dropItem) GameData.ItemHolds.Add(dropItem);

        return dropItem;
    }

    public void DropItems()
    {
        Debug.Log("MASOK PAK EKO! " + dropItem.itemName );
        if (dropItem) GameData.ItemHolds.Add(dropItem);
    }
}


[System.Serializable] 
public class AttackPattern
{
    public string attackName;

    public AttackEvent attackEvent; 
}


