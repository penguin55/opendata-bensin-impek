using UnityEngine;

public class GunInteractDetect : MonoBehaviour
{
    [SerializeField] private GateKeeper_Cannon gateKeeper_Cannon;

    public void Interact()
    {
        gateKeeper_Cannon.ActivateLaser(this);
    }
}
