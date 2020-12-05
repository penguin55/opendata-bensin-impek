using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusController : MonoBehaviour
{
    [SerializeField] private Flowchart flowchart;
    [SerializeField] DialogInput dialogInput;
    private string lastActiveBlock;

    public void Init()
    {
        lastActiveBlock = "MissionStart";
        flowchart.ExecuteBlock(lastActiveBlock);
    }

    public void NextBlock(string blockname)
    {
        flowchart.StopBlock(blockname);
        flowchart.ExecuteBlock(blockname);

        lastActiveBlock = blockname;
    }

    public void NextDialog()
    {
        dialogInput.SetButtonClickedFlag();
    }
}
