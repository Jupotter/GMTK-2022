using System;
using System.Transactions;
using Assets.DiceCalculation;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class StandardDiceDescription : DiceDescription
{
    public TMP_Text Text;

    public int Value;
    public int Count;
    
    public override IDistribution Apply(IDistribution source)
    {
        var dice = Uniform.Distribution(1, Value).Repeat(Count);

        return Operation switch
               {
                   DiceOperation.Add      => source.Add(dice),
                   DiceOperation.Subtract => source.Substract(dice),
                   DiceOperation.Multiply => source.Multiply(dice),
                   _                      => throw new ArgumentOutOfRangeException()
               };
    }

    public override void Updated()
    {
        Text.text = $"{Count}D{Value}";

        base.Updated();
    }

    [UsedImplicitly]
    public void Increment()
    {
        Count++;
        Updated();
    }

    [UsedImplicitly]
    public void Decrement()
    {
        if (Count <= 1)
        {
            ParentList.Delete(this);
            return;
        }

        Count--;

        Updated();
    }
}