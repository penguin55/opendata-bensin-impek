using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrainingManager : MonoBehaviour
{

    [SerializeField] private Tutorialmissile tutorialMissile;


    // Start is called before the first frame update
    void Start()
    {
        tutorialMissile.ExecutePattern(NullHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NullHandler()
    {

    }
}
