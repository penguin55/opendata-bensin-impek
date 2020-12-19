using Fungus;
using UnityEngine;

public class TLE_Interaction : TrainingListenerEvent
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private SpriteRenderer gateRender;
    [SerializeField] private Collider2D gatePhysic;
    [SerializeField] private Sprite gateOpen, gateClose;

    [SerializeField] private SpriteRenderer buttonRender;
    [SerializeField] private Sprite buttonNormal, buttonPressed;

    private bool interactionButtonTask;
    private bool buttonActive;

    public override void ActivateEventListener(bool flag)
    {
        base.ActivateEventListener(flag);

        if (flag) InitPreparation();
        else
        {
            gateRender.gameObject.SetActive(false);
            buttonRender.gameObject.SetActive(false);
            gatePhysic.enabled = false;
        }
    }

    public void ButtonPressed()
    {
        if (!buttonActive)
        {
            gateRender.sprite = gateOpen;
            buttonRender.sprite = buttonPressed;

            gatePhysic.enabled = false;

            CompleteEventListener(ref interactionButtonTask);
        }
    }

    protected override bool ValidateEventListener<E>(E eventListener)
    {
        return false;
    }

    protected override void CompleteEventListener<E>(ref E eventListener)
    {
        if (typeof(E) == typeof(bool))
        {
            eventListener = (E)System.Convert.ChangeType(true, typeof(E));

            buttonActive = true;
            activeEventListener = !buttonActive;
            if (buttonActive) manager.CompleteTrainingSection();
        }
    }

    private void InitPreparation()
    {
        CharaController.instance.transform.position = playerPosition.position;

        gateRender.sprite = gateClose;
        buttonRender.sprite = buttonNormal;

        gateRender.gameObject.SetActive(true);
        buttonRender.gameObject.SetActive(true);

        gatePhysic.enabled = true;
    }
}
