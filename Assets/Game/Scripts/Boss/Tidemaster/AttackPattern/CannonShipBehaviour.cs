using DG.Tweening;
using TomWill;
using UnityEngine;

public class CannonShipBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject interactSign;
    [SerializeField] private ParticleSystem particleShoot;
    [SerializeField] private Animator cannonAnim;
    [SerializeField] private float modifierScaling;
    [SerializeField] private Vector2 movePosition;

    private Vector2 baseScale;
    private bool canInteract;
    private bool isDamaged;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    public void Activate(bool flag)
    {
        interactSign.SetActive(flag);
        canInteract = flag;
    }

    public void TakeDamage()
    {
        isDamaged = true;
        Activate(false);
        gameObject.SetActive(false);
    }

    protected void Launch()
    {
        if (canInteract && !isDamaged)
        {
            canInteract = false;
            interactSign.SetActive(canInteract);
            transform.DOPunchScale(baseScale * modifierScaling, 0.5f, 1);
            transform.DOPunchPosition(movePosition, .5f, 1);
            TWAudioController.PlaySFX("BOSS_SFX", "cannon_launch");
            particleShoot.Play();
            DOVirtual.DelayedCall(2, ()=> {
                TWAudioController.PlaySFX("BOSS_SFX", "cannon_impact");
                BossBehaviour.Instance.TakeDamage();
            });
            
        }
    }
}
