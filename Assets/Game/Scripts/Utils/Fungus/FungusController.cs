using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusController : MonoBehaviour
{
    [SerializeField] private Flowchart flowchart;
    [SerializeField] DialogInput dialogInput;

    public void Init()
    {
        flowchart.ExecuteBlock("MissionStart");
    }

    public void NextBlock(string blockname)
    {
        flowchart.ExecuteBlock(blockname);
    }

    public void NextDialog()
    {
        dialogInput.SetButtonClickedFlag();
    }
}
