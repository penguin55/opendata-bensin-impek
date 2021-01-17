using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShipBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private GameObject interactSign;
    [SerializeField] private ParticleSystem particleShoot;
    [SerializeField] private Animator cannonAnim;

    private bool canInteract;

    public void Activate(bool flag)
    {
        interactSign.SetActive(flag);
        canInteract = flag;
    }

    protected void Launch()
    {
        if (canInteract)
        {

        }
    }
}
