using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrainingManager : TrainingManager
{

    [SerializeField] private TLE_Item trainingItem;
    [SerializeField] private ItemData data; // Debugging purpose
    [SerializeField] private FungusController fungusController;

    private string obstacleType;


    // Start is called before the first frame update
    void Start()
    {
        /*GameData.ActiveItem = data;*///Debugging purpose
        activeTLE = trainingItem;
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
        }
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
        }
    }

    public override void InteruptTrainingSection()
    {
        base.InteruptTrainingSection();
        trainingItem.ActivateEventListener(false);
    }

    public override void RestartActiveTrainingSection()
    {
        base.RestartActiveTrainingSection();
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
        }
    }
}
