using UnityEngine;

public class GunInteractDetect : MonoBehaviour
{
    [SerializeField] private GateKeeper_LaserDamage laserDamage;

    public void Interact()
    {
        laserDamage.ActivateLaser(this);
    }
}
