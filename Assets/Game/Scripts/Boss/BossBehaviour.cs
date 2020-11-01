using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public static BossBehaviour Instance;

    [SerializeField] private ParticleSystem explosion;

    [SerializeField] public int health;
    [SerializeField] protected AttackPattern[] patterns;

    protected AttackEvent currentAttackEvent;

    protected virtual void Preparation()
    {

    }

    protected virtual void Final()
    {

    }

    protected virtual void Die()
    {
        GameVariables.GAME_OVER = true;
        TWAudioController.PlaySFX("helicopter_destroyed");
        TWAudioController.PlaySFX("helicopter_destroyed_2");

        DOTween.Sequence()
            .AppendCallback(() => { explosion.Play(); })
            .AppendCallback(() => { CameraShake.instance.Shake(explosion.main.duration, 3, 10); })
            .AppendInterval(explosion.main.duration / 2)
            .AppendCallback(() => { gameObject.SetActive(false); })
            .AppendInterval(explosion.main.duration / 2)
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
                Die();
            }
        }
    }
}


[System.Serializable] 
public class AttackPattern
{
    public string attackName;

    public AttackEvent attackEvent; 
}


