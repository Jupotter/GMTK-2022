using System.Linq;
using Assets.DiceCalculation;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    public DisplayDistribution Display;

    public DiceList SelectedList { get; private set; }

    public DiceList DefaultList;

    private IDistribution targetDistribution;

    public DiceList TargetList;

    private LevelManager levelManager;

    public void Start()
    {
        levelManager       = FindObjectOfType<LevelManager>();
        targetDistribution = TargetList.GetDistribution();
        SelectedList       = DefaultList;

        Display.SetDistribution(DefaultList.GetDistribution(), targetDistribution);
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
        var distribution = DefaultList.GetDistribution();
        if (distribution == null || targetDistribution == null)
            return;
        Display.SetDistribution(DefaultList.GetDistribution(), targetDistribution);
        CheckVictory();
    }

    public void CheckVictory()
    {
        var tested        = DefaultList.GetDistribution();
        var targetSupport = targetDistribution.Support().ToList();

        var testedSupport = tested.Support().ToList();

        foreach (var value in targetSupport)
        {
            var ok = testedSupport.Contains(value);
            testedSupport.Remove(value);
            ok &= tested.Weight(value) == targetDistribution.Weight(value);

            if (!ok)
            {
                Debug.Log("Not ok");
                return;
            }
        }

        if (testedSupport.Count > 0)
        {
            Debug.Log("Not ok");
            return;
        }

        levelManager.GoalReached();
    }
}
