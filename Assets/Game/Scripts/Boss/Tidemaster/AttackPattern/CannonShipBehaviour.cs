using DG.Tweening;
using UnityEngine;

public class CannonShipBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject interactSign;
    [SerializeField] private ParticleSystem particleShoot;
    [SerializeField] private Animator cannonAnim;
    [SerializeField] private float modifierScaling;

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
            BossBehaviour.Instance.TakeDamage();
        }
    }
}
