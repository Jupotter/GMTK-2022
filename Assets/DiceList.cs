using System;
using System.Collections.Generic;
using System.Linq;
using Assets.DiceCalculation;
using UnityEngine;
using Single = Assets.DiceCalculation.Single;

public class DiceList : MonoBehaviour
{
    public  List<DiceDescription> Dices;
    private SceneContext          context;

    public void Start()
    {
        this.context = FindObjectOfType<SceneContext>();

        var preset = GetComponentsInChildren<DiceDescription>();
        foreach (var dice in preset)
        {
            Add(dice);
        }
    }

    public void AddPrefab(DiceDescription dice)
    {
        var instance = Instantiate(dice, this.transform);
        Add(instance);
    }

    public void Add(DiceDescription dice)
    {
        bool first = Dices.Count == 0;

        Dices.Add(dice);
        dice.SetParent(this, first);
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

        bool first = dice.IsFirst;
        Dices.Remove(dice);
        Destroy(dice.gameObject);

        if (first && Dices.Count > 0)
            Dices[0].IsFirst = true;
        UpdateHistogram();
    }

    public void MoveLeft(DiceDescription dice)
    {
        var index = Dices.IndexOf(dice);
        if (index == -1)
            throw new InvalidOperationException("Dice description not found in the list");

        if (index == 0)
            return;

        var newIndex = index - 1;
        MoveDice(dice, index, newIndex);
    }

    public void MoveRight(DiceDescription dice)
    {
        var index = Dices.IndexOf(dice);
        if (index == -1)
            throw new InvalidOperationException("Dice description not found in the list");

        if (index == Dices.Count - 1)
            return;

        var newIndex = index + 1;
        MoveDice(dice, index, newIndex);
    }

    private void MoveDice(DiceDescription dice, int index, int newIndex)
    {
        Dices.RemoveAt(index);
        Dices.Insert(newIndex, dice);

        dice.transform.SetSiblingIndex(newIndex);
        if (newIndex == 0 || index == 0)
        {
            Dices[0].IsFirst = true;
            Dices[1].IsFirst = false;
        }

        UpdateHistogram();
    }

    public IDistribution GetDistribution()
    {
        IDistribution zero = Single.Distribution(0);
        return Dices.Aggregate(zero, (prev, cur) => cur.Apply(prev));
    }

    public void UpdateHistogram()
    {
        context.UpdateDisplay();
    }
}
