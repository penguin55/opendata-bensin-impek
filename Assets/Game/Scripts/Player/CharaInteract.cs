using DG.Tweening;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaInteract : MonoBehaviour
{
    [HideInInspector] public GameObject projectileDetect;
    [HideInInspector] public GameObject gunDetect;
    [HideInInspector] public GameObject buttonInteract;
    [HideInInspector] public GameObject dynamiteDetect;
    [HideInInspector] public GameObject missileTidemasterDetect;
    [HideInInspector] public GameObject cannonTidemasterDetect;

    [HideInInspector] public bool cancelDynamiteInteract;

    public void DashingProjectile(Vector3 lastDirection, float dashSpeed)
    {
        if (projectileDetect)
        {
            GameObject temp = projectileDetect;
            temp.transform.parent.GetComponent<MissileBM>().DashDeactiveMissile();
            temp.GetComponent<Animator>().SetTrigger("Dash");
            temp.GetComponent<SpriteRenderer>().sortingOrder = 5;
            if (lastDirection.y > 0)
            {
                temp.transform.up = temp.transform.position - BossBehaviour.Instance.transform.position;
                float distance = Mathf.Sqrt((BossBehaviour.Instance.transform.position - temp.transform.position).sqrMagnitude);
                temp.transform.DOMove(BossBehaviour.Instance.transform.position, distance / 20f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    temp.transform.parent.GetComponent<MissileBM>().Explode();
                }).timeScale = GameTime.PlayerTimeScale;
            }
            else
            {
                temp.GetComponent<Rigidbody2D>().velocity = lastDirection * dashSpeed;
                DOVirtual.DelayedCall(2f, () => Destroy(temp)).timeScale = GameTime.PlayerTimeScale;
            }
            projectileDetect = null;
        }
    }

    public void DashingMissileTidemaster()
    {
        if (missileTidemasterDetect)
        {
            GameObject temp = missileTidemasterDetect;
            temp.GetComponent<Animator>().SetTrigger("Dash");
            temp.transform.parent.GetComponent<MissileTM>().DashDeactiveMissile();
            temp.transform.up = temp.transform.position - BossBehaviour.Instance.transform.position;
            float distance = Mathf.Sqrt((BossBehaviour.Instance.transform.position - temp.transform.position).sqrMagnitude);
            temp.transform.DOMove(BossBehaviour.Instance.transform.position, distance / 20f).SetEase(Ease.Linear).OnComplete(() =>
            {
                temp.transform.parent.GetComponent<MissileTM>().Explode();
            }).timeScale = GameTime.PlayerTimeScale;
            missileTidemasterDetect = null;
        }
    }

    public void DashingGun()
    {
        if (gunDetect)
        {
            gunDetect.GetComponent<GunInteractDetect>().Interact();
        }
    }

    public void DashingButtonInteract()
    {
        if (buttonInteract)
        {
            buttonInteract.GetComponent<TLEO_Button>().Interact();
        }
    }

    public void DashingDynamite()
    {
        if (!cancelDynamiteInteract && dynamiteDetect)
        {
            dynamiteDetect.GetComponent<DynamiteChariot>().ThrowDynamite();
            dynamiteDetect = null;
        }

        cancelDynamiteInteract = false;
    }

    public void DashingCannonTidemaster()
    {
        if (cannonTidemasterDetect)
        {
            cannonTidemasterDetect.GetComponent<CannonShipInteractDetect>().Interact();
        }
    }

    public void SetDynamite(GameObject dynamite)
    {
        dynamiteDetect = dynamite;
        cancelDynamiteInteract = true;
        DOVirtual.DelayedCall(1f, ()=> { cancelDynamiteInteract = false; }).SetId("CancellationDynamite");
    }
}
