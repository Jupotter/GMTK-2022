using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.DiceCalculation;
using UnityEngine;
using Single = Assets.DiceCalculation.Single;

public class DiceList : MonoBehaviour
{
    public DisplayDistributionHistogram Display;
    public List<DiceDescription>       Dices;

    public void AddPrefab(DiceDescription dice)
    {
        var instance = Instantiate(dice, this.transform);
        Add(instance);
    }

    public void Add(DiceDescription dice)
    {
        Dices.Add(dice);
        dice.SetParent(this);
        UpdateHistogram();
    }

    public void Delete(int num)
    {
        Dices.RemoveAt(num);
        UpdateHistogram();
    }

    public void Delete(DiceDescription dice)
    {
        if (!Dices.Contains(dice))
            throw new InvalidOperationException("Dice description not found in the list");
        Dices.Remove(dice);
        Destroy(dice.gameObject);
        UpdateHistogram();
    }

    public IDistribution GetDistribution()
    {
        IDistribution zero = Single.Distribution(0);
        return Dices.Aggregate(zero, (prev, cur) => cur.Apply(prev));
    }

    public void UpdateHistogram()
    {
        Display.SetDistribution(GetDistribution());
    }
}