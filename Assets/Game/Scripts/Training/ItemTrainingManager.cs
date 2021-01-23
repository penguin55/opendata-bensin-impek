using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemTrainingManager : TrainingManager
{

    [SerializeField] private TLE_Item trainingItem;
    [SerializeField] private ItemData data; // Debugging purpose
    [SerializeField] private FungusController fungusController;

    private string obstacleType;

    private void Awake()
    {
        /*GameData.ActiveItem = data;*///Debugging purpose
        activeTLE = trainingItem;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameVariables.FREEZE_INPUT = false;
        CheckActiveItem();
        trainingItem.InitEventListener(obstacleType, true);
        StartDialog();
    }

    public void StartDialog()
    {
        switch (GameData.ActiveItem.itemName)
        {
            case "A Pair of Loro Blonyo":
                fungusController.NextBlock("S_Loro");
                break;
            case "Ceremonial Axe Candrasa":
                fungusController.NextBlock("S_Candrasa");
                break;
            case "Deer Sculpture":
                fungusController.NextBlock("S_Deer");
                break;
            case "Mangkunegaran Legion Helmet":
                fungusController.NextBlock("S_LegionHelmet");
                break;
            case "Pustaha Lak Lak":
                fungusController.NextBlock("S_Pustaha");
                break;
            case "Barong Tengkok":
                fungusController.NextBlock("S_Barong");
                break;
            case "Kalimantan Mask":
                fungusController.NextBlock("S_Kalimantan");
                break;
            case "Tiger Mask":
                fungusController.NextBlock("S_Tiger");
                break;
            case "Bird Mask":
                fungusController.NextBlock("S_Bird");
                break;

        }
    }

    public void BacktoPickItem()
    {
        GameVariables.DIALOG_START_MESSAGE = "ITEM_PANEL";
        GameData.ActiveItem.wasUsed = false;
        TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_IN, 1, () => SceneManager.LoadScene("dialogFungus"));
    }

    // Panggil method dibawah ini kalau mau aktifin training
    public void ActivateTraining()
    {
        trainingItem.ActivateEventListener(true);
    }

    public override void CompleteTrainingSection()
    {
        base.CompleteTrainingSection();
        trainingItem.ActivateEventListener(false);
        Debug.Log(GameData.ActiveItem.itemName);
        // Pemanggilan dialog setelah selesai training bisa taruh disini
        switch (GameData.ActiveItem.itemName)
        {
            case "A Pair of Loro Blonyo":
                fungusController.NextBlock("F_Loro");
                break;
            case "Ceremonial Axe Candrasa":
                fungusController.NextBlock("F_Candrasa");
                break;
            case "Deer Sculpture":
                fungusController.NextBlock("F_Deer");
                break;
            case "Mangkunegaran Legion Helmet":
                fungusController.NextBlock("F_LegionHelmet");
                break;
            case "Pustaha Lak Lak":
                fungusController.NextBlock("F_Pustaha");
                break;
            case "Barong Tengkok":
                fungusController.NextBlock("F_Barong");
                break;
            case "Kalimantan Mask":
                fungusController.NextBlock("F_Kalimantan");
                break;
            case "Tiger Mask":
                fungusController.NextBlock("F_Tiger");
                break;
            case "Bird Mask":
                fungusController.NextBlock("F_Bird");
                break;
        }
    }

    public override void InteruptTrainingSection()
    {
        base.InteruptTrainingSection();
        trainingItem.ActivateEventListener(false);
    }

    public override void RestartActiveTrainingSection()
    {
        TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_IN, .5f, () =>
        {
            DOTween.Complete("Shake");
            DOTween.Complete("TLEO_Missile");
            GameData.ActiveItem.wasUsed = false;
            SceneManager.LoadScene("TrainingRoom_Item");
            /*TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_OUT, .5f, () =>
            {
                if (GameData.ActiveItem.ActivateOnStart)
                {
                    GameData.ActiveItem.TakeEffect();
                    TrainingUI.instance.UpdateItemImage();
                }
                GameData.ActiveItem.wasUsed = false;
                CharaData.hp = CharaData.maxhp;
                TrainingUI.instance.UpdateLive();
                TrainingUI.instance.UpdateItemImage();
                base.RestartActiveTrainingSection();
                StartDialog();
            });*/
        }); 
    }

    private void CheckActiveItem()
    {
        switch(GameData.ActiveItem.itemName)
        {
            case "A Pair of Loro Blonyo":
                obstacleType = "missile";
                break;
            case "Ceremonial Axe Candrasa":
                obstacleType = "missile";
                break;
            case "Deer Sculpture":
                obstacleType = "missile";
                break;
            case "Mangkunegaran Legion Helmet":
                obstacleType = "missile";
                break;
            case "Pustaha Lak Lak":
                obstacleType = "missile";
                break;
            case "Bird Mask":
                obstacleType = "missile";
                break;
            case "Barong Tengkok":
                obstacleType = "missile";
                break;
            case "Kalimantan Mask":
                obstacleType = "missile";
                break;
            case "Tiger Mask":
                obstacleType = "missile";
                break;
        }
    }
}
