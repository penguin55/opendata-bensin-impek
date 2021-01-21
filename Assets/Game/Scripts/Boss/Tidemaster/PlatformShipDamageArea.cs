using UnityEngine;

public class PlatformShipDamageArea : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CharaController>().TakeDamage();
        }
    }
}
