using System;
using Assets.DiceCalculation;
using JetBrains.Annotations;
using TMPro;
using Single = Assets.DiceCalculation.Single;

public class ValueDiceDescription : DiceDescription
{
    public TMP_InputField Input;

    public int Value;

    public override IDistribution Apply(IDistribution source)
    {
        var dice = Single.Distribution(Value);
        return source.Add(dice);
    }

    public override void Updated()
    {
        if (int.TryParse(Input.text, out int parsed))
        {
            Value = parsed;
        }
        base.Updated();
    }
}
