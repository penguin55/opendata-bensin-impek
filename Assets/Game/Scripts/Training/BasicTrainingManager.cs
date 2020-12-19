using System.Collections;
using System.Collections.Generic;
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
    }

    private void NextTraining()
    {
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

    }
}

[System.Serializable]
public class BasicTrainingData
{
    public string training_name;
    public Sprite training_button;
    public Sprite training_button_desc;
}