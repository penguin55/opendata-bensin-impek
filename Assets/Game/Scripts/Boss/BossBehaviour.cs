﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;
using UnityEngine.Playables;

public class BossBehaviour : MonoBehaviour
{
    public static BossBehaviour Instance;

    [SerializeField] private ItemData dropItem;
    [SerializeField] protected ParticleSystem explosion;

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

    public bool isDead;

    protected virtual void Init()
    {
        defaultMaterial = sprite.material;
        isDead = false;
    }

    protected virtual void Preparation()
    {
        GameTrackRate.StartTimePlay = Time.time;
    }

    protected virtual void Final()
    {

    }

    protected virtual void Die()
    {
        GameTrackRate.EndTimePlay = Time.time;
        GameTrackRate.CalculateTime();
        GameTrackRate.BossKill();
        InGameUI.instance?.FreezeInput(true);

        GameVariables.GAME_OVER = true;
        TWAudioController.PlaySFX("SFX_BOSS", "helicopter_destroyed");
        TWAudioController.PlaySFX("SFX_BOSS", "helicopter_destroyed_2");
        isDead = true;

        GameData.ActiveBossData.wasDie = true;
        if (!GameData.ActiveItem.CheckIsOneTimeUse())
        {
            GameData.ShiftItemList();
        }
        switch (GameData.ActiveBoss)
        {
            case GameData.BossType.TERRORCOPTER:
                DOTween.Sequence()
                    .AppendCallback(() => { explosion.Play(); })
                    .AppendCallback(() => { CameraShake.instance.Shake(explosion.main.duration, 3, 10); })
                    .AppendInterval(explosion.main.duration / 2)
                    .AppendCallback(() => TWTransition.ScreenFlash(1, 0.2f))
                    .AppendCallback(() => { gameObject.SetActive(false); })
                    .AppendInterval(explosion.main.duration)
                    .AppendCallback(() => fungus.NextBlock("Dialog_1"));
                break;
            case GameData.BossType.GATEKEEPER:
                DOTween.Sequence()
                    .AppendCallback(() => TWTransition.ScreenFlash(1, 0.1f))
                    .AppendCallback(() => { explosion.Play(); })
                    .AppendCallback(() => { CameraShake.instance.Shake(explosion.main.duration, 3, 10); })
                    .AppendInterval(explosion.main.duration)
                    .AppendCallback(() => director.Play());
                break;
            case GameData.BossType.UNHOLYCHARIOT:
                DOTween.Sequence()
                    .AppendCallback(() => director.Play())
                    .AppendCallback(() => explosion.Play());
                break;
            case GameData.BossType.TIDEMASTER:
                DOTween.Sequence()
                    .AppendCallback(() => director.Play())
                    .AppendCallback(() => explosion.Play());
                break;
        }
    }

    public void ChariotCameraShake()
    {
        TWAudioController.PlaySFX("BOSS_SFX", "rocket_impact");
        TWTransition.ScreenFlash(1, 0.5f);
        CameraShake.instance.Shake(1, 3, 10);
    }

    public virtual void TakeDamage()
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


