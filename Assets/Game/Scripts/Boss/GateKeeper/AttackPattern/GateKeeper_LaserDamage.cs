using DG.Tweening;
using TomWill;
using UnityEngine;
using UnityEngine.Events;

public class GateKeeper_LaserDamage : MonoBehaviour
{
    [SerializeField] private LasersGateKeeper[] lasersGateKeeper;
    [SerializeField] private float timeSegment3, timeSegment2, timeSegment1;
    private int multiplierSegment3, multiplierSegment2, multiplierSegment1;

    private GunInteractDetect activeGun;
    private int randomIndex;

    private bool canActiveLaser;
    private bool active_attack;

    public void ActivateLaser(GunInteractDetect gun)
    {
        if (canActiveLaser && gun == lasersGateKeeper[randomIndex].gun)
        {
            activeGun = gun;
            TWAudioController.PlaySFX("BOSS_SFX", "laserbeam_gettingready");
            activeGun.transform.DOPunchScale(Vector3.one * 0.25f, 0.2f, 1, 0);

            canActiveLaser = false;
            active_attack = true;

            InitMultiplier();
            OnEnter_Attack();
        }
    }

    public void ActivateInteractLaser(bool flag)
    {
        if (!active_attack)
        {
            if (flag) randomIndex = Random.Range(0, lasersGateKeeper.Length);

            canActiveLaser = flag;
            lasersGateKeeper[randomIndex].sign.SetActive(flag);
        }
    }

    private void OnEnter_Attack()
    {
        DOTween.Sequence()
            .AppendCallback(() => ScaleGun(timeSegment1 / 2f, multiplierSegment1))
            .AppendInterval(timeSegment1)
            .AppendCallback(() => ScaleGun((timeSegment2 - (multiplierSegment2 * 0.5f)) / 2, multiplierSegment2))
            .AppendInterval(timeSegment2)
            .AppendCallback(() => ScaleGun(0, multiplierSegment3))
            .AppendCallback(()=> TWAudioController.PlaySFX("BOSS_SFX", "laserbeam_ready"))
            .AppendInterval(timeSegment3)
            .OnComplete(()=>Attack());
    }

    private void Attack()
    {
        lasersGateKeeper[randomIndex].sign.SetActive(false);
        
        if (active_attack)
        {
            CameraShake.instance.Shake(1, 3, 5);
            TWAudioController.PlaySFX("BOSS_SFX", "laserbeam_firing");
            lasersGateKeeper[randomIndex].laser.SetActive(true);
            lasersGateKeeper[randomIndex].explode.Play();
        }
        canActiveLaser = false;
        DOVirtual.DelayedCall( 2f, ()=>OnExit_Attack());
    }

    private void OnExit_Attack()
    {
        lasersGateKeeper[randomIndex].laser.SetActive(false);
        active_attack = false;
    }

    private void ScaleGun(float time, int scaleMultiplier)
    {
        DOVirtual.DelayedCall( time > 0.5f ? time : 0.5f, () => activeGun.transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 1, 0).SetLoops(scaleMultiplier));
    }

    private void InitMultiplier()
    {
        multiplierSegment1 = 1;
        multiplierSegment2 = 2;
        multiplierSegment3 = (int) (timeSegment3 / 0.5f);

        timeSegment1 = Mathf.Max(multiplierSegment1 * 0.5f, timeSegment1);
        timeSegment2 = Mathf.Max(multiplierSegment2 * 0.5f, timeSegment2);
        timeSegment3 = Mathf.Max(multiplierSegment3 * 0.5f, timeSegment3);
    }
}

[System.Serializable]
public class LasersGateKeeper
{
    public GunInteractDetect gun;
    public GameObject laser;
    public GameObject sign;
    public ParticleSystem explode;
}
