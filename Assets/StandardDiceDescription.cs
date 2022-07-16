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
        var dice = Uniform.Distribution(1, Value);
        return source.Add(dice.Repeat(Count));
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