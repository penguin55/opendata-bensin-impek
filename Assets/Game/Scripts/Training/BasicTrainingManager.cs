using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

public class BasicTrainingManager : MonoBehaviour
{
    [SerializeField] private BasicTrainingData[] trainingDatas;
    private BasicTrainingData activeTrainingData;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        activeTrainingData = trainingDatas[currentIndex];

        LaunchTraining();
    }

    private void NextTraining()
    {
        activeTrainingData.eventTraining.ActivateEventListener(false);

        currentIndex++;
        if (currentIndex >= trainingDatas.Length)
        {

        } else
        {
            activeTrainingData = trainingDatas[currentIndex];

            LaunchTraining();
        }
    }

    public void CompleteTrainingSection()
    {

    }

    private void LaunchTraining()
    {
        activeTrainingData.eventTraining.ActivateEventListener(true);
    }
}

[System.Serializable]
public class BasicTrainingData
{
    public string training_name;
    public Sprite training_button;
    public Sprite training_button_desc;
    public TrainingListenerEvent eventTraining;
}