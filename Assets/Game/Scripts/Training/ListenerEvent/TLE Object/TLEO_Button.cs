using UnityEngine;

public class TLEO_Button : MonoBehaviour
{
    [SerializeField] private TLE_Interaction interaction;

    public void Interact()
    {
        interaction.ButtonPressed();
    }
}
