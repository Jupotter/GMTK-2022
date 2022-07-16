using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    public DisplayDistributionHistogram Display;

    public DiceList SelectedList { get; private set; }

    public DiceList DefaultList;

    public void Start()
    {
        SelectedList = DefaultList;
    }

    public void SelectDiceList(DiceList list)
    {
        SelectedList = list;
    }

    public void SelectDefaultList()
    {
        SelectedList = DefaultList;
    }

    public void UpdateDisplay()
    {
        Display.UpdateFrom(DefaultList);
    }
}
