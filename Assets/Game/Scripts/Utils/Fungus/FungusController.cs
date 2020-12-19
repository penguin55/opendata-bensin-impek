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
        if (GameVariables.DIALOG_START_MESSAGE.Equals("MISSION_START")) lastActiveBlock = "MissionStart";
        else if (GameVariables.DIALOG_START_MESSAGE.Equals("BOSS_PANEL")) lastActiveBlock = "pick Boss";
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

    public void ChangeClickMode(ClickMode clicked)
    {
        dialogInput.ChangeClickMode(clicked);
    }
}
