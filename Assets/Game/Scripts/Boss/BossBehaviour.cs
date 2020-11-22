using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

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

    [SerializeField] protected AttackPattern[] patterns;

    protected AttackEvent currentAttackEvent;

    public Material DefaultMaterial { get => defaultMaterial; set => defaultMaterial = value; }
    public Material WhiteFlash { get => whiteFlash; set => whiteFlash = value; }
    public float FlashDelay { get => flashDelay; set => flashDelay = value; }
    public SpriteRenderer Sprite { get => sprite; set => sprite = value; }

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

        DOTween.Sequence()
            .AppendCallback(() => { explosion.Play(); })
            .AppendCallback(() => { CameraShake.instance.Shake(explosion.main.duration, 3, 10); })
            .AppendInterval(explosion.main.duration / 2)
            .AppendCallback(() => { gameObject.SetActive(false); })
            .AppendInterval(explosion.main.duration / 2)
            .AppendCallback(() => Time.timeScale = 0f)
            .AppendCallback(() => InGameUI.instance.GameWin());
    }

    public void TakeDamage()
    {
        if (health >= 1)
        {
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
}


[System.Serializable] 
public class AttackPattern
{
    public string attackName;

    public AttackEvent attackEvent; 
}


