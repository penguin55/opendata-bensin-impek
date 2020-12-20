using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrainingManager : TrainingManager
{

    [SerializeField] private TLE_Item trainingItem;

    private string obstacleType;


    // Start is called before the first frame update
    void Start()
    {
        activeTLE = trainingItem;
        CheckActiveItem();
        trainingItem.InitEventListener(obstacleType, true);
    }

    // Panggil method dibawah ini kalau mau aktifin training
    public void ActivateTraining()
    {
        switch (GameData.ActiveItem.itemName)
        {
            case "A Pair of Loro Blonyo":
                
                break;
            case "Ceremonial Axe Candrasa":
                break;
            case "Deer Sculpture":
                break;
            case "Mangkunegaran Legion Helmet":
                break;
            case "Pustaha Lak Lak":
                break;
        }
        trainingItem.ActivateEventListener(true);
    }

    public override void CompleteTrainingSection()
    {
        base.CompleteTrainingSection();
        trainingItem.ActivateEventListener(false);

        // Pemanggilan dialog setelah selesai training bisa taruh disini
    }

    public override void InteruptTrainingSection()
    {
        base.InteruptTrainingSection();
        trainingItem.ActivateEventListener(false);
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
